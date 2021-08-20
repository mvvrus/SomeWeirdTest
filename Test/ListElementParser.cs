using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    abstract class ListElementParser
    {
        public abstract bool Recognize(StringPart Part);
        public abstract bool Parse(StringPart Part, ref bool[] AllowedValues, int MinValue, int MaxValue);

    }
}
