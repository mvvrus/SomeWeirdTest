using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class RangeParser : ListElementParser
    {
        static readonly public RangeParser RANGE_PARSER=new RangeParser();
        public const char DASH = '-';

        public override bool Parse(in ReadOnlyMemory<char> Part, ref bool[] AllowedValues, int MinValue, int MaxValue)
        {
            int start, end;
            bool result = this.ParseRange(Part, out start, out end, MinValue, MaxValue);
            if(result)
                for (int i = start; i <= end; i ++) AllowedValues[i-MinValue] = true;
            return result;
        }

        public override bool Recognize(in ReadOnlyMemory<char> Part)
        {
            return Part.IndexOf(DASH) >= 0;
        }

        public virtual bool ParseRange(in ReadOnlyMemory<char> Part, out int RangeStart, out int RangeEnd, int MinValue, int MaxValue)
        {
            RangeStart = RangeEnd = -1;
            bool result = true;
            int dash_pos = Part.IndexOf(DASH);
            if (dash_pos < 0) return false;
            ReadOnlyMemory<char> limit_part = Part.Slice(0, dash_pos);
            if (result)
                if (limit_part.Length == 0) RangeStart = MinValue;
                else
                    if (!NumberParser.NUMBER_PARSER.ParseInt(limit_part, out RangeStart, MinValue, MaxValue)) result = false;
            limit_part = Part.Slice(dash_pos+1);
            if (result)
                if (limit_part.Length == 0) RangeEnd = MaxValue;
                else
                    if (!NumberParser.NUMBER_PARSER.ParseInt(limit_part, out RangeEnd, MinValue, MaxValue)) result = false;

            return result;
        }
    }
}
