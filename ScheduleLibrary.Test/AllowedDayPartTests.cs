using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace ScheduleLibrary.Test
{
    [TestClass]
    public class AllowedDayPartTests
    {
        [TestMethod]
        public void AllowedDayPartValidity_Test()
        {
            AllowedDateTimePart t;
            int[] parts;
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(null, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,28), 
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,31),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,31),
                new Tuple<int, int>(PartConsts.MONTHS, 9),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,29),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,29),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,32),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.ValueIsAllowed(parts));
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(new int[] {2,28,29,31 }, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,28),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,27),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,31),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,31),
                new Tuple<int, int>(PartConsts.MONTHS, 9),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,29),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,28),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,29),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(new int[] { 32 }, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,30),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,31),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,31),
                new Tuple<int, int>(PartConsts.MONTHS, 9),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,30),
                new Tuple<int, int>(PartConsts.MONTHS, 9),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,28),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,29),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsTrue(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,29),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.ValueIsAllowed(parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,28),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsFalse(t.ValueIsAllowed(parts));
        }

        [TestMethod]
        public void AllowedDayPartStepNoWrap_Test()
        {
            AllowedDateTimePart t;
            int[] parts;
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(null, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,28),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.StepValue(true,parts));
            Assert.AreEqual(29, parts[PartConsts.DAYS]);
            parts[PartConsts.DAYS] = 28;
            Assert.IsTrue(t.StepValue(false, parts));
            Assert.AreEqual(27, parts[PartConsts.DAYS]);
            parts[PartConsts.DAYS] = 30;
            Assert.IsTrue(t.StepValue(true, parts));
            Assert.AreEqual(31, parts[PartConsts.DAYS]);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,28),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsTrue(t.StepValue(true, parts));
            Assert.AreEqual(29, parts[PartConsts.DAYS]);
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(new int[] {2,16,27,28,29,30,31}, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,16),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.StepValue(true, parts));
            Assert.AreEqual(27, parts[PartConsts.DAYS]);
            parts[PartConsts.DAYS] = 16;
            Assert.IsTrue(t.StepValue(false, parts));
            Assert.AreEqual(2, parts[PartConsts.DAYS]);
            parts[PartConsts.DAYS] = 17;
            Assert.IsTrue(t.StepValue(true, parts));
            Assert.AreEqual(27, parts[PartConsts.DAYS]);
            parts[PartConsts.DAYS] = 17;
            Assert.IsTrue(t.StepValue(false, parts));
            Assert.AreEqual(16, parts[PartConsts.DAYS]);
            parts[PartConsts.DAYS] = 15;
            Assert.IsTrue(t.StepValue(true, parts));
            Assert.AreEqual(16, parts[PartConsts.DAYS]);
            parts[PartConsts.DAYS] = 15;
            Assert.IsTrue(t.StepValue(false, parts));
            Assert.AreEqual(2, parts[PartConsts.DAYS]);
            parts[PartConsts.DAYS] = 30;
            Assert.IsTrue(t.StepValue(true, parts));
            Assert.AreEqual(31, parts[PartConsts.DAYS]);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,28),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsTrue(t.StepValue(true, parts));
            Assert.AreEqual(29, parts[PartConsts.DAYS]);
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(new int[] { 32 }, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,16),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.StepValue(true, parts));
            Assert.AreEqual(31, parts[PartConsts.DAYS]);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,16),
                new Tuple<int, int>(PartConsts.MONTHS, 6),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.StepValue(true, parts));
            Assert.AreEqual(30, parts[PartConsts.DAYS]);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,16),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.StepValue(true, parts));
            Assert.AreEqual(28, parts[PartConsts.DAYS]);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,16),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsTrue(t.StepValue(true, parts));
            Assert.AreEqual(29, parts[PartConsts.DAYS]);
        }

        [TestMethod]
        public void AllowedDayPartStepWrap_Test()
        {
            AllowedDateTimePart t;
            int[] parts;
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(null, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,1),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.StepValue(false, parts));
            parts[PartConsts.DAYS] = 31;
            Assert.IsFalse(t.StepValue(true, parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,30),
                new Tuple<int, int>(PartConsts.MONTHS, 6),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.StepValue(true, parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,28),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.StepValue(true, parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,29),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsFalse(t.StepValue(true, parts));
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(new int[] {2}, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,2),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.StepValue(true, parts));
            parts[PartConsts.DAYS] = 2;
            Assert.IsFalse(t.StepValue(false, parts));
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(new int[] { 2, 31 }, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,2),
                new Tuple<int, int>(PartConsts.MONTHS, 6),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.StepValue(true, parts));
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(new int[] { 2, 29 }, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,2),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.StepValue(true, parts));
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(new int[] { 2, 30 }, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,2),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsFalse(t.StepValue(true, parts));
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(new int[] { 28,29,30,31,32 }, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,31),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.StepValue(true, parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,30),
                new Tuple<int, int>(PartConsts.MONTHS, 6),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.StepValue(true, parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,28),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.StepValue(true, parts));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,29),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsFalse(t.StepValue(true, parts));
        }

        [TestMethod]
        public void AllowedDayPartWrap_Test()
        {
            AllowedDateTimePart t;
            int[] parts;
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(null, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,12),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.Wrap(true, parts));
            Assert.AreEqual(1, parts[PartConsts.DAYS]);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,30),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.Wrap(true, parts));
            Assert.AreEqual(1, parts[PartConsts.DAYS]);

            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,12),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.Wrap(false, parts));
            Assert.AreEqual(31, parts[PartConsts.DAYS]);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,12),
                new Tuple<int, int>(PartConsts.MONTHS, 6),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.Wrap(false, parts));
            Assert.AreEqual(30, parts[PartConsts.DAYS]);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,12),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.Wrap(false, parts));
            Assert.AreEqual(28, parts[PartConsts.DAYS]);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,12),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsTrue(t.Wrap(false, parts));
            Assert.AreEqual(29, parts[PartConsts.DAYS]);
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(new int[] { 2,32}, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,12),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });

            Assert.IsTrue(t.Wrap(true, parts));
            Assert.AreEqual(2, parts[PartConsts.DAYS]);

            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,12),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.Wrap(false, parts));
            Assert.AreEqual(31, parts[PartConsts.DAYS]);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,12),
                new Tuple<int, int>(PartConsts.MONTHS, 6),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.Wrap(false, parts));
            Assert.AreEqual(30, parts[PartConsts.DAYS]);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,12),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsTrue(t.Wrap(false, parts));
            Assert.AreEqual(28, parts[PartConsts.DAYS]);
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,12),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2020)
            });
            Assert.IsTrue(t.Wrap(false, parts));
            Assert.AreEqual(29, parts[PartConsts.DAYS]);
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(new int[] { 2, 32 }, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,12),
                new Tuple<int, int>(PartConsts.MONTHS, 8),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });

            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(new int[] { 29 }, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            parts = TestUtils.MakeValueParts(new Tuple<int, int>[] {
                new Tuple<int,int>(PartConsts.DAYS,12),
                new Tuple<int, int>(PartConsts.MONTHS, 2),
                new Tuple<int, int>(PartConsts.YEARS, 2021)
            });
            Assert.IsFalse(t.Wrap(false, parts));
            Assert.IsFalse(t.Wrap(true, parts));

        }

        [TestMethod]
        public void AllowedDayIsCheckOnly_Test()
        {
            AllowedDateTimePart t;
            t = AllowedDayPart.CreateDateTimePart(TestUtils.MakeBoolMap(null, PartConsts.FIRST_DAY_IN_MONTH, PartConsts.LAST_DAY_IN_MONTH));
            Assert.IsFalse(t.IsDependent);
        }
    }
}
