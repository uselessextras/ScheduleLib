using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using ScheduleLib;

namespace ScheduleLib_Tests
{
    [TestFixture]
    public class DateTimeSpan_Tests
    {
        [Test]
        public void Null()
        {
            Assert.IsTrue(DateTimeSpan.Null.IsEmpty);
        }
        readonly DateTimeSpan s_09_10 = new DateTimeSpan(DateTime.Parse("1/1/2020 09:00:00"), DateTime.Parse("1/1/2020 10:00:00"));
        readonly DateTimeSpan s_09_11 = new DateTimeSpan(DateTime.Parse("1/1/2020 09:00:00"), DateTime.Parse("1/1/2020 11:00:00"));
        readonly DateTimeSpan s_09_13 = new DateTimeSpan(DateTime.Parse("1/1/2020 09:00:00"), DateTime.Parse("1/1/2020 13:00:00"));
        readonly DateTimeSpan s_10_11 = new DateTimeSpan(DateTime.Parse("1/1/2020 10:00:00"), DateTime.Parse("1/1/2020 11:00:00"));
        readonly DateTimeSpan s_11_13 = new DateTimeSpan(DateTime.Parse("1/1/2020 11:00:00"), DateTime.Parse("1/1/2020 13:00:00"));
        readonly DateTimeSpan s_10_12 = new DateTimeSpan(DateTime.Parse("1/1/2020 10:00:00"), DateTime.Parse("1/1/2020 12:00:00"));
        readonly DateTimeSpan s_10_13 = new DateTimeSpan(DateTime.Parse("1/1/2020 10:00:00"), DateTime.Parse("1/1/2020 13:00:00"));
        readonly DateTimeSpan s_12_13 = new DateTimeSpan(DateTime.Parse("1/1/2020 12:00:00"), DateTime.Parse("1/1/2020 13:00:00"));
        [Test]
        public void Compare()
        {
            Assert.IsTrue(s_09_10 < s_11_13);
            Assert.IsFalse(s_09_10 <= s_11_13); // don't intersect
            Assert.IsTrue(s_09_11 < s_11_13);
            Assert.IsTrue(s_09_11 <= s_11_13);
            Assert.IsTrue(s_09_11 < s_10_12);
            Assert.IsTrue(s_09_11 <= s_10_12);
            var s = new DateTimeSpan(s_09_10);
            Assert.IsTrue(s.Equals(s_09_10));
            Assert.IsTrue(s == s_09_10);
            Assert.IsTrue(s != s_09_11);
            Assert.IsFalse(s == s_09_11);
        }
        [Test]
        public void Join()
        {
            Assert.IsTrue(s_09_10 + s_09_11 == s_09_11);
            Assert.IsTrue(s_09_10 + s_10_11 == s_09_11);
            Assert.IsTrue(s_09_10 + s_11_13 == s_09_10);
            Assert.IsTrue(s_11_13 + s_10_12 == s_10_13);
         }
        [Test]
        public void Subtract()
        {
            Assert.IsTrue(s_11_13 - s_10_12 == s_12_13);
            Assert.IsTrue(s_09_10 - s_10_12 == s_09_10);
            Assert.IsTrue(s_09_10 - s_11_13 == s_09_10);
            Assert.IsTrue(s_11_13 - s_09_10 == s_11_13);
            Assert.IsTrue(s_09_13 - s_10_11 == s_09_10);
            Assert.IsTrue(s_10_11 - s_09_13 == DateTimeSpan.Null);
        }
    }
}