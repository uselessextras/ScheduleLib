using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using ScheduleLib;

namespace ScheduleLib_Tests
{
    [TestFixture]
    public class Range_Tests
    {
        DateTimeOffset DT(string s)
        {
            return DateTimeOffset.Parse(s);
        }
        Range RX(string b, string e)
        {
            return new Range { Begin = DT(b), End = DT(e), };
        }
        [Test]
        public void DateTimeOffsetProperties()
        {
            Assert.AreEqual(1, DateTimeOffset.MinValue.Year);
            Assert.AreEqual(TimeSpan.Zero, DateTimeOffset.MinValue.Offset);
            Assert.AreEqual(9999, DateTimeOffset.MaxValue.Year);
            Assert.AreEqual(TimeSpan.Zero, DateTimeOffset.MaxValue.Offset);
        }
        [Test]
        public void Simple()
        {
            Range r_09_11 = RX("1/1/2020 09:00:00", "3/1/2020 11:00:00");
            var nexton = r_09_11.NextOn(DT("1/1/2020 08:00:00"), 1);
            Assert.AreEqual(r_09_11.Begin.TimeOfDay, nexton.TimeOfDay);
            var nextoff = r_09_11.NextOff(nexton.AddMilliseconds(1), 2);
            Assert.AreEqual(r_09_11.End.TimeOfDay, nextoff.TimeOfDay);
        }
        [Test]
        public void MonFri_8_17()
        {
            var work = RX("1/1/2000 08:00:00", "12/31/3000 17:00:00");
            var start = work.NextOn(DT("9/28/2019 06:45:00"), 3);
            Assert.AreEqual(work.Begin.TimeOfDay, start.TimeOfDay);
            Assert.AreEqual(DT("9/28/2019 00:00:00").Date, start.Date);
            var finish = work.NextOff(start, 3);
            Assert.AreEqual(work.End.TimeOfDay, finish.TimeOfDay);
            Assert.AreEqual(DT("9/28/2019 00:00:00").Date, finish.Date);
            // apply Mon-Fri
            work.WeekDays = 0b0111110;
            start = work.NextOn(DT("9/28/2019 06:45:00"), 3);
            Assert.AreEqual(work.Begin.TimeOfDay, start.TimeOfDay);
            Assert.AreEqual(DT("9/30/2019 00:00:00").Date, start.Date);
            finish = work.NextOff(start, 1);
            Assert.AreEqual(work.End.TimeOfDay, finish.TimeOfDay);
            Assert.AreEqual(DT("9/30/2019 00:00:00").Date, finish.Date);
        }
    }
}
