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

        static ReadOnlyMemory<char>[] _ratioPartsBase = new ReadOnlyMemory<char>[2];


        public override bool Parse(in ReadOnlyMemory<char> Part, ref bool[] AllowedValues, int MinValue, int MaxValue)
        {
            Span<ReadOnlyMemory<char>> ratio_parts_base = _ratioPartsBase.AsSpan();
            StringPartArray parts = new StringPartArray(ref ratio_parts_base);
            if (!Part.Split(SLASH, ref parts) || parts.Length != 2) return false;
            ReadOnlyMemory<char> numerator = parts[0];
            RangeParser numerator_parser = _numerator_parsers.FirstOrDefault(parser=>parser.Recognize(numerator));
            int denomerator; 
            if (numerator_parser == null || !NumberParser.NUMBER_PARSER.ParseInt(parts[1],out denomerator,2,MaxValue-MinValue) ) return false;
            int start, end;
            if (!numerator_parser.ParseRange(parts[0], out start, out end, MinValue, MaxValue)) return false;
            for (int i = start; i <= end; i += denomerator) AllowedValues[i-MinValue] = true;
            return true;
        }

        public override bool Recognize(in ReadOnlyMemory<char> Part)
        {
            return Part.IndexOf(SLASH) >= 0;
        }
    }
}
