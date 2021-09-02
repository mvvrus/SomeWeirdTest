using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace ScheduleLibrary.Test
{
    [TestClass]
    public class AllowedDateTimePartTests
    {
        void CheckTrue(AllowedDateTimePart AllowedPart, int PartNo, int Value)
        {
            Span<int> part_value;
            part_value = TestUtils.MakeValueParts(PartNo,Value);
            Assert.IsTrue(AllowedPart.ValueIsAllowed(ref part_value));
        }

        void CheckFalse(AllowedDateTimePart AllowedPart, int PartNo, int Value)
        {
            Span<int> part_value;
            part_value = TestUtils.MakeValueParts(PartNo, Value);
            Assert.IsFalse(AllowedPart.ValueIsAllowed(ref part_value));
        }
        [TestMethod]
        public void AllowedDateTimePartValidity_Test()
        {
            AllowedDateTimePart t = AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH,
                TestUtils.MakeBoolMap(null, PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH), PartConsts.MONTHS);
            CheckTrue(t,PartConsts.MONTHS, 11);
            t = AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH,
                TestUtils.MakeBoolMap(new int[] { 2, 11 }, PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH), PartConsts.MONTHS);
            CheckTrue(t, PartConsts.MONTHS, 11);
            t = AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH,
                TestUtils.MakeBoolMap(new int[] { 2, 10 }, PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH), PartConsts.MONTHS);
            CheckFalse(t, PartConsts.MONTHS, 11);
        }
        [TestMethod]
        public void AllowedDateTimePartStepNoWrap_Test()
        {
            AllowedDateTimePart t;
            t = AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH,
                TestUtils.MakeBoolMap(null, PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH), PartConsts.MONTHS);
            Span<int> parts = TestUtils.MakeValueParts(PartConsts.MONTHS, 11);
            Assert.IsTrue(t.StepValue(true,ref parts));
            Assert.AreEqual(12, parts[PartConsts.MONTHS]);
            parts[PartConsts.MONTHS]= 2;
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(1, parts[PartConsts.MONTHS]);
            t = AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH,
                TestUtils.MakeBoolMap(new int[] { 2, 6, 11 }, PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH), PartConsts.MONTHS);
            parts[PartConsts.MONTHS] = 6;
            Assert.IsTrue(t.StepValue(true, ref parts));
            Assert.AreEqual(11, parts[PartConsts.MONTHS]);
            parts[PartConsts.MONTHS] = 6;
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(2, parts[PartConsts.MONTHS]);
            parts[PartConsts.MONTHS] = 5;
            Assert.IsTrue(t.StepValue(true, ref parts));
            Assert.AreEqual(6, parts[PartConsts.MONTHS]);
            parts[PartConsts.MONTHS] = 5;
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(2, parts[PartConsts.MONTHS]);
            parts[PartConsts.MONTHS] = 7;
            Assert.IsTrue(t.StepValue(true, ref parts));
            Assert.AreEqual(11, parts[PartConsts.MONTHS]);
            parts[PartConsts.MONTHS] = 7;
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(6, parts[PartConsts.MONTHS]);
        }

        [TestMethod]
        public void AllowedDateTimePartStepWrap_Test()
        {
            AllowedDateTimePart t;
            t = AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH,
                TestUtils.MakeBoolMap(null, PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH), PartConsts.MONTHS);
            Span<int> parts = TestUtils.MakeValueParts(PartConsts.MONTHS, 12);
            Assert.IsFalse(t.StepValue(true, ref parts));
            Assert.AreEqual(1, parts[PartConsts.MONTHS]);
            parts[PartConsts.MONTHS] = 1;
            Assert.IsFalse(t.StepValue(false, ref parts));
            Assert.AreEqual(12, parts[PartConsts.MONTHS]);
            t = AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH,
                TestUtils.MakeBoolMap(new int[] { 2, 6, 11 }, PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH), PartConsts.MONTHS);
            parts[PartConsts.MONTHS] = 11;
            Assert.IsFalse(t.StepValue(true, ref parts));
            Assert.AreEqual(2, parts[PartConsts.MONTHS]);
            parts[PartConsts.MONTHS] = 2;
            Assert.IsFalse(t.StepValue(false, ref parts));
            Assert.AreEqual(11, parts[PartConsts.MONTHS]);
        }

        [TestMethod]
        public void AllowedDateTimePartWrap_Test()
        {
            AllowedDateTimePart t;
            t = AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH,
                TestUtils.MakeBoolMap(null, PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH), PartConsts.MONTHS);
            Span<int> parts = TestUtils.MakeValueParts(PartConsts.MONTHS, 11);
            Assert.IsTrue(t.Wrap(true, ref parts));
            Assert.AreEqual(1, parts[PartConsts.MONTHS]);
            Assert.IsTrue(t.Wrap(false, ref parts));
            Assert.AreEqual(12, parts[PartConsts.MONTHS]);
            t = AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH,
                TestUtils.MakeBoolMap(new int[] { 2, 6, 11 }, PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH), PartConsts.MONTHS);
            Assert.IsTrue(t.Wrap(true, ref parts));
            Assert.AreEqual(2, parts[PartConsts.MONTHS]);
            Assert.IsTrue(t.Wrap(false, ref parts));
            Assert.AreEqual(11, parts[PartConsts.MONTHS]);
        }

        [TestMethod]
        public void AllowedDateTimePartIsCheckOnly_Test()
        {
            AllowedDateTimePart t;
            t = AllowedDateTimePart.CreateDateTimePart(PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH,
                TestUtils.MakeBoolMap(null, PartConsts.FIRST_MONTH, PartConsts.LAST_MONTH), PartConsts.MONTHS);
            Assert.IsFalse(t.IsCheckOnly);
        }
    }

}
