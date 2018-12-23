using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day20
        {
            private static (int x, int y) Move(char c)
            {
                switch (c)
                {
                case 'N': return (0, -1);
                case 'S': return (0, 1);
                case 'E': return (1, 0);
                case 'W': return (-1, 0);
                default:  return (0, 0);
                }
            }

            private static Dictionary<(int, int), int> GetDistances(string input)
            {
                var distances = new Dictionary<(int, int), int>();
                var path      = new Stack<(int, int)>();

                int x = 0, y = 0, prevX = x, prevY = y;

                distances.Add((x, y), 0);

                foreach (char c in input)
                {
                    switch (c)
                    {
                    case '^':
                    case '$':
                        break;

                    case '(':
                        path.Push((x, y));
                        break;

                    case '|':
                        (x, y) = path.Peek();
                        break;

                    case ')':
                        (x, y) = path.Pop();
                        break;

                    default:
                        var delta = Move(c);
                        x += delta.x;
                        y += delta.y;
                        if (!distances.ContainsKey((x, y)))
                        {
                            distances.Add((x, y), int.MaxValue);
                        }

                        distances[(x, y)] = Math.Min(distances[(prevX, prevY)] + 1,
                                                     distances[(x, y)]);
                        break;
                    }

                    prevX = x;
                    prevY = y;
                }

                return distances;
            }

            public static void Day20_1_2(string input)
            {
                var distances = GetDistances(input);

                Console.WriteLine(distances.Max(p => p.Value));
                Console.WriteLine(distances.Count(p => p.Value >= 1000));
            }
        }
    }
}
