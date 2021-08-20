using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class StepwiseParser : ListElementParser
    {
        private const char SLASH='/';

        static readonly RangeParser[] _numerator_parsers = new RangeParser[]
        {
            RangeParser.RANGE_PARSER,AnyParser.ANY_PARSER
        };

        static readonly public StepwiseParser STEPWISE_PARSER = new StepwiseParser();

        public override bool Parse(StringPart Part, ref bool[] AllowedValues, int MinValue, int MaxValue)
        {
            RangeParser numerator_parser = _numerator_parsers.FirstOrDefault(parser=>parser.Recognize(Part));
            int denomerator; 
            if (numerator_parser == null || !NumberParser.NUMBER_PARSER.ParseInt(Part,out denomerator,2,MaxValue-MinValue) ) return false;
            int start, end;
            if (!numerator_parser.ParseRange(Part, out start, out end, MinValue, MaxValue)) return false;
            for (int i = start; i <= end; i += denomerator) AllowedValues[i-MinValue] = true;
            return true;
        }

        public override bool Recognize(StringPart Part)
        {
            return Part.IndexOf(SLASH) >= 0;
        }
    }
}
