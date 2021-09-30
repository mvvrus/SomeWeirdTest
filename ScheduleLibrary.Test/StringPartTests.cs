using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace ScheduleLibrary.Test
{
    [TestClass]
    public class StringPartTests
    {
        [TestMethod]
        public void EmptyStringPart_Properties()
        {
            StringPart t = new StringPart(null);
            Assert.AreEqual(0, t.Length);
            Assert.AreEqual(null, t.BaseString);
            Assert.AreEqual(0, t.Start);
            Assert.AreEqual(0, t.End);
            Assert.ThrowsException<IndexOutOfRangeException>(()=>t[0]);
        }

        [TestMethod]
        public void EmptyStringPart_IndexOf()
        {
            StringPart t = new StringPart(null);
            Assert.AreEqual(-1, t.IndexOf('a'));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.IndexOf('a', 1));
        }

        [TestMethod]
        public void EmptyStringPart_Clear()
        {
            StringPart t = new StringPart(null);
            t.Clear();
            Assert.AreEqual(null, t.BaseString);
            Assert.AreEqual(0, t.Start);
            Assert.AreEqual(0, t.End);
        }
        [TestMethod]
        public void EmptyStringPart_SubPart()
        {
            StringPart t = new StringPart(null);
            StringPart work = new StringPart("xyzzy");
            StringPart result;
            result = t.SubPart(0, 0, work);
            Assert.AreEqual(null, result.BaseString);
            Assert.AreEqual(0, result.Start);
            Assert.AreEqual(0, result.End);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.SubPart(-1, 0, work));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.SubPart(-1, -1, work));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.SubPart(0, 1, work));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.SubPart(1, 2, work));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.SubPart(1, 0, work));
        }

        [TestMethod]
        public void EmptyStringPart_ToString()
        {
            StringPart t = new StringPart(null);
            Assert.AreEqual(String.Empty, t.ToString());
        }

        [TestMethod]
        public void EmptyStringPart_Truncate()
        {
            StringPart t = new StringPart(null);
            Assert.IsTrue(t.Truncate(0));
            Assert.IsFalse(t.Truncate(-1));
            Assert.IsFalse(t.Truncate(1));
        }

        const string STR = "xyzzy";
        [TestMethod]
        public void NonEmptyStringPart_Properties()
        {
            StringPart t = new StringPart(STR);
            Assert.AreEqual(5, t.Length);
            Assert.AreSame(STR, t.BaseString);
            Assert.AreEqual(0, t.Start);
            Assert.AreEqual(5, t.End);
            Assert.AreEqual('x', t[0]);
            Assert.AreEqual('y', t[1]);
            Assert.AreEqual('y', t[4]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => t[5]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => t[-1]);
            t._start++;
            t._end--;
            Assert.AreEqual(3, t.Length);
            Assert.AreEqual(1, t.Start);
            Assert.AreEqual(4, t.End);
            Assert.AreEqual('y', t[0]);
            Assert.AreEqual('z', t[1]);
            Assert.AreEqual('z', t[2]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => t[3]);
            Assert.ThrowsException<IndexOutOfRangeException>(() => t[-1]);
        }

        [TestMethod]
        public void NonEmptyStringPart_IndexOf()
        {
            StringPart t = new StringPart(STR);
            Assert.AreEqual(-1, t.IndexOf('a'));
            Assert.AreEqual(-1, t.IndexOf('a',1));
            Assert.AreEqual(2, t.IndexOf('z'));
            Assert.AreEqual(3, t.IndexOf('z',3));
            Assert.AreEqual(-1, t.IndexOf('z', 5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.IndexOf('a', 6));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.IndexOf('a', -1));
            StringPart work = new StringPart(null);
            StringPart t2;
            int start = 1, len = 3;
            t2 = t.SubPart(start, start+len, work);
            Assert.AreEqual(1, t2.IndexOf('z'));
            Assert.AreEqual(2, t2.IndexOf('z', 2));
            Assert.AreEqual(-1, t2.IndexOf('z', len));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t2.IndexOf('z', len+1));
        }

        [TestMethod]
        public void NonEmptyStringPart_Clear()
        {
            StringPart t = new StringPart(STR);
            t.Clear();
            Assert.AreEqual(null, t.BaseString);
            Assert.AreEqual(0, t.Start);
            Assert.AreEqual(0, t.End);
        }

        [TestMethod]
        public void NonEmptyStringPart_SubPart()
        {
            StringPart t = new StringPart(STR);
            StringPart work = new StringPart(null);
            StringPart result;
            result = t.SubPart(1, 4, work);
            Assert.AreSame(STR, result.BaseString);
            Assert.AreEqual(1, result.Start);
            Assert.AreEqual(4, result.End);
            Assert.AreEqual(3, result.Length);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.SubPart(-1, 4, work));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.SubPart(1, -1, work));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.SubPart(0, 6, work));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.SubPart(5, 6, work));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.SubPart(1, 0, work));
        }

        [TestMethod]
        public void NonEmptyStringPart_ToString()
        {
            StringPart t = new StringPart(STR);
            Assert.AreEqual(STR, t.ToString());
            StringPart work = new StringPart(null);
            StringPart t2;
            int start = 1, len = 3;
            t2 = t.SubPart(start, start + len, work);
            Assert.AreEqual(STR.Substring(start,len), t2.ToString());
        }
        /*
        [TestMethod]
        public void StrinPartArrayTest()
        {
            StringPartArray t;
            t = new StringPartArray(0);
            Assert.IsFalse(t.Add(STR, 1, 2));

            t = new StringPartArray(2);
            Assert.AreEqual(0, t.Length);
            Assert.AreEqual(2, t.Capacity);
            Assert.ThrowsException<IndexOutOfRangeException>(() => t[0]);
            Assert.IsTrue(t.Add(STR, 0, STR.Length));
            Assert.AreEqual(1, t.Length);
            Assert.AreEqual(STR.Length, t[0].Length);
            Assert.AreEqual(0, t[0].Start);
            Assert.AreEqual(STR.Length, t[0].End);
            Assert.AreSame(STR, t[0].BaseString);
            Assert.AreEqual(STR, t[0].ToString());
            Assert.IsTrue(t.Add(STR, 1, STR.Length-1));
            Assert.AreEqual(2, t.Length);
            Assert.AreEqual(STR.Length-2, t[1].Length);
            Assert.AreEqual(1, t[1].Start);
            Assert.AreEqual(STR.Length-1, t[1].End);
            Assert.AreSame(STR, t[1].BaseString);
            Assert.AreEqual(STR.Substring(1, STR.Length - 2), t[1].ToString());
            Assert.IsFalse(t.Add(STR, 1, 2));
            t.SetLength(1);
            Assert.ThrowsException<IndexOutOfRangeException>(() => t[1]);
        }

        [TestMethod]

        public void StringPart_SplitTest()
        {
            StringPartArray space= new StringPartArray(3);
            StringPartArray t;
            t=(new StringPart(null)).Split(',',space);
            Assert.IsNotNull(t);
            Assert.AreEqual(1, t.Length);
            Assert.AreEqual(0, t[0].Start);
            Assert.AreEqual(0, t[0].End);
            Assert.IsNull(t[0].BaseString);
            t = (new StringPart("PART_ONE")).Split(',', space);
            Assert.IsNotNull(t);
            Assert.AreEqual(1, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            t = (new StringPart("PART_ONE,PART_TWO")).Split(',', space);
            Assert.IsNotNull(t);
            Assert.AreEqual(2, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.AreEqual("PART_TWO", t[1].ToString());
            t = (new StringPart("PART_ONE,PART_TWO,PART_THREE")).Split(',', space);
            Assert.IsNotNull(t);
            Assert.AreEqual(3, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.AreEqual("PART_TWO", t[1].ToString());
            Assert.AreEqual("PART_THREE", t[2].ToString());
            t = (new StringPart("PART_ONE,PART_TWO,PART_THREE,PART_FOUR")).Split(',', space);
            Assert.IsNull(t);
            StringPartArray space2 = new StringPartArray(2);
            t = (new StringPart("PART_ONE,PART_TWO|PART_THREE,PART_FOUR")).Split(',', space);
            Assert.IsNotNull(t);
            Assert.AreEqual(3, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.AreEqual("PART_TWO|PART_THREE", t[1].ToString());
            Assert.AreEqual("PART_FOUR", t[2].ToString());
            StringPartArray t2 = t;
            t = t2[1].Split('|', space2);
            Assert.IsNotNull(t);
            Assert.AreEqual(2, t.Length);
            Assert.AreEqual("PART_TWO", t[0].ToString());
            Assert.AreEqual("PART_THREE", t[1].ToString());
            t = t2[0].Split('|', space2);
            Assert.IsNotNull(t);
            Assert.AreEqual(1, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            t = "PART_ONE,PART_TWO".Split(',', space);
            Assert.IsNotNull(t);
            Assert.AreEqual(2, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.AreEqual("PART_TWO", t[1].ToString());
            t = (new StringPart(",PART_TWO,PART_THREE")).Split(',', space);
            Assert.IsNotNull(t);
            Assert.AreEqual(3, t.Length);
            Assert.AreEqual("", t[0].ToString());
            Assert.AreEqual("PART_TWO", t[1].ToString());
            Assert.AreEqual("PART_THREE", t[2].ToString());
            t = (new StringPart("PART_ONE,,PART_THREE")).Split(',', space);
            Assert.IsNotNull(t);
            Assert.AreEqual(3, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.AreEqual("", t[1].ToString());
            Assert.AreEqual("PART_THREE", t[2].ToString());
            t = (new StringPart("PART_ONE,PART_TWO,")).Split(',', space);
            Assert.IsNotNull(t);
            Assert.AreEqual(3, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.AreEqual("PART_TWO", t[1].ToString());
            Assert.AreEqual("", t[2].ToString());
            t = (new StringPart(",")).Split(',', space);
            Assert.IsNotNull(t);
            Assert.AreEqual(2, t.Length);
            Assert.AreEqual("", t[0].ToString());
            Assert.AreEqual("", t[1].ToString());

        }
        */

    }
}
