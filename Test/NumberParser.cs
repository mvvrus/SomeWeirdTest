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
        public override bool Parse(StringPart Part, ref bool[] AllowedValues, int MinValue, int MaxValue)
        {
            int value;
            if (ParseInt(Part, out value, MinValue, MaxValue))
            {
                AllowedValues[value - MinValue] = true;
                return true;
            }
            else return false;
        }

        public override bool Recognize(StringPart Part)
        {
            return true; //Try to parse any string
        }

        public bool ParseInt(StringPart Part, out int Value, int MinValue, int MaxValue)
        {
            Value = 0;
            for (int i=0;i<Part.Length;i++) {
                if (!Char.IsDigit(Part[i])) return false;
                Value = Part[i] - '0'+Value*10;
                if (Value > MaxValue) return false;
            }
            return Value >= MinValue;

        }
    }
}
