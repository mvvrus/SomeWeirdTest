using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringPart = System.String;


namespace Test
{
    class DayOfWeekPartParser: SecondLevelParser
    {
        static readonly ListParser _parser = new ListParser(PartConsts.FIRST_DOW, PartConsts.LAST_DOW);
        public override bool Parse(StringPart Part, ref bool[][] AllowedLists)
        {
            return _parser.Parse(Part, ref AllowedLists[PartConsts.DOW]);
        }

        public override bool Recognize(StringPart Part)
        {
            return _parser.Recognize(Part);
        }
    }
}
