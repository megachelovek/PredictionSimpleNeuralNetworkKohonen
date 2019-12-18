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
            chartData.ChartAreas[0].AxisX.Enabled = AxisEnabled.Auto;
            chartData.ChartAreas[0].AxisY.Enabled = AxisEnabled.Auto;
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

        private void ShowInputVectorsOnChart()
        {
            chartData.Series["InputVectors"].Points.Clear();
            foreach (var pattern in nn.Patterns) chartData.Series["InputVectors"].Points.AddXY(pattern[0].ToString()+pattern[1].ToString(), pattern[5]);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            var numberOfNeurons = (int)Math.Sqrt(int.Parse(countOfNeuron.Text));
            var window = (int)Math.Sqrt(int.Parse(textBoxWindow.Text));
            var f = Functions.Discrete;

            var tbEpsilon2 = double.Parse(textBoxEpsilon.Text.Replace('.', ','));
            if (nn == null)
            {
                nn = new NeuralNetwork(numberOfNeurons, 0, tbEpsilon2, f);
                nn.Normalize = checkBoxNormalize.Checked;
            }

            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
                nn.ReadDataFromFile(ofd.FileName);
                ShowInputVectorsOnChart();
                SwitchControls(false);
                nn.StartLearning();
                CheckSuccessPercentage();
            }
        }
    }
}
