using System;
using System.Diagnostics;

// ReSharper disable MemberCanBePrivate.Global

namespace Agema.Common.Diagnostics
{
    /// <summary>
    ///     An class for storing extension methods for diagnostic classes (e.g. Stopwatch, TimeSpan, etc)
    /// </summary>
    public static class TimingExtensions
    {
        /// <summary>
        ///     Gets a string representation of elapsed time using an appropriate time unit of measurement for the elapsed
        ///     milliseconds
        /// </summary>
        /// <param name="stopwatch">The stopwatch.</param>
        /// <returns>String (e.g. 3.141 minutes)</returns>
        public static string ElapsedTime(this Stopwatch stopwatch)
        {
            return ElapsedTime(stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        ///     Gets a string representation of elapsed time using an appropriate time unit of measurement for the elapsed
        ///     milliseconds.
        ///     The thresholds are abritray--120000 and below are seconds, 3600000-120000=minutes, 3600000+=hours
        /// </summary>
        /// <param name="elapsedMilliseconds">The elapsed milliseconds.</param>
        /// <param name="elapsedTimeFormat">The elapsed time format (default is "0.###" 5.862).</param>
        /// <returns>String (e.g. 3.141 minutes)</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public static string ElapsedTime(long elapsedMilliseconds, string elapsedTimeFormat = "0.###")
        {
            float elapsedTime;

            string uom;

            if (elapsedMilliseconds < 120000)
            {
                // convert to seconds:
                elapsedTime = elapsedMilliseconds / 1000f;
                uom = "seconds";
            }
            else if ((elapsedMilliseconds >= 120000) & (elapsedMilliseconds < 3600000))
            {
                elapsedTime = elapsedMilliseconds / 60000f;
                uom = "minutes";
            }
            else if (elapsedMilliseconds >= 3600000)
            {
                elapsedTime = elapsedMilliseconds / 3600000f;
                uom = "hours";
            }
            else
            {
                throw new NotImplementedException();
            }

            var s = $"{elapsedTime.ToString(elapsedTimeFormat)} {uom}";

            return s;
        }

        /// <summary>
        ///     Gets a string for logging statements that combines the time and the milliseconds
        /// </summary>
        /// <param name="stopwatch">The stopwatch.</param>
        /// <returns>String (e.g. 20d 08h 12m 45s 033ms)</returns>
        public static string ElapsedTimeMethodLogString(this Stopwatch stopwatch)
        {
            var elapsedTimeString = stopwatch.ElapsedTime();

            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            var format = "Method Elapsed Time: {0}.,{1}ms";

            var s = string.Format(format,
                elapsedTimeString,
                elapsedMilliseconds);

            return s;
        }

        /// <summary>
        ///     Converts a TimeSpan to a Human Readable string: 20d 08h 12m 45s 033ms
        /// </summary>
        /// <param name="ts">TimeSpan</param>
        /// <returns>String (e.g. 20d 08h 12m 45s 033ms)</returns>
        public static string ToHumanReadableString(this TimeSpan ts)
        {
            var result = $"{ts.Days:D2}d:{ts.Hours:D2}h:{ts.Minutes:D2}m:{ts.Seconds:D2}s:{ts.Milliseconds:D3}ms";


            //.Split(':')
            //.SkipWhile(s => Regex.Match(s, @"00\w").Success) // skip zero-valued components
            //.ToArray();


            // var result = string.Join(" ", parts).Trim(); // combine the result


            return result;
        }

        /// <summary>
        ///     Converts a Stopwatch to a Human Readable string: 20d 08h 12m 45s 033ms
        /// </summary>
        /// <param name="stopwatch">The stopwatch.</param>
        /// <returns>String (e.g. 20d 08h 12m 45s 033ms)</returns>
        public static string ToHumanReadableString(this Stopwatch stopwatch)
        {
            return TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToHumanReadableString();
        }
    }
}