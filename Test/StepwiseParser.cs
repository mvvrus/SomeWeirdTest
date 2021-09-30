﻿using System;
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
            StringPartArray space = new StringPartArray(2);
            StringPartArray parts = Part.Split(SLASH, space);
            if (parts.Overflow || parts.Length != 2) return false;
            StringPart numerator = parts[0];
            RangeParser numerator_parser = _numerator_parsers.FirstOrDefault(parser=>parser.Recognize(numerator));
            int denomerator; 
            if (numerator_parser == null || !NumberParser.NUMBER_PARSER.ParseInt(parts[1],out denomerator,2,MaxValue-MinValue) ) return false;
            int start, end;
            if (!numerator_parser.ParseRange(parts[0], out start, out end, MinValue, MaxValue)) return false;
            for (int i = start; i <= end; i += denomerator) AllowedValues[i-MinValue] = true;
            return true;
        }

        public override bool Recognize(StringPart Part)
        {
            return Part.IndexOf(SLASH) >= 0;
        }
    }
}
