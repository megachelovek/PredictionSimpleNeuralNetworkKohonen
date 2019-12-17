using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary
{
    public interface IActivationFunction
    {
        float Function(float x);
        float Derivative(float x);
    }
}
