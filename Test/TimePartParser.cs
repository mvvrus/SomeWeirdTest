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
        static ReadOnlyMemory<char>[] _timePartsBase = new ReadOnlyMemory<char>[_partParsers.Length];

        const string STR_ZERO = "0";

        private const char MSEC_DELIM='.';

        public TimePartParser() : base(DELIM, _timePartsBase, _partParsers) { } 
        protected override bool SplitForParts(in ReadOnlyMemory<char> Part, Char Delim, ref StringPartArray SpaceForParts)
        {
            bool result = base.SplitForParts(Part, Delim, ref SpaceForParts);
            if(result && SpaceForParts.Length==BASE_PARTS ) 
            {
                int sec_pos = BASE_PARTS-1;
                int dot_pos = SpaceForParts[sec_pos].IndexOf(MSEC_DELIM);
                if (dot_pos >= 0)
                {
                    SpaceForParts.Add(SpaceForParts[sec_pos].Slice(dot_pos + 1)) ;
                    SpaceForParts[sec_pos]= SpaceForParts[sec_pos].Slice(0,dot_pos);
                }
                else SpaceForParts.Add(STR_ZERO.AsMemory());
            }
            return result;
        }

    }
}
