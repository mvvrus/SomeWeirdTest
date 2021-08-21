﻿using System;
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
        public override bool Parse(in StringPart Part, ref bool[][] AllowedLists)
        {
            return _parser.Parse(Part, ref AllowedLists[PartConsts.DOW]);
            throw new NotImplementedException();
        }

        public override bool Recognize(in StringPart Part)
        {
            return Part.IndexOf(DatePartParser.DELIM)<0 && Part.IndexOf(TimePartParser.DELIM)<0;
        }
    }
}