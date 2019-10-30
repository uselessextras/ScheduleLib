using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UselessExtras.ScheduleLib
{
    public class DateTimeSpan
    {
        public DateTimeOffset Begin { get; set; }
        public DateTimeOffset End { get; set; }
        public bool IsEmpty { get { return End <= Begin; } }
        public DateTimeSpan(DateTimeOffset begin, DateTimeOffset end)
        {
            Begin = begin;
            End = end;
        }
        public DateTimeSpan(DateTimeSpan span)
        {
            Begin = span.Begin;
            End = span.End;
        }
        /// <summary>
        /// Strict precedence, may or may not overlap
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns>True, if first segment starts before second starts, and ends before second ends. May or may not overlap.</returns>
        public static bool operator ==(DateTimeSpan s1, DateTimeSpan s2) { return s1.Begin == s2.Begin && s1.End == s2.End; }
        public static bool operator !=(DateTimeSpan s1, DateTimeSpan s2) { return s1.Begin != s2.Begin || s1.End != s2.End; ; }
        public static bool operator <(DateTimeSpan s1, DateTimeSpan s2) { return s1.Begin < s2.Begin && s1.End < s2.End; }
        public static bool operator >(DateTimeSpan s1, DateTimeSpan s2) { return s2 < s1; }
        /// <summary>
        /// Soft precedence, meaning they may overlap, and can be joined to one
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns>True, if first segment starts before second starts, and ends before second ends, and segments overlap.</returns>
        public static bool operator <=(DateTimeSpan s1, DateTimeSpan s2) { return s1.Begin <= s2.Begin && s2.Begin <= s1.End && s1.End <= s2.End; }
        public static bool operator >=(DateTimeSpan t1, DateTimeSpan t2) { return t2 <= t1; }
        /// <summary>
        /// Joining two segments (commutative)
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns>Joined (or first of the two, if no intersection) segment</returns>
        public static DateTimeSpan operator +(DateTimeSpan s1, DateTimeSpan s2)
        {
            if (s2.IsEmpty) return s1;
            if (s1 <= s2) return new DateTimeSpan(s1.Begin, s2.End);
            if (s2 <= s1) return new DateTimeSpan(s2.Begin, s1.End);
            if (s2 < s1) return s2;
            return s1;
        }
        /// <summary>
        /// Subtracting segments
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns>Remainder of the first segment after removing overlap with the second, or <see cref="DateTimeSpan.Null"/>Span.Null</returns>
        public static DateTimeSpan operator-(DateTimeSpan s1, DateTimeSpan s2)
        {
            if (s2.IsEmpty) return s1;
            if (s2.Begin < s1.Begin && s1.End < s2.End) return DateTimeSpan.Null;
            if (s1.Begin < s2.Begin && s2.Begin < s1.End) return new DateTimeSpan(s1.Begin, s2.Begin);
            if (s1 <= s2) return new DateTimeSpan(s1.Begin, s2.Begin);
            if (s2 <= s1) return new DateTimeSpan(s2.End, s1.End);
            return s1;
        }
        public override bool Equals(object obj)
        {
            return obj is DateTimeSpan && this == (DateTimeSpan)obj;
        }
        public override int GetHashCode()
        {
            return Begin.GetHashCode() + End.GetHashCode();
        }
        public static DateTimeSpan Null { get; } = new DateTimeSpan(DateTimeOffset.MinValue, DateTimeOffset.MinValue);
    }
}
