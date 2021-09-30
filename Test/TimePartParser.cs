using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringPart = System.String;

namespace Test
{
    class TimePartParser : TwoDelimParser
    {
        public const char DELIM = ':';
        static readonly PartListParserSpecifier[] _partParsers = new PartListParserSpecifier[]
        {
            new PartListParserSpecifier(PartConsts.HOURS, PartConsts.FIRST_HOUR, PartConsts.LAST_HOUR),
            new PartListParserSpecifier(PartConsts.MINUTES, PartConsts.FIRST_MIN, PartConsts.LAST_MIN),
            new PartListParserSpecifier(PartConsts.SECS, PartConsts.FIRST_SEC, PartConsts.LAST_SEC),
            new PartListParserSpecifier(PartConsts.MSECS, PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC)
        };
        const string STR_ZERO = "0";

        private const char MSEC_DELIM='.';

        public TimePartParser() : base(DELIM, _partParsers.Length, _partParsers) { } 
        protected override StringPartArray SplitForParts(StringPart Part, Char Delim, ref StringPartArray SpaceForParts)
        {
            StringPartArray result = base.SplitForParts(Part, Delim, ref SpaceForParts);
            if(!result.Overflow && result.Length==BASE_PARTS ) 
            {
                int sec_pos = BASE_PARTS-1;
                int dot_pos = result[sec_pos].IndexOf(MSEC_DELIM);
                if (dot_pos >= 0)
                {
                    result.Add(result[sec_pos].BaseString, result[sec_pos].Start + dot_pos + 1, result[sec_pos].End);
                    result[sec_pos].Truncate(dot_pos);
                }
                else result.Add(STR_ZERO, 0, STR_ZERO.Length);
            }
            return result;
        }

    }
}
