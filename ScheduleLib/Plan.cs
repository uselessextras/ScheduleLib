using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleLib
{
    public class Plan
    {
        public List<Range> Enable { get; protected set; } = new List<Range>();
        public List<Range> Disable { get; protected set; } = new List<Range>();
        public Span Next(DateTimeOffset dt, int days)
        {
            var span = Span.Null;
            if (DateTimeOffset.MinValue == dt) return span;
            foreach(var re in Enable)
            {
                var next = Next(re, dt, days);
                if (next.IsEmpty) continue;
                if (span.IsEmpty) span = next;
                else span += next;
            }
            foreach(var rd in Disable)
            {
                var next = Next(rd, dt, days);
                if (next.IsEmpty) continue;
                span -= next;
                if (span.IsEmpty || span.End <= dt) return Next(next.End.AddTicks(1), days);
            }
            return span;
        }
        public Span Next(Range range, DateTimeOffset dt, int days)
        {
            var on = range.NextOn(dt, days);
            if (on < dt) return Span.Null;
            var off = range.NextOff(on.AddTicks(1), days);
            if (off < on) off = on.Date.AddDays(days);
            return new Span(on, off);
        }
    }
}
