using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StringPart = System.String;

namespace Test
{
    public struct PartListParserSpecifier
    {
        public int Index { get; }
        public ListParser Parser { get; }
        public PartListParserSpecifier(int index, int first, int last)
        {
            Index = index;
            Parser = new ListParser(first, last);
        }
    }

    public abstract class TwoDelimParser: SecondLevelParser
    {
        readonly char _delim;
        readonly StringPartArray _spaceForParts;
        readonly PartListParserSpecifier[] _partParsers;
        protected TwoDelimParser(Char Delim, StringPartArray SpaceForParts, PartListParserSpecifier[] PartParsers)
        {
            _delim = Delim;
            _spaceForParts = SpaceForParts;
            _partParsers = PartParsers;
        }

        protected virtual StringPartArray SplitForParts(StringPart Part, Char Delim, StringPartArray SpaceForParts)
        {
            StringPartArray result = Part.Split(Delim, SpaceForParts);
            if (result != null && result.Length != 3) result = null;
            return result;
        }

        public override bool Parse(StringPart Part, ref bool[][] AllowedLists)
        {
            StringPartArray parts = SplitForParts(Part, _delim, _spaceForParts);
            bool result = true;
            if (parts != null)
                for (int i = 0; i < parts.Length && result; i++)
                {
                    result = result && _partParsers[i].Parser.Parse(parts[i], ref AllowedLists[_partParsers[i].Index]);
                }
            else result = false;
            return result;
        }

        public override bool Recognize(StringPart Part)
        {
            int pos_delim_1 = Part.IndexOf(_delim);
            int pos_delim_2 = pos_delim_1 >= 0 ? Part.IndexOf(_delim, pos_delim_1 + 1) : -1;
            return pos_delim_2 >= 0;
        }
    }
}
