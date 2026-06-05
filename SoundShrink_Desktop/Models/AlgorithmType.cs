using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundShrink_Desktop.Models
{
    public enum AlgorithmType
    {
        NonlinearQuantization,
        DPCM,
        PredictiveDifferentialCoding,
        DeltaModulation,
        AdaptiveDeltaModulation
    }
}
