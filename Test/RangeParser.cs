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
        static readonly StringPart _work_part = new StringPart(null);

        static StringPart AcquireWorkPart()
        {
            return _work_part;
        }
        static void ReleaseWorkPart(StringPart _1) { }

        public override bool Parse(StringPart Part, ref bool[] AllowedValues, int MinValue, int MaxValue)
        {
            int start, end;
            bool result = this.ParseRange(Part, out start, out end, MinValue, MaxValue);
            if(result)
                for (int i = start; i <= end; i ++) AllowedValues[i-MinValue] = true;
            return result;
        }

        public override bool Recognize(StringPart Part)
        {
            return Part.IndexOf(DASH) >= 0;
        }

        public virtual bool ParseRange(StringPart Part, out int RangeStart, out int RangeEnd, int MinValue, int MaxValue)
        {
            RangeStart = RangeEnd = -1;
            bool result = true;
            int dash_pos = Part.IndexOf(DASH);
            if (dash_pos < 0) return false;
            StringPart work_part = AcquireWorkPart();
            try
            {
                StringPart limit_part = Part.SubPart(0, dash_pos, work_part);
                if (result)
                    if (limit_part.Length == 0) RangeStart = MinValue;
                    else
                        if (!NumberParser.NUMBER_PARSER.ParseInt(limit_part, out RangeStart, MinValue, MaxValue)) result = false;
                limit_part = Part.SubPart(dash_pos+1, Part.Length, work_part);
                if (result)
                    if (limit_part.Length == 0) RangeEnd = MaxValue;
                    else
                        if (!NumberParser.NUMBER_PARSER.ParseInt(limit_part, out RangeEnd, MinValue, MaxValue)) result = false;

            }
            finally { ReleaseWorkPart(work_part); }
            return result;
        }
    }
}
