using System;
using System.Linq;
using Test;

namespace ScheduleLibrary.Test
{
    static public class TestUtils
    {
        static public bool CheckBoolMap(int[] ValuesToBeTrue, bool[] BoolMap, int Start, int End)
        {
            if (BoolMap == null)
                return ValuesToBeTrue == null;
            if (ValuesToBeTrue == null) return false;
            for(int i=0;i< ValuesToBeTrue.Length;i++)
                if (!BoolMap[ValuesToBeTrue[i] - Start]) return false;
            int allowed_count = BoolMap.Count(value => value);
            return allowed_count == ValuesToBeTrue.Length;
        }

        static public void Reset (this bool[] BoolMap)
        {
            for (int i = 0; i < BoolMap.Length; i++) BoolMap[i] = false;
        }

        static public bool CheckMapAbsence(int[] NoCheckFor, bool[][] AllowedLists)
        {
            for (int i = 0; i < AllowedLists.Length; i++)
                if (AllowedLists[i] != null && NoCheckFor.All(value => value != i)) return false;
            return true;
        }

        static public bool[] MakeBoolMap (int[] ValuesToBeTrue, int Start, int End)
        {
            if (ValuesToBeTrue == null) return null;
            bool[] result = new bool[End - Start + 1];
            foreach (int ndx in ValuesToBeTrue) result[ndx - Start] = true;
            return result;
        }

        static public int[] MakeValueParts(int Part, int Value)
        {
            int[] result = new int[PartConsts.NUM_PARTS];
            result[Part] = Value;
            return result;
        }
        static public int[] MakeValueParts(Tuple<int,int>[] PartValuePairs )
        {
            int[] result = new int[PartConsts.NUM_PARTS];
            foreach(Tuple<int,int> pair in PartValuePairs) { result[pair.Item1] = pair.Item2; }
            return result;
        }
    }
}
