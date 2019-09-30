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
        Range r_09_11 = new Range(DateTimeOffset.Parse("1/1/2020 09:00:00"), DateTimeOffset.Parse("3/1/2020 11:00:00"));
        [Test]
        public void Simple()
        {
            var nexton = r_09_11.NextOn(DateTimeOffset.Parse("1/1/2020 08:00:00"), 1);
            Assert.AreEqual(r_09_11.Begin.TimeOfDay, nexton.TimeOfDay);
            var nextoff = r_09_11.NextOff(nexton.AddMilliseconds(1), 2);
            Assert.AreEqual(r_09_11.End.TimeOfDay, nextoff.TimeOfDay);
        }
        [Test]
        public void MonFri_8_17()
        {
            var work = new Range(DateTimeOffset.Parse("1/1/2000 08:00:00"), DateTimeOffset.Parse("12/31/3000 17:00:00"));
            var start = work.NextOn(DateTimeOffset.Parse("9/28/2019 06:45:00"), 3);
            Assert.AreEqual(work.Begin.TimeOfDay, start.TimeOfDay);
            Assert.AreEqual(DateTimeOffset.Parse("9/28/2019 00:00:00").Date, start.Date);
            var finish = work.NextOff(start, 3);
            Assert.AreEqual(work.End.TimeOfDay, finish.TimeOfDay);
            Assert.AreEqual(DateTimeOffset.Parse("9/28/2019 00:00:00").Date, finish.Date);
            // apply Mon-Fri
            work.WeekDays = 0b0111110;
            start = work.NextOn(DateTimeOffset.Parse("9/28/2019 06:45:00"), 3);
            Assert.AreEqual(work.Begin.TimeOfDay, start.TimeOfDay);
            Assert.AreEqual(DateTimeOffset.Parse("9/30/2019 00:00:00").Date, start.Date);
            finish = work.NextOff(start, 1);
            Assert.AreEqual(work.End.TimeOfDay, finish.TimeOfDay);
            Assert.AreEqual(DateTimeOffset.Parse("9/30/2019 00:00:00").Date, finish.Date);
        }
    }
}
