using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class AnyParser : ListElementParser
    {
        const char AnyChar = '*';
        public override bool Parse(in StringPart Part, ref bool[] AllowedValues, int _1, int _2)
        {
            if (Part.Length == 1 && Part[0] == AnyChar) AllowedValues = null;
            else return false;
            return true;
        }

        public override bool Recognize(in StringPart Part)
        {
            return Part.IndexOf(AnyChar) >= 0;
        }
    }
}
