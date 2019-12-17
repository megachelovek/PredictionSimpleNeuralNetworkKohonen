using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary.PerceptronLibrary
{
    public class NeuralData
    {
        public List<double> Data { get; set; }

        int counter = 0;

        public NeuralData(int rows)
        {
            Data = new List<double>(rows);
        }
        
    }
}
