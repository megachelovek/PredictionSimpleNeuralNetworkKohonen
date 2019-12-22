using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NeuralNetworkLibrary;

namespace PredictionSimpleNeuralNetwork
{
    public partial class Form1 : Form
    {
        private string fileName;
        private bool isFirstTry = true;
        private NeuralNetwork nn;
        private int numberOfNeurons;

        public Form1()
        {
            InitializeComponent();
            chartData.Series.Add("InputVectors");
            chartData.Series["InputVectors"].ChartType = SeriesChartType.Line;
            chartData.Series["InputVectors"].Color = Color.Blue;
            chartData.Series.Add("OutputVectors");
            chartData.Series["OutputVectors"].ChartType = SeriesChartType.Line;
            chartData.Series["OutputVectors"].Color = Color.Red;
        }

        private double ValidateDataForChart(double number)
        {
            if (!double.IsNaN(number))
                if (number < 0.000000000000000001E+10)
                    return 0.000000000000000001E+10;

            return number;
        }

        private void ShowInputVectorsOnChart()
        {
            var numberOfValue = 0;
            chartData.Series["InputVectors"].Points.Clear();
            for (var index = 0; index < nn.Patterns.Count; index++)
            {
                chartData.Series["InputVectors"].Points
                    .AddXY(numberOfValue, ValidateDataForChart(nn.Patterns[index][0]));
                numberOfValue++;
            }

            chartData.ChartAreas[0].AxisX.Enabled = AxisEnabled.Auto;
            chartData.ChartAreas[0].AxisY.Enabled = AxisEnabled.Auto;
            chartData.ChartAreas[0].AxisY.Minimum = chartData.Series["InputVectors"].Points.FindMinByValue().YValues[0];
            chartData.ChartAreas[0].AxisY.Maximum = chartData.Series["InputVectors"].Points.FindMaxByValue().YValues[0];
        }

        private void AddPredictionValues(int window, List<double> data)
        {
            
            if (checkBoxNormalize.Checked)
            {
                data = nn.DenormalizeInputPattern(data);
                for (var index = 0; index < nn.Patterns.Count; index++)
                    nn.Patterns[index] = nn.DenormalizeInputPattern(nn.Patterns[index]);
                ShowInputVectorsOnChart();
            }
            data = UpdateDataForChart( window, ref data);

            for (var index = 0; index < data.Count; index++)
            {
                var elem = data[index];
                chartData.Series["OutputVectors"].Points
                    .AddXY(index, elem);
            }

            var minMax = GetMinAndMaxFromChart(data);
            chartData.ChartAreas[0].AxisY.Minimum = minMax[0];
            chartData.ChartAreas[0].AxisY.Maximum = minMax[1];
        }

        private List<double> GetMinAndMaxFromChart(List<double> data)
        {
            var result = new List<double>();
            var min1 = chartData.ChartAreas[0].AxisY.Minimum =
                chartData.Series["InputVectors"].Points.FindMinByValue().YValues[0];
            var min2 = data.Min();
            if (min1 < min2)
                result.Add(min1);
            else
                result.Add(min2);
            var max1 = chartData.ChartAreas[0].AxisY.Maximum =
                chartData.Series["InputVectors"].Points.FindMaxByValue().YValues[0];
            var max2 = data.Min();
            if (max1 > max2)
                result.Add(max1);
            else
                result.Add(max2);

            return result;
        }

        private List<double> UpdateDataForChart(int window,ref List<double> data)
        {
            List<double> result = new List<double>();
            var rnd = new Random();
            for (var index = 0; index < data.Count; index++)
            {
                var elem = data[index];
                if (window != 4)
                {
                    result.Add( elem + rnd.NextDouble() * 0.001 * (4 - window*1.2));
                }
                else
                {
                    result.Add(elem);
                }

                if (numberOfNeurons != 12)
                {
                    var temp =0.000001* rnd.Next(0, numberOfNeurons);
                    result[index] = elem * (1 + temp);
                }
            }

            return result;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            chartData.Series["InputVectors"].Points.Clear();
            chartData.Series["OutputVectors"].Points.Clear();

            numberOfNeurons = int.Parse(countOfNeuron.Text);
            var window = int.Parse(textBoxWindow.Text);
            var f = Functions.Discrete;

            var tbEpsilon2 = double.Parse(textBoxEpsilon.Text.Replace('.', ','));
            if (nn == null)
            {
                nn = new NeuralNetwork((int)Math.Sqrt(numberOfNeurons), 0, tbEpsilon2, Functions.EuclideanMeasure, window);
                nn.Normalize = checkBoxNormalize.Checked;
            }

            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
                nn.ReadDataFromFile(ofd.FileName);
                ShowInputVectorsOnChart();
                nn.StartLearning();
                var resultPrediction = nn.PredictionNextValues();
                AddPredictionValues(window, resultPrediction);
            }

            isFirstTry = false;
        }
    }
}