using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NeuralNetworkLibrary.PerceptronLibrary;

namespace NeuralNetworkLibrary
{
    public delegate void EndEpochEventHandler(object sender, EndEpochEventArgs e);
    public delegate void EndIterationEventHandler(object sender, EventArgs e);

    public class NeuralNetwork
    {
        private readonly Functions function; //Функция обновления весов
        private readonly int numberOfIterations; //Количество итераций
        private readonly double valueSKO; // Среднеквадратичная ошибка
        private int currentIteration; //Текущая итерация
        private Dictionary<string, int> currentRatingClasses; // Не используется (текущий рейтинг классов)
        private bool isLearning = true; //Обучается
        private int hiddenLayerDimension;
        private int numberOfPatterns; //Количество входных векторов
        private bool isPerceptron; //Это персептрон
        private List<double> weightsLayouts;
        public Dictionary<Color, string> LegendaColors; //Привязка цветов к классам

        public List<List<Neuron>> OutputLayer { get; set; }
        public Color[,] ColorMatrixNn { get; private set; }
        public bool Normalize { get; set; }
        public List<List<double>> Patterns { get; set; }
        public List<string> Classes { get; private set; }
        public int InputLayerDimension { get; private set; }
        public int OutputLayerDimensionInitialize { get; }
        public double CurrentDelta { get; private set; }
        public SortedList<string, int> ExistentClasses { get; private set; }


        private double normalizationConst { get; set; }
        private int windowSize { get; set; }

        #region DefaultNetworkFunctions
        public NeuralNetwork(int sqrtOfCountNeurons, int numberOfIterations, double valueSko, Functions f, int windowSize)
        {
            OutputLayerDimensionInitialize = sqrtOfCountNeurons;
            currentIteration = 1;
            this.numberOfIterations = numberOfIterations;
            function = f;
            valueSKO = valueSko;
            CurrentDelta = 100;
            this.isPerceptron = false;
            this.windowSize = windowSize;
        }
        public NeuralNetwork(int sqrtOfCountNeurons, int numberOfIterations, double valueSko, bool isPerceptron=true)
        {
            this.weightsLayouts = new List<double>();
            this.OutputLayer = new List<List<Neuron>>();

            currentIteration = 1;
            this.numberOfIterations = numberOfIterations;
            valueSKO = valueSko;
            this.isPerceptron = isPerceptron;
        }

        //(3) этап Евклидова мера
        private double EuclideanCalculateVectors(List<double> Xi, List<double> Wi)
        {
            double value = 0;
            for (var i = 0; i < Xi.Count; i++)
                value += Math.Pow(Xi[i] - Wi[i], 2);
            value = Math.Sqrt(value);
            return value;
        }

        /// <summary>
        ///     Нормализация, как раз Xj = Xj / (Math.sqrt ( Math.Sum(Xj^2)))
        /// Денормализация Xj = Xj * (Math.sqrt ( Math.Sum(Xj^2))) = const
        /// </summary>
        /// <param name="pattern"></param>
        private List<double> NormalizeInputPattern(List<double> pattern)
        {
            double nn = 0;
            for (var i = 0; i < InputLayerDimension; i++) normalizationConst += pattern[i] * pattern[i];
            normalizationConst = Math.Sqrt(normalizationConst);
            for (var i = 0; i < InputLayerDimension; i++) pattern[i] /= normalizationConst;
            return pattern;
        }

        /// <summary>
        ///     Нормализация, как раз Xj = Xj / (Math.sqrt ( Math.Sum(Xj^2)))
        /// Денормализация Xj = Xj * (Math.sqrt ( Math.Sum(Xj^2))) = const
        /// </summary>
        /// <param name="pattern"></param>
        public List<double> DenormalizeInputPattern(List<double> pattern)
        {
            for (var i = 0; i < pattern.Count; i++)
            {
                pattern[i] = pattern[i] * normalizationConst;
            }

            return pattern;
        }



