using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringPart = System.String;

namespace Test
{
    class DatePartParser : TwoDelimParser
    {
        public const char DELIM = '.';
        static readonly PartListParserSpecifier[] _partParsers = new PartListParserSpecifier[]
        {
            new PartListParserSpecifier(PartConsts.DAYS, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH),
            new PartListParserSpecifier(PartConsts.MONTHS, PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH),
            new PartListParserSpecifier(PartConsts.YEARS, PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR)
        };

        static readonly StringPartArray _dateParts = new StringPartArray(_partParsers.Length);
        public DatePartParser() : base(DELIM, _dateParts,_partParsers) { }

    }
}
