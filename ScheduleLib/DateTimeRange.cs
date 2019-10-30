using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UselessExtras.ScheduleLib
{
    public class DateTimeRange
    {
        public DateTimeOffset Begin { get; set; } = DateTimeOffset.MinValue;
        public DateTimeOffset End { get; set; } = DateTimeOffset.MinValue;
        public byte WeekDays { get; set; } = 0;
        public uint MonthDays { get; set; } = 0;
        public ushort Months { get; set; } = 0;
        public DateTimeRange() { }
        public DateTimeRange(DateTimeRange r)
        {
            Begin = r.Begin;
            End = r.End;
            WeekDays = r.WeekDays;
            MonthDays = r.MonthDays;
            Months = r.Months;
        }
        public DateTimeOffset NextOn(DateTimeOffset dt, int days)
        {
            var nd = dt.Date;
            for (var n = 0; n < days; n++, nd = nd.AddDays(1))
            {
                if (nd.Date < Begin.Date) continue;
                if (End.Date < nd.Date) continue;
                if (0 < WeekDays && (WeekDays & (1 << (int)nd.DayOfWeek)) == 0) continue;
                if (0 < MonthDays && (MonthDays & (1 << (int)nd.Day - 1)) == 0) continue;
                if (0 < Months && (Months & (1 << (int)nd.Month - 1)) == 0) continue;
                if (Begin.TimeOfDay == TimeSpan.Zero && End.TimeOfDay == TimeSpan.Zero) return nd;
                var t = 0 < n ? TimeSpan.Zero : dt.TimeOfDay;
                if (Begin.TimeOfDay <= End.TimeOfDay)
                {
                    // daytime range, includes starting at midnight
                    if (End.TimeOfDay <= t) continue;    // over for today
                    if (t < Begin.TimeOfDay) return nd.Add(Begin.TimeOfDay);
                }
                else if (TimeSpan.Zero < End.TimeOfDay)
                {
                    // nighttime range
                    if (t < End.TimeOfDay) return nd.AddDays(1);    //active yet, return midnight
                    if (t < Begin.TimeOfDay) return nd.Add(Begin.TimeOfDay);
                }
                else
                {
                    // ending at midnight
                    if (t < Begin.TimeOfDay) return nd.Add(Begin.TimeOfDay);
                }
            }
            return DateTimeOffset.MinValue;
        }
        public DateTimeOffset NextOff(DateTimeOffset dt, int days)
        {
            var nt = End;
            if (Begin.TimeOfDay != End.TimeOfDay)
            {
                // <complement range>.NextOn()
                nt = new DateTimeRange { Begin = Begin.Date.Add(End.TimeOfDay), End = End.Date.Add(Begin.TimeOfDay) }.NextOn(dt, days);
            }
            if (0 != WeekDays)
            {
                var nd = new DateTimeRange { WeekDays = (byte)~WeekDays }.NextOn(dt, days);
                if (nt == DateTimeOffset.MinValue || (DateTimeOffset.MinValue < nd && nd < nt)) nt = nd;
            }
            if (0 != MonthDays)
            {
                var nd = new DateTimeRange { MonthDays = (uint)~MonthDays }.NextOn(dt, days);
                if (nt == DateTimeOffset.MinValue || (DateTimeOffset.MinValue < nd && nd < nt)) nt = nd;
            }
            if (0 != Months)
            {
                var nd = new DateTimeRange { Months = (ushort)~Months }.NextOn(dt, days);
                if (nt == DateTimeOffset.MinValue || (DateTimeOffset.MinValue < nd && nd < nt)) nt = nd;
            }
            return nt;
        }
    }
}
