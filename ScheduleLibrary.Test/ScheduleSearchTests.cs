using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test;

namespace ScheduleLibrary.Test
{
    [TestClass]
    public class ScheduleSearchTests
    {
        [TestMethod]
        public void ValidStartNearestNextPrev_Test()
        {
            Schedule t = new Schedule("17-30.08.2021 2 18:44:50.500");
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 50, 500), t.NearestEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 50, 500), t.NearestPrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
        }

        [TestMethod]
        public void ValidStartNextPrev_Test()
        {
            Schedule t = new Schedule("24.08.2021 2 18:44:50.499-501");
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 50, 501), t.NextEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 50, 499), t.PrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
        }

        [TestMethod]
        public void ValidStartNextPrevPartsMinStep_Test() {
            Schedule t = new Schedule("24.08.2021 18:44:49-51.500");
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 51, 500), t.NextEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 49, 500), t.PrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            t = new Schedule("24.08.2021 18:43-45:50.500");
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 45, 50, 500), t.NextEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 43, 50, 500), t.PrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            t = new Schedule("24.08.2021 17-19:44:50.500");
            Assert.AreEqual(new DateTime(2021, 8, 24, 19, 44, 50, 500), t.NextEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 17, 44, 50, 500), t.PrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            t = new Schedule("23-25.08.2021 18:44:50.500");
            Assert.AreEqual(new DateTime(2021, 8, 25, 18, 44, 50, 500), t.NextEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 8, 23, 18, 44, 50, 500), t.PrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            t = new Schedule("24.07-09.2021 18:44:50.500");
            Assert.AreEqual(new DateTime(2021, 9, 24, 18, 44, 50, 500), t.NextEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 7, 24, 18, 44, 50, 500), t.PrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            t = new Schedule("24.08.2020-2022 18:44:50.500");
            Assert.AreEqual(new DateTime(2022, 8, 24, 18, 44, 50, 500), t.NextEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2020, 8, 24, 18, 44, 50, 500), t.PrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
        }

        [TestMethod]
        public void ValidStartNextPrevPartsWrap_Test()
        {
            Schedule t = new Schedule("2,23-25,30.2,7-9,11.2020-2022 1,17-19,22:1,43-45,58:1,49-51,58.1,500,998");
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 51, 1), t.NextEvent(new DateTime(2021, 8, 24, 18, 44, 50, 998)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 49, 998), t.PrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 1)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 45, 1, 1), t.NextEvent(new DateTime(2021, 8, 24, 18, 44, 58, 998)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 43, 58, 998), t.PrevEvent(new DateTime(2021, 8, 24, 18, 44, 1, 1)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 19, 1, 1, 1), t.NextEvent(new DateTime(2021, 8, 24, 18, 58, 58, 998)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 17, 58, 58, 998), t.PrevEvent(new DateTime(2021, 8, 24, 18, 1, 1, 1)));
            Assert.AreEqual(new DateTime(2021, 8, 25, 1, 1, 1, 1), t.NextEvent(new DateTime(2021, 8, 24, 22, 58, 58, 998)));
            Assert.AreEqual(new DateTime(2021, 8, 23, 22, 58, 58, 998), t.PrevEvent(new DateTime(2021, 8, 24, 1, 1, 1, 1)));
            Assert.AreEqual(new DateTime(2021, 9, 2, 1, 1, 1, 1),      t.NextEvent(new DateTime(2021, 8, 30, 22, 58, 58, 998)));
            Assert.AreEqual(new DateTime(2021, 7, 30,22,58,58,998),    t.PrevEvent(new DateTime(2021, 8, 2,  1,  1,  1, 1)));
            Assert.AreEqual(new DateTime(2022, 2,  2, 1, 1, 1, 1),      t.NextEvent(new DateTime(2021, 11, 30, 22, 58, 58, 998)));
            Assert.AreEqual(new DateTime(2020, 11, 30,22,58,58,998),    t.PrevEvent(new DateTime(2021, 2, 2, 1, 1, 1, 1)));
            Assert.ThrowsException<NoMoreEventsException>(()=> t.NextEvent(new DateTime(2022, 11, 30, 22, 58, 58, 998)));
            Assert.ThrowsException<NoMoreEventsException>(() => t.PrevEvent(new DateTime(2020, 2, 2, 1, 1, 1, 1)));
        }

        [TestMethod]
        public void InvalidStartNearestNextPrev_Test()
        {
            Schedule t;
            t = new Schedule("2,23-25,30.2,7-9,11.2020-2022 1,17-19,22:1,43-45,58:1,49-51,58.1,998");
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 50, 998), t.NearestEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 50, 1), t.NearestPrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            t = new Schedule("2,23-25,30.2,7-9,11.2020-2022 1,17-19,22:1,43-45,58:1,49,51,58.1,500,998");
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 51, 1), t.NearestEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 44, 49, 998), t.NearestPrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            t = new Schedule("2,23-25,30.2,7-9,11.2020-2022 1,17-19,22:1,43,45,58:1,49-51,58.1,500,998");
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 45, 1, 1), t.NearestEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 18, 43, 58, 998), t.NearestPrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            t = new Schedule("2,23-25,30.2,7-9,11.2020-2022 1,17,19,22:1,43-45,58:1,49-51,58.1,500,998");
            Assert.AreEqual(new DateTime(2021, 8, 24, 19, 1, 1, 1), t.NearestEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 8, 24, 17, 58, 58, 998), t.NearestPrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            t = new Schedule("2,23,25,30.2,7-9,11.2020-2022 1,17-19,22:1,43-45,58:1,49-51,58.1,500,998");
            Assert.AreEqual(new DateTime(2021, 8, 25, 1, 1, 1, 1), t.NearestEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 8, 23, 22, 58, 58, 998), t.NearestPrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            t = new Schedule("2,23-25,30.2,7,9,11.2020-2022 1,17-19,22:1,43-45,58:1,49-51,58.1,500,998");
            Assert.AreEqual(new DateTime(2021, 9, 2, 1, 1, 1, 1), t.NearestEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2021, 7, 30, 22, 58, 58, 998), t.NearestPrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            t = new Schedule("2,23-25,30.2,7-9,11.2020,2022 1,17-19,22:1,43-45,58:1,49-51,58.1,500,998");
            Assert.AreEqual(new DateTime(2022, 2, 2, 1, 1, 1, 1), t.NearestEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
            Assert.AreEqual(new DateTime(2020, 11, 30, 22, 58, 58, 998), t.NearestPrevEvent(new DateTime(2021, 8, 24, 18, 44, 50, 500)));
        }

        [TestMethod]
        public void Feb29OnlyNearestOrNextPrev_Test()
        {
            Schedule t;
            t = new Schedule("29.2.* 0:0:0");
            Assert.AreEqual(new DateTime(2020, 2, 29, 0, 0, 0, 0), t.NearestEvent(new DateTime(2020, 2, 29, 0, 0, 0, 0)));
            Assert.AreEqual(new DateTime(2020, 2, 29, 0, 0, 0, 0), t.NearestPrevEvent(new DateTime(2020, 2, 29, 0, 0, 0, 0)));
            Assert.AreEqual(new DateTime(2024, 2, 29, 0, 0, 0, 0), t.NextEvent(new DateTime(2020, 2, 29, 0, 0, 0, 0)));
            Assert.AreEqual(new DateTime(2016, 2, 29, 0, 0, 0, 0), t.PrevEvent(new DateTime(2020, 2, 29, 0, 0, 0, 0)));
        }

        [TestMethod]
        public void Friday13OnlyNearestOrNextPrev_Test()
        {
            Schedule t;
            t = new Schedule("13.*.* 5 0:0:0");
            Assert.AreEqual(new DateTime(2021, 8, 13, 0, 0, 0, 0), t.NearestEvent(new DateTime(2021, 8, 13, 0, 0, 0, 0)));
            Assert.AreEqual(new DateTime(2021, 8, 13, 0, 0, 0, 0), t.NearestPrevEvent(new DateTime(2021, 8, 13, 0, 0, 0, 0)));
            Assert.AreEqual(new DateTime(2022, 5, 13, 0, 0, 0, 0), t.NextEvent(new DateTime(2021, 8, 13, 0, 0, 0, 0)));
            Assert.AreEqual(new DateTime(2020, 11, 13, 0, 0, 0, 0), t.PrevEvent(new DateTime(2021, 8, 13, 0, 0, 0, 0)));
        }


    }
}
