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
    }
}