        /// <summary>
        ///     1 эпоха = однократная подача всех обучающих примеров
        ///     Этапы 4 и 5 67
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="iteration"></param>
        private void StartEpoch(List<double> pattern, int iteration)
        {
            var Winner = FindWinner(pattern);
            CurrentDelta = 0;
            for (var i = 0; i < OutputLayer.Count; i++)
                for (var j = 0; j < OutputLayer[0].Count; j++)
                    CurrentDelta += OutputLayer[i][j].ModifyWeights(pattern, Winner.Coordinate, currentIteration, function,
                        OutputLayer.Count * OutputLayer[0].Count);
            currentIteration++;
            CurrentDelta = Math.Abs(CurrentDelta / (OutputLayer.Count * OutputLayer[0].Count));
            var e = new EndEpochEventArgs();
            OnEndEpochEvent(e);
        }

        /// <summary>
        ///     (3 и 4) Поиск минимума, основная функция
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public Neuron FindWinner(List<double> pattern)
        {
            double D = 0;
            var Winner = OutputLayer[0][0];
            var min = EuclideanCalculateVectors(pattern, OutputLayer[0][0].Weights);
            for (var i = 0; i < OutputLayer.Count; i++)
                for (var j = 0; j < OutputLayer[i].Count; j++)
                {
                    D = EuclideanCalculateVectors(pattern, OutputLayer[i][j].Weights);
                    if (D < min)
                    {
                        min = D;
                        Winner = OutputLayer[i][j];
                    }
                }

            return Winner;
        }

        public void StartLearning()
        {
            numberOfPatterns = this.Patterns.Count;
            var iterations = 0;
            if (numberOfIterations == 0)
                while (CurrentDelta > valueSKO) //Пока не станет меньше значения
                {
                    var patternsToLearn = new List<List<double>>(numberOfPatterns);
                    foreach (var pArray in Patterns)
                        patternsToLearn.Add(pArray);
                    var randomPattern = new Random();
                    var pattern = new List<double>(InputLayerDimension);
                    for (var i = 0; i < numberOfPatterns; i++)
                    {
                        pattern = patternsToLearn[randomPattern.Next(numberOfPatterns - i)];

                        StartEpoch(pattern, i);

                        patternsToLearn.Remove(pattern);
                    }

                    iterations++;
                    OnEndIterationEvent(new EventArgs());
                }
        }


        #endregion

        #region PerceptronFunctions

        public void Build()
        {
            int i = 0;
            foreach (var layer in OutputLayer)
            {
                if (i >= OutputLayer.Count - 1)
                {
                    break;
                }

                var nextLayer = OutputLayer[i + 1];
                CreateNetwork(layer, nextLayer);

                i++;
            }
        }

        private void CreateNetwork(List<Neuron> connectingFrom, List<Neuron> connectingTo)
        {
            foreach (var from in connectingFrom)
            {
                from.Dendrites = new List<Dendrite>();
                from.Dendrites.Add(new Dendrite());
            }

            foreach (var to in connectingFrom)
            {
                to.Dendrites = new List<Dendrite>();
                for (var index = 0; index < connectingTo.Count; index++)
                {
                    var from = connectingFrom[index];
                    to.Dendrites.Add(new Dendrite() {InputPulse = from.outputPulse, SynapticWeight = weightsLayouts[index] });
                }
            }
        }

        public void AddNeuralLayer(int countNeurons, double initialWeight)//public NeuralLayer(int count, double initialWeight, string name = "")
        {
            OutputLayer.Add(new List<Neuron>());
            int lastIndex = OutputLayer.Count-1;
            weightsLayouts.Add(initialWeight);
            for (int i = 0; i < countNeurons; i++)
            {
                OutputLayer[lastIndex].Add(new Neuron(lastIndex, i, OutputLayer.Count));
                OutputLayer[lastIndex][i].Weights = new List<double>(InputLayerDimension);
                for (int k = 0; k < Patterns[0].Count; k++) OutputLayer[lastIndex][i].Weights.Add(initialWeight);
            }

            AddDendritesToAllLayers();
        }

