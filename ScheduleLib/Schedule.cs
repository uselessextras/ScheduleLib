using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UselessExtras.ScheduleLib
{
    public class Schedule
    {
        public List<DateTimeRange> EnabledRanges { get; protected set; } = new List<DateTimeRange>();
        public List<DateTimeRange> DisabledRanges { get; protected set; } = new List<DateTimeRange>();
        public DateTimeSpan Next(DateTimeOffset dt, int days)
        {
            var span = DateTimeSpan.Null;
            if (DateTimeOffset.MinValue == dt) return span;
            foreach(var re in EnabledRanges)
            {
                var next = Next(re, dt, days);
                if (next.IsEmpty) continue;
                if (span.IsEmpty) span = next;
                else span += next;
            }
            foreach(var rd in DisabledRanges)
            {
                var next = Next(rd, dt, days);
                if (next.IsEmpty) continue;
                span -= next;
                if (span.IsEmpty || span.End <= dt) return Next(next.End.AddTicks(1), days);
            }
            return span;
        }
        public static DateTimeSpan Next(DateTimeRange range, DateTimeOffset dt, int days)
        {
            var on = range.NextOn(dt, days);
            if (on < dt) return DateTimeSpan.Null;
            var off = range.NextOff(on.AddTicks(1), days);
            if (off < on) off = on.Date.AddDays(days);
            return new DateTimeSpan(on, off);
        }
    }
}
