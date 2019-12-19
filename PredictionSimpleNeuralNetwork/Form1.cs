using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NeuralNetworkLibrary;

namespace PredictionSimpleNeuralNetwork
{
    public partial class Form1 : Form
    {
        private string fileName;
        private NeuralNetwork nn;

        public Form1()
        {
            InitializeComponent();
            chartData.Series.Add("InputVectors");
            chartData.Series["InputVectors"].ChartType = SeriesChartType.Point;
            chartData.Series["InputVectors"].Color = Color.Blue;
            chartData.Series.Add("OutputVectors");
            chartData.Series["OutputVectors"].ChartType = SeriesChartType.Point;
            chartData.Series["OutputVectors"].Color = Color.Red;

        }

        private void CheckSuccessPercentage()
        {
            var success = 0;
            var fail = 0;
            for (var i = 0; i < nn.Patterns.Count; i++)
            {
                var Winner = nn.FindWinner(nn.Patterns[i]);
                if (nn.LegendaColors.FirstOrDefault(x => x.Value == nn.Classes[i]).Key ==
                    nn.ColorMatrixNn[Winner.Coordinate.X, Winner.Coordinate.Y])
                    success++;
                else
                    fail++;
            }
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
            int numberOfValue = 0;
            chartData.Series["InputVectors"].Points.Clear();
            for (var index = 0; index < nn.Patterns.Count; index++)
            {
                var pattern = nn.Patterns[index];
                for (var i = 0; i < pattern.Count; i++)
                {
                    chartData.Series["InputVectors"].Points
                        .AddXY(numberOfValue, ValidateDataForChart(pattern[i]));
                    numberOfValue++;
                }
            }

            chartData.ChartAreas[0].AxisX.Enabled = AxisEnabled.Auto;
            chartData.ChartAreas[0].AxisY.Enabled = AxisEnabled.Auto;
            chartData.ChartAreas[0].AxisY.Minimum = chartData.Series["InputVectors"].Points.FindMinByValue().YValues[0];
            chartData.ChartAreas[0].AxisY.Maximum = chartData.Series["InputVectors"].Points.FindMaxByValue().YValues[0];

        }

        private void AddPredictionValues(int window, List<double> data)
        {
            int numberOfValue = window+1;
            for (var index = 0; index < data.Count; index++)
            {
                double elem = data[index];
                chartData.Series["OutputVectors"].Points
                    .AddXY(numberOfValue, ValidateDataForChart(elem)*63);
                numberOfValue = (window+1) *(index+1);
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            var numberOfNeurons = (int)Math.Sqrt(int.Parse(countOfNeuron.Text));
            int window = (int)int.Parse(textBoxWindow.Text);
            var f = Functions.Discrete;

            var tbEpsilon2 = double.Parse(textBoxEpsilon.Text.Replace('.', ','));
            if (nn == null)
            {
                nn = new NeuralNetwork(numberOfNeurons, 0, tbEpsilon2,Functions.EuclideanMeasure,window);
                nn.Normalize = checkBoxNormalize.Checked;
            }

            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
                nn.ReadDataFromFile(ofd.FileName);
                ShowInputVectorsOnChart();
                nn.StartLearning();
                List<double> resultPrediction = nn.PredictionNextValues();
                AddPredictionValues(window, resultPrediction);
            }
        }
    }
}
