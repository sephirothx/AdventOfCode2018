using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day2
        {
            public static int Day2_1(IEnumerable<string> input)
            {
                int num2 = 0,
                    num3 = 0;

                foreach (var s in input)
                {
                    bool is2 = false,
                         is3 = false;
                    foreach (var c in s)
                    {
                        if (s.Count(p => p == c) == 2)
                        {
                            is2 = true;
                        }

                        if (s.Count(p => p == c) == 3)
                        {
                            is3 = true;
                        }
                    }

                    if (is2)
                        num2++;
                    if (is3)
                        num3++;
                }

                Console.WriteLine(num2 + " " + num3);
                return num2 * num3;
            }

            public static string Day2_2(IReadOnlyList<string> input)
            {
                for (var i = 0; i < input.Count; i++)
                {
                    for (var j = i; j < input.Count; j++)
                    {
                        int numDiff = 0,
                            index   = 0;
                        for (var k = 0; k < input[i].Length; k++)
                        {
                            if (input[i][k] == input[j][k]) continue;

                            numDiff++;
                            index = k;
                        }

                        if (numDiff == 1)
                        {
                            return input[i].Remove(index, 1);
                        }
                    }
                }

                return "";
            }
        }
    }
}