using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class IdentityFunction : IActivationFunction
    {
        public float Function(float x)
        {
            return x;
        }

        public float Derivative(float x)
        {
            return 1;
        }
    }
}
