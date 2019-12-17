using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class SignFunction : IActivationFunction
    {
        public float Function(float x)
        {
            if (x >= 0)
                return 1;
            else
                return -1;
        }

        public float Derivative(float x)
        {
            return 0;
        }
    }

}
