using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace ScheduleLibrary.Test
{
    [TestClass]
    public class AllowedDowPartTest
    {
        [TestMethod]
        public void AllowedDowPartValueIsAllowedToday_Tests()
        {
            AllowedDateTimePart t;
            DateTime base_date;
            int[] parts;
            base_date = new DateTime(2021, 8, 28);
            t = new AllowedDowPart(TestUtils.MakeBoolMap(null, PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,base_date.Day),
                new Tuple<int, int>(PartConsts.MONTHS, base_date.Month),
                new Tuple<int, int>(PartConsts.YEARS, base_date.Year)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            t = new AllowedDowPart(TestUtils.MakeBoolMap(new int[] { (int)base_date.DayOfWeek }, PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            Assert.IsTrue(t.ValueIsAllowed(parts));
            t = new AllowedDowPart(TestUtils.MakeBoolMap(new int[] { ((int)base_date.DayOfWeek + 1) % (PartConsts.LAST_DOW - PartConsts.FIRST_DOW + 1) },
                PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            Assert.IsFalse(t.ValueIsAllowed(parts));
        }

        void TestDatePairUnidir(DateTime DateBase, DateTime DateTest)
        {
            AllowedDateTimePart t;
            DateTime base_date;
            int[] parts;
            base_date = DateBase;
            t = new AllowedDowPart(TestUtils.MakeBoolMap(new int[] { (int)DateTest.DayOfWeek }, PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,DateTest.Day),
                new Tuple<int, int>(PartConsts.MONTHS, DateTest.Month),
                new Tuple<int, int>(PartConsts.YEARS, DateTest.Year)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            DateTime DateTest2 = DateTest.AddDays(1);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,DateTest2.Day),
                new Tuple<int, int>(PartConsts.MONTHS, DateTest2.Month),
                new Tuple<int, int>(PartConsts.YEARS, DateTest2.Year)
            });
            Assert.IsFalse(t.ValueIsAllowed(parts));
        }

        void TestDatePair(DateTime Date1, DateTime Date2)
        {
            TestDatePairUnidir(Date1, Date2);
            TestDatePairUnidir(Date2, Date1);
        }

        [TestMethod]
        public void AllowedDowPartValueIsAllowedThisMonth_Tests()
        {
            TestDatePair(new DateTime(2021, 8, 18), new DateTime(2021, 8, 26));
        }

        [TestMethod]
        public void AllowedDowPartValueIsAllowedThisNonLeapYear_Tests()
        {
            TestDatePair(new DateTime(2021, 8, 28), new DateTime(2021, 4, 15));
            TestDatePair(new DateTime(2021, 8, 28), new DateTime(2021, 2, 15));
            TestDatePair(new DateTime(2021, 1, 28), new DateTime(2021, 2, 15));
            TestDatePair(new DateTime(2021, 1, 28), new DateTime(2021, 4, 15));
        }

        [TestMethod]
        public void AllowedDowPartValueIsAllowedThisLeapYear_Tests()
        {
            TestDatePair(new DateTime(2020, 8, 28), new DateTime(2020, 4, 15));
            TestDatePair(new DateTime(2020, 8, 28), new DateTime(2020, 2, 15));
            TestDatePair(new DateTime(2020, 1, 28), new DateTime(2020, 2, 15));
            TestDatePair(new DateTime(2020, 1, 28), new DateTime(2020, 4, 15));
        }


        [TestMethod]
        public void AllowedDowPartValueIsAllowedANumberofYears_Tests()
        {
            TestDatePair(new DateTime(2020,4,15), new DateTime(2021, 2, 21));  //4.2020<->2.2021
            TestDatePair(new DateTime(2020, 4, 15), new DateTime(2021, 8, 21));  //4.2020<->8.2021
            TestDatePair(new DateTime(2020, 2, 15), new DateTime(2021, 8, 21));  //2.2020<->8.2021
            TestDatePair(new DateTime(2019, 8, 7), new DateTime(2020, 2, 21));  //8.2019<->2.2020
            TestDatePair(new DateTime(2019, 8, 7), new DateTime(2020, 4, 21));  //8.2019<->4.2020
            TestDatePair(new DateTime(2016, 4, 27), new DateTime(2020, 2, 20));  //4.2016<->2.2020
            TestDatePair(new DateTime(2016, 2, 27), new DateTime(2020, 2, 20));  //2.2016<->2.2020
            TestDatePair(new DateTime(2016, 2, 27), new DateTime(2020, 4, 20));  //2.2016<->4.2020
            TestDatePair(new DateTime(2016, 4, 27), new DateTime(2020, 2, 20));  //4.2016<->2.2020
            TestDatePair(new DateTime(2016, 4, 27), new DateTime(2020, 3, 20));  //4.2016<->3.2020
            TestDatePair(new DateTime(2015, 8, 22), new DateTime(2021, 4, 15));  //8.2015<->4.2021
        }

        [TestMethod]
        public void AllowedDowPartIsCheckOnly_Test()
        {
            AllowedDateTimePart t;
            t = AllowedDowPart.CreateDateTimePart(TestUtils.MakeBoolMap(null, PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Assert.IsTrue(t.IsCheckOnly);
        }

        [TestMethod]
        public void AllowedDowPartStep_Test()
        {
            AllowedDateTimePart t;
            t = AllowedDowPart.CreateDateTimePart(TestUtils.MakeBoolMap(null, PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Assert.ThrowsException<NotImplementedException>(() => t.StepValue(true, TestUtils.MakeValueParts(PartConsts.DOW, 1)));
        }

        [TestMethod]
        public void AllowedDowPartWrap_Test()
        {
            AllowedDateTimePart t;
            t = AllowedDowPart.CreateDateTimePart(TestUtils.MakeBoolMap(null, PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Assert.ThrowsException<NotImplementedException>(() => t.Wrap(true, TestUtils.MakeValueParts(PartConsts.DOW, 1)));
        }

        [TestMethod]
        public void AllowedDowPartMinimalDependentPart_Test()
        {
            AllowedDateTimePart t;
            t = AllowedDowPart.CreateDateTimePart(TestUtils.MakeBoolMap(null, PartConsts.FIRST_DOW, PartConsts.LAST_DOW));
            Assert.AreEqual(PartConsts.DAYS, t.MinimalDependentPart);
        }
    }
}
