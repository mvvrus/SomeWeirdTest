using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace ScheduleLibrary.Test
{
    [TestClass]
    public class StringPartTests
    {
        const string STR = "xyzzy";

        [TestMethod]
        public void EmptyStringPart_IndexOf()
        {
            ReadOnlyMemory<char> t = ReadOnlyMemory<char>.Empty;
            Assert.AreEqual(-1, t.IndexOf('a'));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.IndexOf('a', 1));
        }

        [TestMethod]
        public void NonEmptyStringPart_IndexOf()
        {
            ReadOnlyMemory<char> t = STR.AsMemory();
            Assert.AreEqual(-1, t.IndexOf('a'));
            Assert.AreEqual(-1, t.IndexOf('a', 1));
            Assert.AreEqual(2, t.IndexOf('z'));
            Assert.AreEqual(3, t.IndexOf('z', 3));
            Assert.AreEqual(-1, t.IndexOf('z', 5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.IndexOf('a', 6));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t.IndexOf('a', -1));
            ReadOnlyMemory<char> t2;
            int start = 1, len = 3;
            t2 = t.Slice(start, len);
            Assert.AreEqual(1, t2.IndexOf('z'));
            Assert.AreEqual(2, t2.IndexOf('z', 2));
            Assert.AreEqual(-1, t2.IndexOf('z', len));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => t2.IndexOf('z', len + 1));
        }

        [TestMethod]
        public void StrinPartArrayTest()
        {
            StringPartArray t;
            t = new StringPartArray(0);
            Assert.IsFalse(t.Add(STR.AsMemory().Slice(1,1)));

            t = new StringPartArray(2);
            Assert.AreEqual(0, t.Length);
            Assert.AreEqual(2, t.Capacity);
            bool raised = false;
            ReadOnlyMemory<char> dummy;
            try {dummy = t[0];} catch (IndexOutOfRangeException) { raised = true;}
            Assert.IsTrue(raised);
            Assert.IsTrue(t.Add(STR.AsMemory()));
            Assert.AreEqual(1, t.Length);
            Assert.AreEqual(STR.Length, t[0].Length);
            Assert.AreEqual(STR, t[0].ToString());
            Assert.IsTrue(t.Add(STR.AsMemory().Slice(1, STR.Length-2)));
            Assert.AreEqual(2, t.Length);
            Assert.AreEqual(STR.Length-2, t[1].Length);
            Assert.AreEqual(STR.Substring(1, STR.Length - 2), t[1].ToString());
            Assert.IsFalse(t.Add(STR.AsMemory().Slice( 1, 1)));
            t.SetLength(1);
            raised = false;
            try { dummy = t[1]; } catch (IndexOutOfRangeException) { raised = true; }
            Assert.IsTrue(raised);
        }

        [TestMethod]

        public void StringPart_SplitTest()
        {
            StringPartArray t= new StringPartArray(3);
            Assert.IsTrue(ReadOnlyMemory<char>.Empty.Split(',', ref t));
            Assert.AreEqual(1, t.Length);
            Assert.AreEqual(0, t[0].Length);
            Assert.IsTrue("PART_ONE".AsMemory().Split(',', ref t));
            Assert.AreEqual(1, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.IsTrue("PART_ONE,PART_TWO".AsMemory().Split(',', ref t));
            Assert.AreEqual(2, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.AreEqual("PART_TWO", t[1].ToString());
            Assert.IsTrue("PART_ONE,PART_TWO,PART_THREE".AsMemory().Split(',', ref t));
            Assert.AreEqual(3, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.AreEqual("PART_TWO", t[1].ToString());
            Assert.AreEqual("PART_THREE", t[2].ToString());
            Assert.IsFalse("PART_ONE,PART_TWO,PART_THREE,PART_FOUR".AsMemory().Split(',', ref t));
            StringPartArray space2 = new StringPartArray(2);
            Assert.IsTrue("PART_ONE,PART_TWO|PART_THREE,PART_FOUR".AsMemory().Split(',', ref t));
            Assert.AreEqual(3, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.AreEqual("PART_TWO|PART_THREE", t[1].ToString());
            Assert.AreEqual("PART_FOUR", t[2].ToString());
            ReadOnlyMemory<char> t0 = t[0].Slice(0, t[0].Length);
            ReadOnlyMemory<char> t1 = t[1].Slice(0, t[1].Length);
            Assert.IsTrue(t1.Split('|', ref t));
            Assert.AreEqual(2, t.Length);
            Assert.AreEqual("PART_TWO", t[0].ToString());
            Assert.AreEqual("PART_THREE", t[1].ToString());
            Assert.IsTrue(t0.Split('|', ref t));
            Assert.AreEqual(1, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.IsTrue("PART_ONE,PART_TWO".Split(',', ref t));
            Assert.AreEqual(2, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.AreEqual("PART_TWO", t[1].ToString());
            Assert.IsTrue(",PART_TWO,PART_THREE".AsMemory().Split(',', ref t));
            Assert.AreEqual(3, t.Length);
            Assert.AreEqual("", t[0].ToString());
            Assert.AreEqual("PART_TWO", t[1].ToString());
            Assert.AreEqual("PART_THREE", t[2].ToString());
            Assert.IsTrue("PART_ONE,,PART_THREE".AsMemory().Split(',', ref t));
            Assert.AreEqual(3, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.AreEqual("", t[1].ToString());
            Assert.AreEqual("PART_THREE", t[2].ToString());
            Assert.IsTrue("PART_ONE,PART_TWO,".AsMemory().Split(',', ref t));
            Assert.AreEqual(3, t.Length);
            Assert.AreEqual("PART_ONE", t[0].ToString());
            Assert.AreEqual("PART_TWO", t[1].ToString());
            Assert.AreEqual("", t[2].ToString());
            Assert.IsTrue(",".AsMemory().Split(',', ref t));
            Assert.AreEqual(2, t.Length);
            Assert.AreEqual("", t[0].ToString());
            Assert.AreEqual("", t[1].ToString());
        }
    }
}
