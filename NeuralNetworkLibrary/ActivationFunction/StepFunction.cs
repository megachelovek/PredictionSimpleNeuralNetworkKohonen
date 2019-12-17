using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class StepFunction : IActivationFunction
    {
        public float Function(float x)
        {
            if (x >= 0)
                return 1;
            else
                return 0;
        }

        public float Function(float x, float Theta)
        {
            if (x >= Theta)
                return 1;
            else
                return 0;
        }

        public float Derivative(float x)
        {
            return Function(x);
        }
    }

}
