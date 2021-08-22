using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
