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

            public static string Day2_2(IEnumerable<string> input)
            {
                var strings = input as string[] ?? input.ToArray();
                for (var i = 0; i < strings.Length; i++)
                {
                    for (var j = i; j < strings.Length; j++)
                    {
                        int numDiff = 0,
                            index   = 0;
                        for (var k = 0; k < strings[i].Length; k++)
                        {
                            if (strings[i][k] == strings[j][k]) continue;

                            numDiff++;
                            index = k;
                        }

                        if (numDiff == 1)
                            return strings[i].Remove(index, 1);
                    }
                }

                return "";
            }
        }
    }
}