using System;
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

        static readonly StringPart _work_part = new StringPart(null);

        public ListParser(int Start, int End)
        {
            _start = Start;
            _end = End;
        }
        
        static StringPart AcquireWorkPart()
        {
            return _work_part;
        }

        static void ReleaseWorkPart(StringPart _1){}
        public bool Recognize(in StringPart Part)
        {
            if (Part != null)
            {
                if (Part.IndexOf(DELIM) >= 0) return true;
                foreach(var t in _listElementParsers)
                    if (t.Recognize(Part)) return true;
            }
            return false;
        }
        public bool Parse(in StringPart Part, ref bool[] AllowedList)
        {
            int list_delim_pos=-1;
            StringPart work_part = AcquireWorkPart();
            int array_length = _end - _start + 1;
            AllowedList = new bool[array_length];
            bool[] saved_list = AllowedList;  //Permanent (for the method duration) anchor to avoid AllowedList be garbage collected
            bool[] element_list=AllowedList;  //Work reference
            try {
                do
                {
                    int old_pos = list_delim_pos + 1;
                    list_delim_pos = Part.IndexOf(DELIM, old_pos);
                    StringPart element_part = Part.SubPart(old_pos, (list_delim_pos < 0 ? Part.Length : list_delim_pos), work_part);
                    ListElementParser element_parser = _listElementParsers.FirstOrDefault(curparser => curparser.Recognize(element_part));
                    if (element_parser == null) return false;
                    if (element_parser.Parse(element_part, ref element_list, _start, _end))
                    {
                        if (element_list == null )
                        {
                            element_list = saved_list;
                            AllowedList = null;
                            //but we do not break here because we want to validate the rest of this part of the schedule string
                        }
                    }
                    else return false;
                } while (list_delim_pos >= 0);
            } finally
            {
                ReleaseWorkPart(work_part);
            }
            bool result = AllowedList==null;
            for (int i = 0; !result && i < array_length; i++)
                result = result || AllowedList[i];
            return result;
        }
    }
}
