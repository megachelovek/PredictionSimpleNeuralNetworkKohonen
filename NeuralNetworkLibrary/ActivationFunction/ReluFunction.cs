using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class ReluFunction : IActivationFunction
    {
        public float Function(float x)
        {
            return Math.Max(x, 0);
        }

        public float Derivative(float x)
        {
            if (x >= 0)
                return 1;
            return 0;
        }
    }
}
