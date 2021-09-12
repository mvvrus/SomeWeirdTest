using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace ScheduleLibrary.Test
{
    [TestClass]
    public class AllowedDateTimePartMappedTests
    {
        [TestMethod]
        public void AllowedMappedAllValidStepTest()
        {
            AllowedDateTimePart t;

            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC,
                            TestUtils.MakeBoolMap(null, PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC), PartConsts.MSECS, new int[] { 10, 10 });
            Span<int> parts;
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 11);
            Assert.IsTrue(t.StepValue(true, ref parts));
            Assert.AreEqual(12, parts[PartConsts.MSECS]);
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 11);
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(10, parts[PartConsts.MSECS]);
        }
        [TestMethod]
        public void AllowedMappedIntrablockStepTest()
        {
            AllowedDateTimePart t;

            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC,
                            TestUtils.MakeBoolMap(new int[] { 10, 11, 12 }, PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC), PartConsts.MSECS, new int[] { 10, 10 });
            Span<int> parts;
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 11);
            Assert.IsTrue(t.StepValue(true, ref parts));
            Assert.AreEqual(12, parts[PartConsts.MSECS]);
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 11);
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(10, parts[PartConsts.MSECS]);
        }
        [TestMethod]
        public void AllowedMappedInterblockNoWrapStepTest()
        {
            AllowedDateTimePart t;

            Span<int> parts;
            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC,
                            TestUtils.MakeBoolMap(new int[] { 1, 11, 998 }, PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC), PartConsts.MSECS, new int[] { 10, 10 });
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 11);
            Assert.IsTrue(t.StepValue(true, ref parts));
            Assert.AreEqual(998, parts[PartConsts.MSECS]);
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 11);
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(1, parts[PartConsts.MSECS]);
            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC,
                            TestUtils.MakeBoolMap(new int[] { 1, 9, 11, 990, 998 }, PartConsts.FIRST_MSEC, PartConsts.LAST_MSEC), PartConsts.MSECS, new int[] { 10, 10 });
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 11);
            Assert.IsTrue(t.StepValue(true, ref parts));
            Assert.AreEqual(990, parts[PartConsts.MSECS]);
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 11);
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(9, parts[PartConsts.MSECS]);
            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC+1, PartConsts.LAST_MSEC+1,
                            TestUtils.MakeBoolMap(new int[] { 2, 10, 12, 991, 999 }, PartConsts.FIRST_MSEC+1, PartConsts.LAST_MSEC+1), PartConsts.MSECS, new int[] { 10, 10 });
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 12);
            Assert.IsTrue(t.StepValue(true, ref parts));
            Assert.AreEqual(991, parts[PartConsts.MSECS]);
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 12);
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(10, parts[PartConsts.MSECS]);
            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1,
                            TestUtils.MakeBoolMap(new int[] { 1, 2, 10, 12, 991, 1000 }, PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1), PartConsts.MSECS, new int[] { 10, 10 });
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 990);
            Assert.IsTrue(t.StepValue(true, ref parts));
            Assert.AreEqual(991, parts[PartConsts.MSECS]);
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 11);
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(10, parts[PartConsts.MSECS]);
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 991);
            Assert.IsTrue(t.StepValue(true, ref parts));
            Assert.AreEqual(1000, parts[PartConsts.MSECS]);
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 2);
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(1, parts[PartConsts.MSECS]);
        }
        [TestMethod]
        public void AllowedMappedDiffScalesNoWrapStepTest()
        {
            AllowedDateTimePart t;

            Span<int> parts;
            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1,
                            TestUtils.MakeBoolMap(new int[] { 1, 2, 17, 22, 984, 1000 }, PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1), PartConsts.MSECS, new int[] { 17, 6 });
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 983);
            Assert.IsTrue(t.StepValue(true, ref parts));
            Assert.AreEqual(984, parts[PartConsts.MSECS]);
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 22);
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(17, parts[PartConsts.MSECS]);
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 984);
            Assert.IsTrue(t.StepValue(true, ref parts));
            Assert.AreEqual(1000, parts[PartConsts.MSECS]);
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 2);
            Assert.IsTrue(t.StepValue(false, ref parts));
            Assert.AreEqual(1, parts[PartConsts.MSECS]);
        }

        [TestMethod]
        public void AllowedMappedWrapStepTest()
        {
            AllowedDateTimePart t;

            Span<int> parts;
            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1,
                            TestUtils.MakeBoolMap(new int[] { 2, 10, 12, 991, 999 }, PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1), PartConsts.MSECS, new int[] { 10, 10 });
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 999);
            Assert.IsFalse(t.StepValue(true, ref parts));
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 2);
            Assert.IsFalse(t.StepValue(false, ref parts));
            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1,
                            TestUtils.MakeBoolMap(new int[] { 1, 10, 12, 991, 1000 }, PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1), PartConsts.MSECS, new int[] { 10, 10 });
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 1000);
            Assert.IsFalse(t.StepValue(true, ref parts));
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 1);
            Assert.IsFalse(t.StepValue(false, ref parts));
            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1,
                            TestUtils.MakeBoolMap(new int[] { 501 }, PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1), PartConsts.MSECS, new int[] { 10, 10 });
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 501);
            Assert.IsFalse(t.StepValue(true, ref parts));
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 501);
            Assert.IsFalse(t.StepValue(false, ref parts));
        }
        [TestMethod]
        public void AllowedMappedDiffScalesWrapStepTest()
        {
            AllowedDateTimePart t;

            Span<int> parts;
            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1,
                            TestUtils.MakeBoolMap(new int[] { 2, 17, 22, 984, 999 }, PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1), PartConsts.MSECS, new int[] { 17, 6 });
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 999);
            Assert.IsFalse(t.StepValue(true, ref parts));
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 2);
            Assert.IsFalse(t.StepValue(false, ref parts));
            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1,
                            TestUtils.MakeBoolMap(new int[] { 1, 17, 22, 984, 1000 }, PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1), PartConsts.MSECS, new int[] { 17, 6 });
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 1000);
            Assert.IsFalse(t.StepValue(true, ref parts));
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 1);
            Assert.IsFalse(t.StepValue(false, ref parts));
            t = AllowedDateTimePartMapped.CreateDateTimePart(PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1,
                            TestUtils.MakeBoolMap(new int[] { 501 }, PartConsts.FIRST_MSEC + 1, PartConsts.LAST_MSEC + 1), PartConsts.MSECS, new int[] { 17, 6 });
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 501);
            Assert.IsFalse(t.StepValue(true, ref parts));
            parts = TestUtils.MakeValueParts(PartConsts.MSECS, 501);
            Assert.IsFalse(t.StepValue(false, ref parts));
        }
    }
}
