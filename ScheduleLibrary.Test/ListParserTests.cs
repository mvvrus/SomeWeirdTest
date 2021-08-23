using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace ScheduleLibrary.Test
{
    [TestClass]
    public class ListParserTests
    {
        [TestMethod]
        public void NumberParser_RecognizeTest()
        {
            StringPartArray space = new StringPartArray(3);
            StringPartArray t = "10,abc,".Split(',', space);
            Assert.IsTrue(NumberParser.NUMBER_PARSER.Recognize(t[0]));
            Assert.IsTrue(NumberParser.NUMBER_PARSER.Recognize(t[1]));
            Assert.IsFalse(NumberParser.NUMBER_PARSER.Recognize(t[2]));
        }
        [TestMethod]
        public void NumberParser_ParseIntTest()
        {
            int Value;
            StringPartArray space = new StringPartArray(9);
            StringPartArray t = "1,25,01,101, 1,1 ,,abc,1a".Split(',', space);
            Assert.IsTrue(NumberParser.NUMBER_PARSER.ParseInt(t[0], out Value, 0, 26));
            Assert.AreEqual(1, Value);
            Assert.IsTrue(NumberParser.NUMBER_PARSER.ParseInt(t[0], out Value, 1, 26));
            Assert.IsFalse(NumberParser.NUMBER_PARSER.ParseInt(t[0], out Value, 2, 26));
            Assert.IsTrue(NumberParser.NUMBER_PARSER.ParseInt(t[1], out Value, 0, 26));
            Assert.AreEqual(25, Value);
            Assert.IsTrue(NumberParser.NUMBER_PARSER.ParseInt(t[1], out Value, 0, 25));
            Assert.IsFalse(NumberParser.NUMBER_PARSER.ParseInt(t[1], out Value, 0, 24));
            Assert.IsTrue(NumberParser.NUMBER_PARSER.ParseInt(t[2], out Value, 0, 26));
            Assert.AreEqual(1, Value);
            Assert.IsTrue(NumberParser.NUMBER_PARSER.ParseInt(t[3], out Value, 0, 126));
            Assert.AreEqual(101, Value);
            Assert.IsFalse(NumberParser.NUMBER_PARSER.ParseInt(t[4], out Value, 0, 126));
            Assert.IsFalse(NumberParser.NUMBER_PARSER.ParseInt(t[5], out Value, 0, 126));
            Assert.IsFalse(NumberParser.NUMBER_PARSER.ParseInt(t[6], out Value, 0, 126));
            Assert.IsFalse(NumberParser.NUMBER_PARSER.ParseInt(t[7], out Value, 0, 126));
            Assert.IsFalse(NumberParser.NUMBER_PARSER.ParseInt(t[8], out Value, 0, 126));
        }

        [TestMethod]
        public void NumberParser_ParseTest()
        {
            StringPartArray space = new StringPartArray(10);
            StringPartArray t = "0,1,5,26,27, 1,1 ,,abc,1a".Split(',', space);
            bool[] BoolMap = new bool[26];
            bool[] SavedBoolMap = BoolMap;
            Assert.IsFalse(NumberParser.NUMBER_PARSER.Parse(t[0], ref BoolMap, 1, 26));
            BoolMap = SavedBoolMap;
            BoolMap.Reset();
            Assert.IsTrue(NumberParser.NUMBER_PARSER.Parse(t[1], ref BoolMap, 1, 26));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 1 }, BoolMap, 1, 26));
            BoolMap = SavedBoolMap;
            BoolMap.Reset();
            Assert.IsTrue(NumberParser.NUMBER_PARSER.Parse(t[2], ref BoolMap, 1, 26));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 5 }, BoolMap, 1, 26));
            BoolMap = SavedBoolMap;
            BoolMap.Reset();
            Assert.IsTrue(NumberParser.NUMBER_PARSER.Parse(t[3], ref BoolMap, 1, 26));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 26 }, BoolMap, 1, 26));
            BoolMap = SavedBoolMap;
            BoolMap.Reset();
            Assert.IsFalse(NumberParser.NUMBER_PARSER.Parse(t[4], ref BoolMap, 1, 26));
            BoolMap = SavedBoolMap;
            BoolMap.Reset();
            Assert.IsFalse(NumberParser.NUMBER_PARSER.Parse(t[5], ref BoolMap, 1, 26));
            BoolMap = SavedBoolMap;
            BoolMap.Reset();
            Assert.IsFalse(NumberParser.NUMBER_PARSER.Parse(t[6], ref BoolMap, 1, 26));
            BoolMap = SavedBoolMap;
            BoolMap.Reset();
            Assert.IsFalse(NumberParser.NUMBER_PARSER.Parse(t[7], ref BoolMap, 1, 26));
            BoolMap = SavedBoolMap;
            BoolMap.Reset();
            Assert.IsFalse(NumberParser.NUMBER_PARSER.Parse(t[8], ref BoolMap, 1, 26));
            BoolMap = SavedBoolMap;
            BoolMap.Reset();
            Assert.IsFalse(NumberParser.NUMBER_PARSER.Parse(t[9], ref BoolMap, 1, 26));
        }

        [TestMethod]
        public void AnyParser_RecognizeTest()
        {
            StringPartArray space = new StringPartArray(4);
            StringPartArray t = " *,,*,26".Split(',', space);
            Assert.IsTrue(AnyParser.ANY_PARSER.Recognize(t[0]));
            Assert.IsFalse(AnyParser.ANY_PARSER.Recognize(t[1]));
            Assert.IsTrue(AnyParser.ANY_PARSER.Recognize(t[2]));
            Assert.IsFalse(AnyParser.ANY_PARSER.Recognize(t[3]));

        }

        [TestMethod]
        public void AnyParser_ParseRangeTest()
        {
            StringPartArray space = new StringPartArray(4);
            StringPartArray t = " *,,*,26".Split(',', space);
            Assert.IsFalse(AnyParser.ANY_PARSER.ParseRange(t[0], out int RangeStart,out int RangeEnd,1,6));
            Assert.IsFalse(AnyParser.ANY_PARSER.ParseRange(t[1], out RangeStart, out RangeEnd, 1, 6));
            Assert.IsTrue(AnyParser.ANY_PARSER.ParseRange(t[2], out RangeStart, out RangeEnd, 1, 6));
            Assert.AreEqual(1, RangeStart);
            Assert.AreEqual(6, RangeEnd);
            Assert.IsFalse(AnyParser.ANY_PARSER.ParseRange(t[3], out RangeStart, out RangeEnd, 1, 6));
        }

        [TestMethod]
        public void AnyParser_ParseTest()
        {
            StringPartArray space = new StringPartArray(4);
            StringPartArray t = " *,,*,26".Split(',', space);
            bool[] BoolMap = new bool[6];
            bool[] SaveMap = BoolMap;
            Assert.IsFalse(AnyParser.ANY_PARSER.Parse(t[0], ref BoolMap, 1, 6));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(AnyParser.ANY_PARSER.Parse(t[1], ref BoolMap, 1, 6));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(AnyParser.ANY_PARSER.Parse(t[2], ref BoolMap, 1, 6));
            Assert.IsNull(BoolMap);
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(AnyParser.ANY_PARSER.Parse(t[3], ref BoolMap, 1, 6));
        }

        [TestMethod]
        public void RangeParser_RecognizeTest()
        {
            StringPartArray space = new StringPartArray(11);
            StringPartArray t = "2-4,-6,20-,-,0-6,20-27,0-,-27,aaa-10,10-bbb,aaa".Split(',', space);
            Assert.IsTrue(RangeParser.RANGE_PARSER.Recognize(t[0]));
            Assert.IsTrue(RangeParser.RANGE_PARSER.Recognize(t[1]));
            Assert.IsTrue(RangeParser.RANGE_PARSER.Recognize(t[2]));
            Assert.IsTrue(RangeParser.RANGE_PARSER.Recognize(t[3]));
            Assert.IsTrue(RangeParser.RANGE_PARSER.Recognize(t[4]));
            Assert.IsTrue(RangeParser.RANGE_PARSER.Recognize(t[5]));
            Assert.IsTrue(RangeParser.RANGE_PARSER.Recognize(t[6]));
            Assert.IsTrue(RangeParser.RANGE_PARSER.Recognize(t[7]));
            Assert.IsTrue(RangeParser.RANGE_PARSER.Recognize(t[8]));
            Assert.IsTrue(RangeParser.RANGE_PARSER.Recognize(t[9]));
            Assert.IsFalse(RangeParser.RANGE_PARSER.Recognize(t[10]));
        }
        [TestMethod]
        public void RangeParser_ParseRangeTest()
        {
            StringPartArray space = new StringPartArray(11);
            StringPartArray t = "2-4,-6,20-,-,0-6,20-27,0-,-27,aaa-10,10-bbb,aaa".Split(',', space);
            int RangeStart, RangeEnd;
            Assert.IsTrue(RangeParser.RANGE_PARSER.ParseRange(t[0], out RangeStart, out RangeEnd, 1, 26));
            Assert.AreEqual(2, RangeStart);
            Assert.AreEqual(4, RangeEnd);
            Assert.IsTrue(RangeParser.RANGE_PARSER.ParseRange(t[1], out RangeStart, out RangeEnd, 1, 26));
            Assert.AreEqual(1, RangeStart);
            Assert.AreEqual(6, RangeEnd);
            Assert.IsTrue(RangeParser.RANGE_PARSER.ParseRange(t[2], out RangeStart, out RangeEnd, 1, 26));
            Assert.AreEqual(20, RangeStart);
            Assert.AreEqual(26, RangeEnd);
            Assert.IsTrue(RangeParser.RANGE_PARSER.ParseRange(t[3], out RangeStart, out RangeEnd, 1, 26));
            Assert.AreEqual(1, RangeStart);
            Assert.AreEqual(26, RangeEnd);
            Assert.IsFalse(RangeParser.RANGE_PARSER.ParseRange(t[4], out RangeStart, out RangeEnd, 1, 26));
            Assert.IsFalse(RangeParser.RANGE_PARSER.ParseRange(t[5], out RangeStart, out RangeEnd, 1, 26));
            Assert.IsFalse(RangeParser.RANGE_PARSER.ParseRange(t[6], out RangeStart, out RangeEnd, 1, 26));
            Assert.IsFalse(RangeParser.RANGE_PARSER.ParseRange(t[7], out RangeStart, out RangeEnd, 1, 26));
            Assert.IsFalse(RangeParser.RANGE_PARSER.ParseRange(t[8], out RangeStart, out RangeEnd, 1, 26));
            Assert.IsFalse(RangeParser.RANGE_PARSER.ParseRange(t[9], out RangeStart, out RangeEnd, 1, 26));
            Assert.IsFalse(RangeParser.RANGE_PARSER.ParseRange(t[10], out RangeStart, out RangeEnd, 1, 26));
        }

        [TestMethod]
        public void RangeParser_ParseTest()
        {
            StringPartArray space = new StringPartArray(11);
            StringPartArray t = "2-4,-6,20-,-,0-6,20-27,0-,-27,aaa-10,10-bbb,aaa".Split(',', space);
            bool[] BoolMap = new bool[26];
            bool[] SaveMap = BoolMap;
            Assert.IsTrue(RangeParser.RANGE_PARSER.Parse(t[0], ref BoolMap, 1, 26));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 2, 3, 4 }, BoolMap, 1, 26));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(RangeParser.RANGE_PARSER.Parse(t[1], ref BoolMap, 1, 26));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 1, 2, 3, 4, 5, 6 }, BoolMap, 1, 26));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(RangeParser.RANGE_PARSER.Parse(t[2], ref BoolMap, 1, 26));
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 20, 21, 22, 23, 24, 25, 26 }, BoolMap, 1, 26));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(RangeParser.RANGE_PARSER.Parse(t[3], ref BoolMap, 1, 26));
            Assert.IsTrue(TestUtils.CheckBoolMap(
                new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 }, BoolMap, 1, 26));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(RangeParser.RANGE_PARSER.Parse(t[4], ref BoolMap, 1, 26));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(RangeParser.RANGE_PARSER.Parse(t[5], ref BoolMap, 1, 26));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(RangeParser.RANGE_PARSER.Parse(t[6], ref BoolMap, 1, 26));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(RangeParser.RANGE_PARSER.Parse(t[7], ref BoolMap, 1, 26));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(RangeParser.RANGE_PARSER.Parse(t[8], ref BoolMap, 1, 26));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(RangeParser.RANGE_PARSER.Parse(t[9], ref BoolMap, 1, 26));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(RangeParser.RANGE_PARSER.Parse(t[10], ref BoolMap, 1, 26));
        }

        [TestMethod]
        public void StepwiseParser_RecognizeTest()
        {
            StringPartArray space = new StringPartArray(13);
            StringPartArray t = "2-10/4,-6/4,7-/3,*/2,*/1a, */2,6-8/1,5-13/3,0-6/2,1-6/13,/10,bbb/3,aaa".Split(',', space);
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Recognize(t[0])); //"2-10/4"
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Recognize(t[1])); //"-6/4"
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Recognize(t[2])); //"7-/3"
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Recognize(t[3])); //"*/2"
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Recognize(t[4])); //"*/1a"
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Recognize(t[5])); //" */2"
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Recognize(t[6])); //"6-8/1"
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Recognize(t[7])); //"5-13/3"
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Recognize(t[8])); //"0-6/2"
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Recognize(t[9])); //"1-6/13"
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Recognize(t[10])); //"/10"
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Recognize(t[11])); //"bbb/3"
            Assert.IsFalse(StepwiseParser.STEPWISE_PARSER.Recognize(t[12])); //"aaa"
        }

        [TestMethod]
        public void StepwiseParser_ParseTest()
        {
            StringPartArray space = new StringPartArray(13);
            StringPartArray t = "2-10/4,-6/4,7-/3,*/2,*/1a, */2,6-8/1,5-13/3,0-6/2,1-6/13,/10,bbb/3,aaa".Split(',', space);
            bool[] BoolMap = new bool[12];
            bool[] SaveMap = BoolMap;
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Parse(t[0], ref BoolMap, 1, 12)); //"2-10/4"
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {2, 6, 10}, BoolMap, 1, 12));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Parse(t[1], ref BoolMap, 1, 12)); //"-6/4"
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {1, 5}, BoolMap, 1, 12));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Parse(t[2], ref BoolMap, 1, 12)); //"7-/3"
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {7, 10}, BoolMap, 1, 12));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(StepwiseParser.STEPWISE_PARSER.Parse(t[3], ref BoolMap, 1, 12)); //"*/2"
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {1, 3, 5, 7, 9, 11}, BoolMap, 1, 12));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(StepwiseParser.STEPWISE_PARSER.Parse(t[4], ref BoolMap, 1, 12)); //"*/1a"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(StepwiseParser.STEPWISE_PARSER.Parse(t[5], ref BoolMap, 1, 12)); //" */2"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(StepwiseParser.STEPWISE_PARSER.Parse(t[6], ref BoolMap, 1, 12)); //"6-8/1"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(StepwiseParser.STEPWISE_PARSER.Parse(t[7], ref BoolMap, 1, 12)); //"5-13/3"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(StepwiseParser.STEPWISE_PARSER.Parse(t[8], ref BoolMap, 1, 12)); //"0-6/2"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(StepwiseParser.STEPWISE_PARSER.Parse(t[9], ref BoolMap, 1, 12)); //"1-6/13"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(StepwiseParser.STEPWISE_PARSER.Parse(t[10], ref BoolMap, 1, 12)); //"/10"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(StepwiseParser.STEPWISE_PARSER.Parse(t[11], ref BoolMap, 1, 12)); //"bbb/3"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(StepwiseParser.STEPWISE_PARSER.Parse(t[12], ref BoolMap, 1, 12)); //"aaa"
        }

        [TestMethod]
        public void ListParser_ParseTest()
        {
            ListParser parser = new ListParser(1, 12);
            const String test_cases = "5;3,7;1-3,5-8/2,10;*/2,2,3;*;1,*,3,4-6,9-/2;*,*/2;;1,2,;0,1;2,*/1;3,* ,4;3,0-2;3,*,13";
            const char case_delim = ';';
            StringPartArray space = new StringPartArray(test_cases.Count(c => c == case_delim)+1);
            bool[] BoolMap = new bool[12];
            bool[] SaveMap = BoolMap;
            StringPartArray t = test_cases.Split(case_delim, space);
            Assert.IsTrue(parser.Parse(t[0],ref BoolMap)); //"5"
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 5 }, BoolMap, 1, 12));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(parser.Parse(t[1], ref BoolMap)); //"3,7"
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 3,7 }, BoolMap, 1, 12));
            Assert.IsFalse(TestUtils.CheckBoolMap(new int[] { 5,7 }, BoolMap, 1, 12));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(parser.Parse(t[2], ref BoolMap)); //"1-3,5-8/2,10"
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] {1,2,3,5,7,10 }, BoolMap, 1, 12));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(parser.Parse(t[3], ref BoolMap));//"*/2,2,3"
            Assert.IsTrue(TestUtils.CheckBoolMap(new int[] { 1, 2, 3, 5, 7, 9, 11 }, BoolMap, 1, 12));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(parser.Parse(t[4], ref BoolMap));//"*"
            Assert.IsTrue(TestUtils.CheckBoolMap(null, BoolMap, 1, 12));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(parser.Parse(t[5], ref BoolMap));//"1,*,3,4-6,9-/2"
            Assert.IsTrue(TestUtils.CheckBoolMap(null, BoolMap, 1, 12));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsTrue(parser.Parse(t[6], ref BoolMap));//"*,*/2"
            Assert.IsTrue(TestUtils.CheckBoolMap(null, BoolMap, 1, 12));
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(parser.Parse(t[7], ref BoolMap)); //""
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(parser.Parse(t[8], ref BoolMap)); //"1,2,"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(parser.Parse(t[9], ref BoolMap)); //"0,1"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(parser.Parse(t[10], ref BoolMap)); //"2,*/1"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(parser.Parse(t[11], ref BoolMap)); //"3,* ,4"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(parser.Parse(t[12], ref BoolMap)); //"3,0-2"
            BoolMap = SaveMap;
            BoolMap.Reset();
            Assert.IsFalse(parser.Parse(t[13], ref BoolMap)); //"3,*,13"
            BoolMap = SaveMap;
            BoolMap.Reset();
        }


    }
}