        private void AddDendritesToAllLayers()//public void AddLayer(NeuralLayer layer)
        {
            //int dendriteCount = 1;//TODO переделать
            //if (OutputLayer.Count > 0)
            //{
            //    dendriteCount = OutputLayer.Last().Count;
            //}

            for (var index = 0; index < OutputLayer.Count; index++)
            {
                var element = OutputLayer[index];
                if (index+1 != OutputLayer.Count)
                {
                    for (var index1 = 0; index1 < OutputLayer[index].Count; index1++)
                    {
                        var neuron = OutputLayer[index][index1];
                        int dendriteCount = OutputLayer[index + 1].Count;
                        if (neuron.Dendrites == null)
                        {
                            neuron.Dendrites = new List<Dendrite>();
                        }

                        for (int i = 0; i < dendriteCount; i++)
                        {
                            neuron.Dendrites.Add(new Dendrite());
                        }
                    }
                }
            }
        }

        public void Optimize(int numberOfLayer, double learningRate, double delta)
        {
            weightsLayouts[numberOfLayer] += learningRate * delta;
            foreach (var neuron in OutputLayer[numberOfLayer])
            {
                neuron.UpdateWeights(weightsLayouts[numberOfLayer]);
            }
        }

        public void Forward(int numberOfLayer)
        {
            foreach (var neuron in OutputLayer[numberOfLayer])
            {
                neuron.Fire();
            }
        }

        public void TrainPerceptron(List<List<double>> neuralData, double[] answersNumberOfClass, int iterations, double learningRate = 0.1)
        {
            int epoch = 1;
            //Loop till the number of iterations
            while (iterations >= epoch)
            {
                //Get the input layers
                var inputLayer = OutputLayer[0];
                List<double> outputs = new List<double>();

                //Loop through the record
                for (int i = 0; i < neuralData.Count; i++)
                {
                    //Set the input data into the first layer
                    for (int j = 0; j < neuralData[i].Count; j++)
                    {
                        inputLayer[j].outputPulse = neuralData[i][j];
                    }

                    //Fire all the neurons and collect the output
                    ComputeOutput();
                    outputs.Add(OutputLayer.Last().First().outputPulse);
                }

                //Check the accuracy score against Y with the actual output
                double accuracySum = 0;
                int y_counter = 0;
                outputs.ForEach((x) => {
                    if (x == answersNumberOfClass[y_counter]) 
                    {
                        accuracySum++;
                    }

                    y_counter++;
                });

                //Optimize the synaptic weights
                OptimizeWeights(accuracySum / y_counter);
                epoch++;
            }
        }

        private void ComputeOutput()
        {
            bool first = true;
            for (int i = 0; i < OutputLayer.Count; i++)
            {
                List<Neuron> layer = OutputLayer[i];
                //Skip first layer as it is input
                if (first)
                {
                    first = false;
                    continue;
                }

                Forward(i);
            }
        }

        private void OptimizeWeights(double accuracy)
        {
            float lr = 0.1f;
            //Skip if the accuracy reached 100%
            if (accuracy == 1)
            {
                return;
            }

            if (accuracy > 1)
            {
                lr = -lr;
            }

            //Update the weights for all the layers
            for (var index = 0; index < OutputLayer.Count; index++)
            {
                Optimize(index,lr, 1);
            }
        }

        #endregion

        #region MyRegion Второстепенные функции

        private int NumberOfClasses()
        {
            ExistentClasses = new SortedList<string, int>();
            ExistentClasses.Add(Classes[0], 1);
            var k = 0;
            var d = 2;
            for (var i = 1; i < Classes.Count; i++)
            {
                k = 0;
                for (var j = 0; j < ExistentClasses.Count; j++)
                    if (ExistentClasses.IndexOfKey(Classes[i]) != -1)
                        k++;
                if (k == 0)
                {
                    ExistentClasses.Add(Classes[i], d);
                    d++;
                }
            }

            return ExistentClasses.Count;
        }

