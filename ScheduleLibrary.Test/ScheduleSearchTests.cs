using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace ScheduleLibrary.Test
{
    [TestClass]
    public class ScheduleSearchTests
    {
        DateTime _testTime = new DateTime(2021, 8, 24, 18, 44, 50, 500);
        [TestMethod]
        public void ValidStartNearestNextPrev_Test()
        {
            Schedule t = new Schedule("17-30.08.2021 2 18:44:50.500");
            Assert.AreEqual(_testTime, t.NearestEvent(_testTime));
            Assert.AreEqual(_testTime, t.NearestPrevEvent(_testTime));
        }

        [TestMethod]
        public void ValidStartNextPrev_Test()
        {
            Schedule t = new Schedule("17-30.08.2021 2 18:44:50.499-501");
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 50, 501), t.NextEvent(_testTime));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 50, 499), t.PrevEvent(_testTime));
        }

        [TestMethod]
        public void ValidStartNextPrevTimeParts_Test() {
            Schedule t = new Schedule("17-30.08.2021 2 18:44:49-51.500");
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 51, 500), t.NextEvent(_testTime));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 49, 500), t.PrevEvent(_testTime));
            t = new Schedule("17-30.08.2021 2 18:43-45:50.500");
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 45, 50, 500), t.NextEvent(_testTime));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 43, 50, 500), t.PrevEvent(_testTime));
            t = new Schedule("17-30.08.2021 2 17-19:44:50.500");
            Assert.AreEqual(new DateTime(2021, 8, 24, 19, 44, 50, 500), t.NextEvent(_testTime));
            Assert.AreEqual(new DateTime(2021, 8, 24, 17, 44, 50, 500), t.PrevEvent(_testTime));
        }
    }
}
