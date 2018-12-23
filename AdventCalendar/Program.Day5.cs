using System;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day5
        {
            public static int Day5_1(string input)
            {
                int i = 1;

                while (i < input.Length)
                {
                    if (Math.Abs(input[i] - input[i - 1]) == 32)
                    {
                        input = input.Remove(i - 1, 2);
                        i     = i > 1 ? i - 1 : 1;

                        continue;
                    }

                    i++;
                }

                return input.Length;
            }

            public static int Day5_2(string input)
            {
                const string UNITS     = "abcdefghijklmnopqrstuvwxyz";
                int          minLength = input.Length;

                foreach (char c in UNITS)
                {
                    string buffer = input.Replace(c.ToString(), "")
                                         .Replace(char.ToUpper(c).ToString(), "");
                    int remainingLength = Day5_1(buffer);
                    minLength = Math.Min(minLength, remainingLength);
                }

                return minLength;
            }
        }
    }
}