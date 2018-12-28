using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day4
        {
            private class Guard
            {
                public int TimeAsleep { get; set; }
                public int[] MinutesAsleep { get; private set; }
                public int ID { get; private set; }

                public Guard(int id)
                {
                    ID            = id;
                    TimeAsleep    = 0;
                    MinutesAsleep = new int[60];
                }
            }

            private static Dictionary<int, Guard> GetGuards(IEnumerable<string> input)
            {
                var strings = input.OrderBy(p => p).ToArray();
                var guards  = new Dictionary<int, Guard>();
                var guard   = new Guard(-1);
                var start   = 0;

                foreach (var s in strings)
                {
                    if (s.Contains("#"))
                    {
                        var id = int.Parse(Regex.Match(s.Substring(s.IndexOf("#", StringComparison.Ordinal)), @"\d+")
                                                .Value);
                        guard = guards.ContainsKey(id) ? guards[id] : guards[id] = new Guard(id);
                    }
                    else if (s.Contains("falls"))
                    {
                        start = int.Parse(s.Substring(s.IndexOf(":", StringComparison.Ordinal) + 1, 2));
                    }
                    else if (s.Contains("wakes"))
                    {
                        var end = int.Parse(s.Substring(s.IndexOf(":", StringComparison.Ordinal) + 1, 2));
                        guard.TimeAsleep += end - start;
                        for (var i = start; i < end; i++)
                        {
                            guard.MinutesAsleep[i]++;
                        }
                    }
                }

                return guards;
            }

            public static int Day4_1(IEnumerable<string> input)
            {
                var guards = GetGuards(input);

                var bestGuard  = guards.Values.Aggregate((agg, next) => next.TimeAsleep > agg.TimeAsleep ? next : agg);
                var bestMinute = bestGuard.MinutesAsleep.ToList().IndexOf(bestGuard.MinutesAsleep.Max());

                return bestGuard.ID * bestMinute;
            }

            public static int Day4_2(IEnumerable<string> input)
            {
                var guards = GetGuards(input);

                var bestGuard = guards.Values.Aggregate((agg, next) =>
                                                            next.MinutesAsleep.Max() > agg.MinutesAsleep.Max()
                                                                ? next
                                                                : agg);
                var bestMinute = bestGuard.MinutesAsleep.ToList().IndexOf(bestGuard.MinutesAsleep.Max());

                return bestGuard.ID * bestMinute;
            }
        }
    }
}