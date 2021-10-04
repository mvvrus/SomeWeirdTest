using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class NumberParser : ListElementParser
    {
        static readonly public NumberParser NUMBER_PARSER = new NumberParser();
        public override bool Parse(in ReadOnlyMemory<char> Part, ref bool[] AllowedValues, int MinValue, int MaxValue)
        {
            int value;
            if (ParseInt(Part, out value, MinValue, MaxValue))
            {
                AllowedValues[value - MinValue] = true;
                return true;
            }
            else return false;
        }

        public override bool Recognize(in ReadOnlyMemory<char> Part)
        {
            if (Part.Length > 0)
                for (int i = 0; i < Part.Length; i++)
                {
                    if (!Char.IsDigit(Part.Span[i])) return false;
                }
            else return false;
            return true;
        }

        public bool ParseInt(in ReadOnlyMemory<char> Part, out int Value, int MinValue, int MaxValue)
        {
            Value = 0;
            if (Part.Length <= 0) return false;
            for (int i=0;i<Part.Length;i++) {
                if (!Char.IsDigit(Part.Span[i])) return false;
                Value = Part.Span[i] - '0'+Value*10;
                if (Value > MaxValue) return false;
            }
            return Value >= MinValue;
        }
    }
}