        public Color[,] ColorSOFM()
        {
            var colorMatrix = new Color[OutputLayerDimensionInitialize, OutputLayerDimensionInitialize];
            var numOfClasses = NumberOfClasses();
            var goodColors = new List<Color>();
            goodColors.Add(Color.Red);
            goodColors.Add(Color.Navy);
            goodColors.Add(Color.Green);
            goodColors.Add(Color.Yellow);
            goodColors.Add(Color.Cyan);
            goodColors.Add(Color.CornflowerBlue);
            goodColors.Add(Color.DarkOrange);
            goodColors.Add(Color.GreenYellow);
            goodColors.Add(Color.Bisque);
            var rnd = new Random();
            for (var i = 0; i < numOfClasses - 7; i++)
                goodColors.Add(Color.FromArgb(100, rnd.Next(255), rnd.Next(125) * rnd.Next(2), rnd.Next(255)));
            LegendaColors = new Dictionary<Color, string>();
            var k = 0;
            var randomColor = 0;
            var r = new Random();
            foreach (var classs in ExistentClasses)
            {
                randomColor = r.Next(goodColors.Count);
                if (!LegendaColors.ContainsKey(goodColors[randomColor]))
                    LegendaColors.Add(goodColors[randomColor], classs.Key);
                else
                    LegendaColors.Add(Color.FromArgb(100, rnd.Next(255), rnd.Next(125) * rnd.Next(2), rnd.Next(255)),
                        classs.Key);
            }
            for (var i = 0; i < OutputLayerDimensionInitialize; i++)
                for (var j = 0; j < OutputLayerDimensionInitialize; j++)
                    colorMatrix[i, j] = Color.FromKnownColor(KnownColor.ButtonFace);

            for (var i = 0; i < Patterns.Count; i++)
            {
                var n = FindWinner(Patterns[i]);
                colorMatrix[n.Coordinate.X, n.Coordinate.Y] =
                    LegendaColors.ElementAt(ExistentClasses[Classes[i]] - 1).Key;
            }

            ColorMatrixNn = colorMatrix;
            return colorMatrix;
        }

        public void CreateClassNamesForEachNeuron()
        {
            for (var index = 0; index < Patterns.Count; index++)
            {
                var oneVector = Patterns[index];
                FindWinner(oneVector).UpdateSimilarMap(Classes[index]);
            }

            foreach (var layer in OutputLayer)
            {
                foreach (var neuron in layer)
                {
                    neuron.SetUpClassNameViaSimilarMap();
                }
            }
        }


        /// <summary>
        ///     Берет данные из файла и приводит к нужному формату с которого можно считать
        /// </summary>
        /// <param name="inputDataFileName"></param>
        public void ReadDataFromFile(string inputDataFileName)
        {
            var sr = new StreamReader(inputDataFileName);
            var line = sr.ReadLine();
            var k = 0;
            for (var i = 0; i < line.Length; i++)
                if (line[i] == ' ')
                    k++;

            InputLayerDimension = k;

            k = 0;
            while (line != null)
            {
                line = sr.ReadLine();
                k++;
            }

            Patterns = new List<List<double>>(k);
            Classes = new List<string>(k);
            numberOfPatterns = k;

            List<double> pattern;

            sr = new StreamReader(inputDataFileName);
            line = sr.ReadLine();

            while (line != null && line!="")
            {
                var startPos = 0;
                var endPos = 0;
                var j = 0;
                pattern = new List<double>(InputLayerDimension); // Заполняет первый график двумя первыми числами
                pattern.Add(Convert.ToDouble(line.Substring(0, 10).Replace('.', ',')));

                if (Normalize)
                {
                    if (Patterns.Count ==2) Patterns[0] = Patterns[1];

                    pattern = NormalizeInputPattern(pattern);
                }
                Patterns.Add(pattern);

                if (line.LastIndexOf(' ') != -1)
                {
                    startPos = line.LastIndexOf(' ');
                    Classes.Add(line.Substring(startPos));
                    line = sr.ReadLine();
                }
            }
            this.Patterns = NormPredictionReadFile(this.windowSize);
            InputLayerDimension = this.Patterns[0].Count;

            sr.Close();

            currentRatingClasses = new Dictionary<string, int>();
            foreach (var cl in Classes)
                if (!currentRatingClasses.ContainsKey(cl))
                    currentRatingClasses.Add(cl, 0);

            if (OutputLayer == null )
            {
                OutputLayer = InitializeOutputLayer(OutputLayerDimensionInitialize);
            }

        }

