﻿using System;
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
        const int SEC_POS= 2;
        const string STR_ZERO = "0";

        static readonly StringPartArray _timeParts = new StringPartArray(_partParsers.Length);
        private const char MSEC_DELIM='.';

        public TimePartParser() : base(DELIM, _timeParts, _partParsers) { } 
        protected override StringPartArray SplitForParts(StringPart Part, Char Delim, StringPartArray SpaceForParts)
        {
            StringPartArray result = base.SplitForParts(Part, Delim, SpaceForParts);
            if(result!=null ) 
            {
                if (result.Length == SEC_POS + 1)
                {
                    int dot_pos = result[SEC_POS].IndexOf(MSEC_DELIM);
                    if (dot_pos >= 0)
                    {
                        result.Add(result[SEC_POS].BaseString, result[SEC_POS].Start + dot_pos + 1, result[SEC_POS].End);
                        result[SEC_POS].Truncate(dot_pos);
                    }
                    else result.Add(STR_ZERO, 0, STR_ZERO.Length);
                }
                else result = null;
            }
            return result;
        }

    }
}
