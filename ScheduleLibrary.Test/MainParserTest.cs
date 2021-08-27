using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Test;

namespace ScheduleLibrary.Test
{
    [TestClass]
    public class MainParserTest
    {
        const char _delim = '|';
        bool[][] AllowedLists = new bool[PartConsts.NUM_PARTS][];
        void Clear()
        {
            for (int i = 0; i < AllowedLists.Length; i++) AllowedLists[i] = null;
        }

        [TestMethod]
        public void TestMainParser_GoodScheduleStings()
        {
            const string parseTests = "25.08.2021 3 18:51:50.500|25.08.2021 18:51:50.500|18:51:50.500|25.08.2021 3 18:51:50|25.08.2021 18:51:50|18:51:50";
            String[] t = parseTests.Split(_delim);
            Clear();
            Schedule.MainParser(t[0], AllowedLists);
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 500 }, AllowedLists[PartConsts.MSECS], PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 50 }, AllowedLists[PartConsts.SECS], PartConsts.FIRST_SEC, PartConsts.LAST_SEC));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 51 }, AllowedLists[PartConsts.MINUTES], PartConsts.FIRST_MIN, PartConsts.LAST_MIN));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 18 }, AllowedLists[PartConsts.HOURS], PartConsts.FIRST_HOUR, PartConsts.LAST_HOUR));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 25 }, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 8 }, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 2021 }, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 3 }, AllowedLists[PartConsts.DOW], PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Clear();
            Schedule.MainParser(t[1], AllowedLists);
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 500 }, AllowedLists[PartConsts.MSECS], PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 50 }, AllowedLists[PartConsts.SECS], PartConsts.FIRST_SEC, PartConsts.LAST_SEC));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 51 }, AllowedLists[PartConsts.MINUTES], PartConsts.FIRST_MIN, PartConsts.LAST_MIN));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 18 }, AllowedLists[PartConsts.HOURS], PartConsts.FIRST_HOUR, PartConsts.LAST_HOUR));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 25 }, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 8 }, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 2021 }, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.DOW], PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Clear();
            Schedule.MainParser(t[2], AllowedLists);
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 500 }, AllowedLists[PartConsts.MSECS], PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 50 }, AllowedLists[PartConsts.SECS], PartConsts.FIRST_SEC, PartConsts.LAST_SEC));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 51 }, AllowedLists[PartConsts.MINUTES], PartConsts.FIRST_MIN, PartConsts.LAST_MIN));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 18 }, AllowedLists[PartConsts.HOURS], PartConsts.FIRST_HOUR, PartConsts.LAST_HOUR));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.DOW], PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Clear();
            Schedule.MainParser(t[3], AllowedLists);
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 0 }, AllowedLists[PartConsts.MSECS], PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 50 }, AllowedLists[PartConsts.SECS], PartConsts.FIRST_SEC, PartConsts.LAST_SEC));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 51 }, AllowedLists[PartConsts.MINUTES], PartConsts.FIRST_MIN, PartConsts.LAST_MIN));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 18 }, AllowedLists[PartConsts.HOURS], PartConsts.FIRST_HOUR, PartConsts.LAST_HOUR));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 25 }, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 8 }, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 2021 }, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 3 }, AllowedLists[PartConsts.DOW], PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Clear();
            Schedule.MainParser(t[4], AllowedLists);
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 0 }, AllowedLists[PartConsts.MSECS], PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 50 }, AllowedLists[PartConsts.SECS], PartConsts.FIRST_SEC, PartConsts.LAST_SEC));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 51 }, AllowedLists[PartConsts.MINUTES], PartConsts.FIRST_MIN, PartConsts.LAST_MIN));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 18 }, AllowedLists[PartConsts.HOURS], PartConsts.FIRST_HOUR, PartConsts.LAST_HOUR));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 25 }, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 8 }, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 2021 }, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.DOW], PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Clear();
            Schedule.MainParser(t[5], AllowedLists);
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 0 }, AllowedLists[PartConsts.MSECS], PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 50 }, AllowedLists[PartConsts.SECS], PartConsts.FIRST_SEC, PartConsts.LAST_SEC));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 51 }, AllowedLists[PartConsts.MINUTES], PartConsts.FIRST_MIN, PartConsts.LAST_MIN));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 18 }, AllowedLists[PartConsts.HOURS], PartConsts.FIRST_HOUR, PartConsts.LAST_HOUR));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.DAYS], PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.MONTHS], PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.YEARS], PartConsts.FIRST_YEAR, PartConsts.LAST_YEAR));
            Assert.IsTrue(TestUtils.CheckBoolMap(null, AllowedLists[PartConsts.DOW], PartConsts.FIRST_DOW, PartConsts.LAST_DOW));

        }

        [TestMethod]
        public void TestMainParser_BadScheduleStings()
        {
            const string parseTests = "25.08.2021 7 18:51:50.500|25.13.221 18:51:50.500|18:60:50.500|25.08.2221 3 18:51:50|29.02.2021 18:51:50|24:51:50";
            String[] t = parseTests.Split(_delim);
            Clear();
            Assert.ThrowsException<InvalidOperationException>(() => Schedule.MainParser(t[0], AllowedLists));
            Clear();
            Assert.ThrowsException<InvalidOperationException>(() => Schedule.MainParser(t[1], AllowedLists));
            Clear();
            Assert.ThrowsException<InvalidOperationException>(() => Schedule.MainParser(t[2], AllowedLists));
            Clear();
            Assert.ThrowsException<InvalidOperationException>(() => Schedule.MainParser(t[3], AllowedLists));
            Clear();
            Assert.ThrowsException<InvalidOperationException>(() => Schedule.MainParser(t[4], AllowedLists));
            Clear();
            Assert.ThrowsException<InvalidOperationException>(() => Schedule.MainParser(t[5], AllowedLists));
        }
        [TestMethod]
        public void TestMainParser_UnParseableScheduleStings()
        {
            const string parseTests = "18:51:50.500 3 25.08.2021|18:51:50.500 25.13.2021||25.08.2021 3|2 18:51:50|an:bc:aa";
            String[] t = parseTests.Split(_delim);
            Clear();
            Assert.ThrowsException<InvalidOperationException>(() => Schedule.MainParser(t[0], AllowedLists));
            Clear();
            Assert.ThrowsException<InvalidOperationException>(() => Schedule.MainParser(t[1], AllowedLists));
            Clear();
            Assert.ThrowsException<InvalidOperationException>(() => Schedule.MainParser(t[2], AllowedLists));
            Clear();
            Assert.ThrowsException<InvalidOperationException>(() => Schedule.MainParser(t[3], AllowedLists));
            Clear();
            Assert.ThrowsException<InvalidOperationException>(() => Schedule.MainParser(t[4], AllowedLists));
            Clear();
            Assert.ThrowsException<InvalidOperationException>(() => Schedule.MainParser(t[5], AllowedLists));
        }
    }
}