        private List<List<Neuron>> InitializeOutputLayer(int dimension)
        {
            List<List<Neuron>> result = new List<List<Neuron>>();
            // (2) Этап Заполнение случайными весами 
            var r = new Random();
            for (var i = 0; i < dimension; i++)
            {
                result.Add(new List<Neuron>());
                for (var j = 0; j < dimension; j++)
                {
                    result[i].Add(new Neuron(i, j, dimension));
                    result[i][j].Weights = new List<double>(InputLayerDimension);
                    for (int k = 0; k < InputLayerDimension; k++) result[i][j].Weights.Add(r.NextDouble());
                }
            }
            return result;
        }

        public event EndEpochEventHandler EndEpochEvent;
        public event EndIterationEventHandler EndIterationEvent;

        protected virtual void OnEndEpochEvent(EndEpochEventArgs e)
        {
            if (EndEpochEvent != null)
                EndEpochEvent(this, e);
        }

        protected virtual void OnEndIterationEvent(EventArgs e)
        {
            if (EndIterationEvent != null)
                EndIterationEvent(this, e);
        }

        #endregion

        #region PredictionFunctions
        
        /// <summary>
        /// Скалярное произведение двух векторов (Одинаковой длины)
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public double ScalarMultiply(List<double> X, List<double> Y)
        {
            double result = 0;
            for (int i = 0; i < X.Count; ++i)
            {
                result += X[i] * Y[i];
            }
            return result;
        }

        /// <summary>
        /// Просто гиперболический тангенс
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public double HyperbolicTan(double arg)
        {
            return Math.Tanh(arg);
        }

        /// <summary>
        /// Предсказание следующего значения по предыдущему
        /// </summary>
        /// <param name="previousValue"></param>
        /// <returns></returns>
        public List<double> PredictionNextValues()
        {
            double sumResult = 0;
            List<double> result = new List<double>();
            foreach (var inputVector in Patterns)
            {
                    sumResult = TestValueMiddle(inputVector);
                result.Add(sumResult);
            }
            
            return result;
        }

        public List<double> DontUseTestPrediction(int windowSize, List<double> data)//НЕ ИСПОЛЬЗОВАТЬ
        {
            Random rnd = new Random();
            for (var index = 0; index < 10; index++)
            {
                data[index] = data[index + 1];
            }
            for (var index = 10; index < data.Count-1; index++)
            {
                data[index] *= data[index + 1] * (1 + 0.1 * rnd.Next(1, 110 - windowSize) - 2);
            }

            return data;
        }

        public List<List<double>> NormPredictionReadFile(int windowSize)
        {
            List<List<double>> newPatterns= new List<List<double>>();
            int countPatterns = Patterns.Count;
            for (var index = 0; index < countPatterns; index++)
            {
                
                List<double> newVector = new List<double>(windowSize);
                for (int i = 0; i < windowSize; i++)
                {
                    List<double> currentPreviousVector = Patterns[index];
                    index++;
                    newVector.Add(currentPreviousVector[0]);
                }
                newPatterns.Add(newVector);
                countPatterns = countPatterns - (windowSize + 1);
            }

            return newPatterns;
        }

        private double TestValueMiddle(List<double> test)
        {
            double result=0;
            foreach (var value in test)
            {
                result += value;
            }

            result = result / test.Count;
            return result;
        }

        #endregion
    }
}