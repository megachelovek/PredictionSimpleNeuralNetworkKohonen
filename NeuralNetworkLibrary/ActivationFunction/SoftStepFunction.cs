using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public class SoftStepFunction : IActivationFunction
    {
        public float Function(float x)
        {
            float Y;

            Y = (float) Math.Exp(-x);
            Y = Y + 1;
            Y = (float) (1 / (double)Y);
            return Y;
        }
        
        public float Derivative(float x)
        {
            float Y;

            Y = Function(x);
            return Y * (1 - Y);
        }
    }

}
