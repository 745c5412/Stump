
// Generated on 03/25/2013 19:24:34
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("EffectInstanceMinMax")]
    [Serializable]
    public class EffectInstanceMinMax : EffectInstance, IIndexedData
    {
        public uint min;
        public uint max;

        public uint Min
        {
            get { return min; }
            set { min = value; }
        }

        public uint Max
        {
            get { return max; }
            set { max = value; }
        }

    }
}