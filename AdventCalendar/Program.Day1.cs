using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day1
        {
            public static int Day1_1(IEnumerable<string> input)
            {
                return input.Select(int.Parse).Sum();
            }

            public static int Day1_2(IEnumerable<string> input)
            {
                int n   = 0;
                var set = new HashSet<int>();

                var collection = input.Select(int.Parse).ToArray();

                while (true)
                {
                    foreach (var i in collection)
                    {
                        n += i;

                        if (set.Contains(n))
                        {
                            return n;
                        }

                        set.Add(n);
                    }
                }
            }
        }
    }
}