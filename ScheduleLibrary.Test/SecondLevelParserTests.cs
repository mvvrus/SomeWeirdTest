using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace ScheduleLibrary.Test
{
    [TestClass]
    public class SecondLevelParserTests
    {
        bool[][]AllowedLists=new bool[PartConsts.NUM_PARTS][];
        void Clear() 
        {
            for (int i = 0; i < AllowedLists.Length; i++) AllowedLists[i] = null;
        }

        const char _delim = '|';
        const string _dowTests = "0|3-6|0,1,2|7|11.02.21|17:35:20||*|*/4";
        int[] _dowParts = new int[] { PartConsts.DOW };

        [TestMethod]
        public void DayOfWeekPartParser_RecognizeTest()
        {
            DayOfWeekPartParser parser = new DayOfWeekPartParser();
            StringPartArray space = new StringPartArray(_dowTests.Count(c => c == _delim) + 1);
            StringPartArray t = _dowTests.Split(_delim, space);
            Assert.IsTrue(parser.Recognize(t[0])); //"0"
            Assert.IsTrue(parser.Recognize(t[1])); //"3-6"
            Assert.IsTrue(parser.Recognize(t[2])); //"0,1,2"
            Assert.IsTrue(parser.Recognize(t[3])); //"7"
            Assert.IsFalse(parser.Recognize(t[4])); //"11.02.21"
            Assert.IsFalse(parser.Recognize(t[5])); //"17:35:20"
            Assert.IsTrue(parser.Recognize(t[6])); //""
            Assert.IsTrue(parser.Recognize(t[7])); //"*"
            Assert.IsTrue(parser.Recognize(t[8])); //"*/4"
        }

        [TestMethod]
        public void DayOfWeekPartParser_ParseTest()
        {
            DayOfWeekPartParser parser = new DayOfWeekPartParser();
            StringPartArray space = new StringPartArray(_dowTests.Count(c => c == _delim) + 1);
            StringPartArray t = _dowTests.Split(_delim, space);
            Clear();
            Assert.IsTrue(parser.Parse(t[0],ref AllowedLists)); //"0"
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 0 }, AllowedLists[PartConsts.DOW], PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dowParts, AllowedLists));
            Clear();
            Assert.IsTrue(parser.Parse(t[1], ref AllowedLists)); //"3-6"
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 3,4,5,6 }, AllowedLists[PartConsts.DOW], PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dowParts, AllowedLists));
            Clear();
            Assert.IsTrue(parser.Parse(t[2], ref AllowedLists)); //"0,1,2"
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 0,1,2 }, AllowedLists[PartConsts.DOW], PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dowParts, AllowedLists));
            Clear();
            Assert.IsFalse(parser.Parse(t[3], ref AllowedLists)); //"7"
            Clear();
            Assert.IsFalse(parser.Parse(t[4], ref AllowedLists)); //"11.02.21"
            Clear();
            Assert.IsFalse(parser.Parse(t[5], ref AllowedLists)); //"17:35:20"
            Clear();
            Assert.IsFalse(parser.Parse(t[6], ref AllowedLists)); //""
            Clear();
            Assert.IsTrue(parser.Parse(t[7], ref AllowedLists)); //""
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.DOW], PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dowParts, AllowedLists));
            Clear();
            Assert.IsTrue(parser.Parse(t[8], ref AllowedLists)); //"*/4"
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 0, 4 }, AllowedLists[PartConsts.DOW], PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dowParts, AllowedLists));
        }

        const string _dateTests = "01.02.2021|10-13.10-11.2020-2021|1,12,21.8,11.2013,2020|*.*.*|5-20/5.*/4.2011-2021/2"+
            "|31.06.2022|31.6-10/4.2021|31.*.2012|31.2-6/2.2020|29-31.02.2021-2029/4|29-30.02.2021-2025|29-32.02.2021"+
            "|0.10.2021|10.13.2021|12.04.1961|11.02|11||.11.2024|01..2024|05.12.|a.11.2024|01.b.2024|05.12.c|*.b.2024|05.*.c";
        int[] _dateParts = new int[] { PartConsts.YEARS, PartConsts.MONTHS, PartConsts.DAYS };

        [TestMethod]
        public void DatePartParser_RecognizeTest()
        {
            DatePartParser parser = new DatePartParser();
            StringPartArray space = new StringPartArray(_dateTests.Count(c => c == _delim) + 1);
            StringPartArray t = _dateTests.Split(_delim, space);
            Assert.IsTrue(parser.Recognize(t[0])); //"01.02.2021"
            Assert.IsTrue(parser.Recognize(t[1])); //"10-13.10-11.2020-2021"
            Assert.IsTrue(parser.Recognize(t[2])); //"1,12,21.8,11.2013,2020"
            Assert.IsTrue(parser.Recognize(t[3])); //"*.*.*"
            Assert.IsTrue(parser.Recognize(t[4])); //"5-20/5.*/4.2011-2021/2"
            Assert.IsTrue(parser.Recognize(t[5])); //"31.06.2022"
            Assert.IsTrue(parser.Recognize(t[6])); //"31.6-10/4.2021"
            Assert.IsTrue(parser.Recognize(t[7])); //"31.*.2012"
            Assert.IsTrue(parser.Recognize(t[8])); //"31.2-6/2.2020"
            Assert.IsTrue(parser.Recognize(t[9])); //"29-31.02.2021-2029/4"
            Assert.IsTrue(parser.Recognize(t[10])); //"29-30.02.2021-2025"
            Assert.IsTrue(parser.Recognize(t[11])); //"29-32.02.2021"
            Assert.IsTrue(parser.Recognize(t[12])); //"0.10.2021"
            Assert.IsTrue(parser.Recognize(t[13])); //"10.13.2021"
            Assert.IsTrue(parser.Recognize(t[14])); //"12.04.1961"
            Assert.IsFalse(parser.Recognize(t[15])); //"11.02"
            Assert.IsFalse(parser.Recognize(t[16])); //"11"
            Assert.IsFalse(parser.Recognize(t[17])); //""
            Assert.IsTrue(parser.Recognize(t[18])); //".11.2024"
            Assert.IsTrue(parser.Recognize(t[19])); //"01..2024"
            Assert.IsTrue(parser.Recognize(t[20])); //"05.12."
            Assert.IsTrue(parser.Recognize(t[21])); //"a.11.2024"
            Assert.IsTrue(parser.Recognize(t[22])); //"01.b.2024"
            Assert.IsTrue(parser.Recognize(t[23])); //"05.12.c"
            Assert.IsTrue(parser.Recognize(t[24])); //"*.b.2024"
            Assert.IsTrue(parser.Recognize(t[25])); //"05.*.c"
        }
        [TestMethod]
        public void DatePartParser_ParseTest()
        {
            DatePartParser parser = new DatePartParser();
            StringPartArray space = new StringPartArray(_dateTests.Count(c => c == _delim) + 1);
            StringPartArray t = _dateTests.Split(_delim, space);
            Clear();
            Assert.IsTrue(parser.Parse(t[0], ref AllowedLists)); //"01.02.2021"
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dateParts, AllowedLists));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {1}, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {2}, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {2021}, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Clear();
            Assert.IsTrue(parser.Parse(t[1], ref AllowedLists)); //"10-13.10-11.2020-2021"
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dateParts, AllowedLists));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {10,11,12,13}, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {10,11}, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {2020,2021}, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Clear();
            Assert.IsTrue(parser.Parse(t[2], ref AllowedLists)); //"1,12,21.8,11.2013,2020"
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dateParts, AllowedLists));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {1,12,21}, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {8,11}, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {2013,2020}, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Clear();
            Assert.IsTrue(parser.Parse(t[3], ref AllowedLists)); //"*.*.*"
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dateParts, AllowedLists));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Clear();
            Assert.IsTrue(parser.Parse(t[4], ref AllowedLists)); //"5-20/5.*/4.2011-2021/2"
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dateParts, AllowedLists));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {5,10,15,20}, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {1,5,9}, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {2011,2013,2015,2017,2019,2021}, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Clear();
            Assert.IsFalse(parser.Parse(t[5], ref AllowedLists)); //"31.06.2022"
            Clear();
            Assert.IsTrue(parser.Parse(t[6], ref AllowedLists)); //"31.6-10/4.2021"
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dateParts, AllowedLists));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {31}, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {6,10}, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {2021}, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Clear();
            Assert.IsTrue(parser.Parse(t[7], ref AllowedLists)); //"31.*.2012"
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dateParts, AllowedLists));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {31}, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {2012}, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Clear();
            Assert.IsFalse(parser.Parse(t[8], ref AllowedLists)); //"31.2-6/2.2020"
            Clear();
            Assert.IsFalse(parser.Parse(t[9], ref AllowedLists)); //"29-31.02.2021-2029/4"
            Clear();
            Assert.IsTrue(parser.Parse(t[10], ref AllowedLists)); //"29-30.02.2021-2025"
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dateParts, AllowedLists));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {29,30}, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {2}, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {2021,2022,2023,2024,2025}, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Clear();
            Assert.IsTrue(parser.Parse(t[11], ref AllowedLists)); //"29-32.02.2021"
            Assert.IsTrue(TestUtils.CheckMapAbsence(_dateParts, AllowedLists));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {29,30,31,32}, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {2}, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {2021}, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Clear();
            Assert.IsFalse(parser.Parse(t[12], ref AllowedLists)); //"0.10.2021"
            Clear();
            Assert.IsFalse(parser.Parse(t[13], ref AllowedLists)); //"10.13.2021"
            Clear();
            Assert.IsFalse(parser.Parse(t[14], ref AllowedLists)); //"12.04.1961"
            Clear();
            Assert.IsFalse(parser.Parse(t[15], ref AllowedLists)); //"11.02"
            Clear();
            Assert.IsFalse(parser.Parse(t[16], ref AllowedLists)); //"11"
            Clear();
            Assert.IsFalse(parser.Parse(t[17], ref AllowedLists)); //""
            Clear();
            Assert.IsFalse(parser.Parse(t[18], ref AllowedLists)); //".11.2024"
            Clear();
            Assert.IsFalse(parser.Parse(t[19], ref AllowedLists)); //"01..2024"
            Clear();
            Assert.IsFalse(parser.Parse(t[20], ref AllowedLists)); //"05.12."
            Clear();
            Assert.IsFalse(parser.Parse(t[21], ref AllowedLists)); //"a.11.2024"
            Clear();
            Assert.IsFalse(parser.Parse(t[22], ref AllowedLists)); //"01.b.2024"
            Clear();
            Assert.IsFalse(parser.Parse(t[23], ref AllowedLists)); //"05.12.c"
            Clear();
            Assert.IsFalse(parser.Parse(t[24], ref AllowedLists)); //"*.b.2024"
            Clear();
            Assert.IsFalse(parser.Parse(t[25], ref AllowedLists)); //"05.*.c"
        }
    }
}
