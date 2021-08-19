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
        static readonly ListElementParser[] _listElementParsers=new ListElementParser[] { };
        static readonly StringPart _element_part = new StringPart(null);

        public ListParser(int Start, int End)
        {
            _start = Start;
            _end = End;
        }
        
        static StringPart AcquireWorkElement()
        {
            return _element_part;
        }

        static void ReleaseWorkElement(StringPart _1){}
        public bool Parse(in StringPart Part, ref bool[] AllowedList)
        {
            int list_delim_pos=-1;
            StringPart work_part = AcquireWorkElement();
            int array_length = _end - _start + 1;
            AllowedList = new bool[array_length];
            bool[] saved_list = AllowedList;
            bool[] element_list=AllowedList;
            try {
                do
                {
                    int old_pos = list_delim_pos + 1;
                    list_delim_pos = Part.IndexOf(DELIM, old_pos);
                    StringPart element_part = Part.SubPart(old_pos, (list_delim_pos < 0 ? Part.Length : list_delim_pos), ref work_part);
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
                ReleaseWorkElement(work_part);
            }
            bool result = AllowedList!=null;
            for (int i = 0; !result && i < array_length; i++)
                result = result || AllowedList[i];
            return result;
        }
    }
}
