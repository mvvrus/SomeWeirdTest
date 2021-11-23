﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class ListParser
    {
        const char DELIM = ',';
        readonly int _start;
        readonly int _end;
        static readonly ListElementParser[] _listElementParsers=new ListElementParser[] //Parsers for the list elements, in the order ow lowering priority
        {
            StepwiseParser.STEPWISE_PARSER,
            RangeParser.RANGE_PARSER,
            AnyParser.ANY_PARSER,
            NumberParser.NUMBER_PARSER
        };


        public ListParser(int Start, int End)
        {
            _start = Start;
            _end = End;
        }

        public bool Recognize(in ReadOnlyMemory<char> Part)
        {
            if (Part.IndexOf(DELIM) >= 0) return true;
            foreach(var t in _listElementParsers)
                if (t.Recognize(Part)) return true;
            return false;
        }
        public bool Parse(in ReadOnlyMemory<char> Part, ref bool[] AllowedList)
        {
            int list_delim_pos=-1;
            int array_length = _end - _start + 1;
            AllowedList = new bool[array_length];
            do
            {
                if (null == AllowedList) return false;
                int old_pos = list_delim_pos + 1;
                list_delim_pos = Part.IndexOf(DELIM, old_pos);
                ReadOnlyMemory<char> element_part = Part.Slice(old_pos, (list_delim_pos < 0 ? Part.Length - old_pos : list_delim_pos - old_pos));
                ListElementParser element_parser = _listElementParsers.FirstOrDefault(curparser => curparser.Recognize(element_part));
                if (element_parser == null) return false;
                if (element_parser.Parse(element_part, ref AllowedList, _start, _end))
                {
                    if (AllowedList == null && old_pos > 0) return false;
                }
                else return false;
            } while (list_delim_pos >= 0);
            bool result = AllowedList==null;
            for (int i = 0; !result && i < array_length; i++)
                result = result || AllowedList[i];
            return result;
        }
    }
}
