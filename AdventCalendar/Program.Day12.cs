using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day12
        {
            private const int CENTER = 1000;

            private static long CalculateResult(long generations, char[] plants, IReadOnlyDictionary<string, char> rules)
            {
                var set = new HashSet<string>();
                long k;
                int count = 0;

                for (k = 0; k < generations; k++)
                {
                    string Plants = new string(plants);
                    if (set.Contains(Plants.TrimStart('.').TrimEnd('.')))
                    {
                        count = Plants.Count(c => c == '#');
                        break;
                    }

                    set.Add(Plants.TrimStart('.').TrimEnd('.'));

                    for (int j = 2; j < plants.Length - 2; j++)
                    {
                        string tmp = Plants.Substring(j - 2, 5);
                        plants[j] = rules[tmp];
                    }
                }

                return plants.Select((c, j) => (c == '#')
                                                   ? j - CENTER
                                                   : 0)
                             .Sum() +
                       count * (generations - k);
            }

            private static long CalculateResultV2(long generations, char[] plants, IReadOnlyDictionary<string, char> rules)
            {
                long k,
                     sum    = 0,
                     diff   = 0,
                     diff_1 = -1,
                     diff_2 = -2,
                     diff_3 = -3;

                for (k = 0; k < generations; k++)
                {
                    string Plants = new string(plants);

                    if (diff == diff_1 &&
                        diff == diff_2 &&
                        diff == diff_3)
                    {
                        break;
                    }

                    for (int j = 2; j < plants.Length - 2; j++)
                    {
                        string tmp = Plants.Substring(j - 2, 5);
                        plants[j] = rules[tmp];
                    }

                    long sum_1 = sum;
                    sum = plants.Select((c, j) => (c == '#')
                                                      ? j - CENTER
                                                      : 0)
                                .Sum();

                    diff_3 = diff_2;
                    diff_2 = diff_1;
                    diff_1 = diff;
                    diff   = sum - sum_1;
                }

                return sum + diff * (generations - k);
            }

            private static long Day12_0(IReadOnlyList<string> input, long generations)
            {
                var regex1 = new Regex(@"[.#]");
                var regex2 = new Regex(@"([.#]*) => ([.#])");

                var matches = regex1.Matches(input[0]);
                var plants = new char[input[0].Length + CENTER * 2 - 15];

                var rules = new Dictionary<string, char>();

                int i;

                for (i = 0; i < CENTER; i++)
                {
                    plants[i] = '.';
                }

                foreach (Match match in matches)
                {
                    plants[i++] = match.Value[0];
                }

                for (; i < plants.Length; i++)
                {
                    plants[i] = '.';
                }

                for (int j = 2; j < input.Count; j++)
                {
                    var match = regex2.Match(input[j]);
                    rules.Add(match.Groups[1].Value,
                              match.Groups[2].Value[0]);
                }

                return CalculateResult(generations, plants, rules);
            }

            public static long Day12_1(IReadOnlyList<string> input)
            {
                return Day12_0(input, 20);
            }
        
            public static long Day12_2(IReadOnlyList<string> input)
            {
                return Day12_0(input, 50000000000);
            }
        }
    }
}
