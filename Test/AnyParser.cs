using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class AnyParser : RangeParser
    {
        static readonly public AnyParser ANY_PARSER = new AnyParser();
        const char AnyChar = '*';
        public override bool Parse(in ReadOnlyMemory<char> Part, ref bool[] AllowedValues, int _1, int _2)
        {
            if (Part.Length == 1 && Part.Span[0] == AnyChar) AllowedValues = null;
            else return false;
            return true;
        }

        public override bool Recognize(in ReadOnlyMemory<char> Part)
        {
            return Part.IndexOf(AnyChar) >= 0;
        }

        public override bool ParseRange(in ReadOnlyMemory<char> Part, out int RangeStart, out int RangeEnd, int MinValue, int MaxValue)
        {
            RangeStart = MinValue;
            RangeEnd = MaxValue;
            return Part.Length == 1 && Part.Span[0] == AnyChar;
        }
    }
}
