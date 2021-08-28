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

        [TestMethod]
        public void AllowedDowPartValueIsAllowedThisMonth_Tests()
        {
            AllowedDateTimePart t;
            DateTime base_date;
            int[] parts;
            base_date = new DateTime(2021, 8, 18);
            t = new AllowedDowPart(TestUtils.MakeBoolMap(new int[] { 4 }, PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,26),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts[PartConsts.DAYS] = 27;
            Assert.IsFalse(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,5),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts[PartConsts.DAYS] = 6;
            Assert.IsFalse(t.ValueIsAllowed(parts));
        }

        [TestMethod]
        public void AllowedDowPartValueIsAllowedThisNonLeapYear_Tests()
        {
            AllowedDateTimePart t;
            DateTime base_date;
            int[] parts;
            base_date = new DateTime(2021, 8, 28);
            t = new AllowedDowPart(TestUtils.MakeBoolMap(new int[] { 4 }, PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,15),
                new Tuple<int, int>(PartConsts.MONTHS, 4),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts[PartConsts.DAYS] = 16;
            Assert.IsFalse(t.ValueIsAllowed(parts));
            t = new AllowedDowPart(TestUtils.MakeBoolMap(new int[] { 1 }, PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,15),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts[PartConsts.DAYS] = 16;
            Assert.IsFalse(t.ValueIsAllowed(parts));

            base_date = new DateTime(2021, 1, 28);
            t = new AllowedDowPart(TestUtils.MakeBoolMap(new int[] { 1 }, PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,15),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts[PartConsts.DAYS] = 16;
            Assert.IsFalse(t.ValueIsAllowed(parts));

            t = new AllowedDowPart(TestUtils.MakeBoolMap(new int[] { 4 }, PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,15),
                new Tuple<int, int>(PartConsts.MONTHS, 4),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts[PartConsts.DAYS] = 16;
            Assert.IsFalse(t.ValueIsAllowed(parts));

        }

        [TestMethod]
        public void AllowedDowPartValueIsAllowedThisLeapYear_Tests()
        {
            AllowedDateTimePart t;
            DateTime base_date;
            int[] parts;
            base_date = new DateTime(2021, 8, 28);
            t = new AllowedDowPart(TestUtils.MakeBoolMap(new int[] { 3 }, PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,15),
                new Tuple<int, int>(PartConsts.MONTHS, 4),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts[PartConsts.DAYS] = 16;
            Assert.IsFalse(t.ValueIsAllowed(parts));
            t = new AllowedDowPart(TestUtils.MakeBoolMap(new int[] { 6 }, PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,15),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts[PartConsts.DAYS] = 16;
            Assert.IsFalse(t.ValueIsAllowed(parts));

            base_date = new DateTime(2020, 1, 28);
            t = new AllowedDowPart(TestUtils.MakeBoolMap(new int[] { 6 }, PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,15),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts[PartConsts.DAYS] = 16;
            Assert.IsFalse(t.ValueIsAllowed(parts));

            t = new AllowedDowPart(TestUtils.MakeBoolMap(new int[] { 3 }, PartConsts.FIRST_DOW, PartConsts.LAST_DOW), base_date);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,15),
                new Tuple<int, int>(PartConsts.MONTHS, 4),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts[PartConsts.DAYS] = 16;
            Assert.IsFalse(t.ValueIsAllowed(parts));
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
