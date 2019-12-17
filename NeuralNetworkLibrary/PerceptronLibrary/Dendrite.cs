using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkLibrary.PerceptronLibrary
{
    public class Dendrite
    {
        public double InputPulse { get; set; }

        public double SynapticWeight { get; set; }

        public bool Learnable { get; set; } = true;
    }
}
