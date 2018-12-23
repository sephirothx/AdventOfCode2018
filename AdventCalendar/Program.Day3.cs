using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day3
        {
            private const int MAX_DIM = 1000;

            public static int Day3_1(IEnumerable<string> input)
            {
                var matrix = new int[MAX_DIM, MAX_DIM];
                var regex  = new Regex(@".*@\s(\d+),(\d+):\s(\d+)x(\d+)");

                var claims = input.Select(s => regex.Match(s))
                                  .Select(match => (x:      int.Parse(match.Groups[1].Value),
                                                    y:      int.Parse(match.Groups[2].Value),
                                                    width:  int.Parse(match.Groups[3].Value),
                                                    height: int.Parse(match.Groups[4].Value)))
                                  .ToList();

                foreach ((int x, int y, int width, int height) in claims)
                    for (var j = x; j < (x + width); j++)
                    for (var k = y; k < (y + height); k++)
                        matrix[j, k]++;

                var sum = 0;
                for (var i = 0; i < MAX_DIM; i++)
                for (var j = 0; j < MAX_DIM; j++)
                    if (matrix[i, j] > 1)
                        sum++;

                return sum;
            }

            public static int Day3_2(IEnumerable<string> input)
            {
                var matrix = new int[MAX_DIM, MAX_DIM];
                var regex  = new Regex(@"#(\d+)\s@\s(\d+),(\d+):\s(\d+)x(\d+)");

                var claims = input.Select(s => regex.Match(s))
                                  .Select(match => (id:     int.Parse(match.Groups[1].Value),
                                                    x:      int.Parse(match.Groups[2].Value),
                                                    y:      int.Parse(match.Groups[3].Value),
                                                    width:  int.Parse(match.Groups[4].Value),
                                                    height: int.Parse(match.Groups[5].Value)))
                                  .ToList();

                foreach ((int _, int x, int y, int width, int height) in claims)
                    for (var j = x; j < (x + width); j++)
                    for (var k = y; k < (y + height); k++)
                        matrix[j, k]++;

                foreach ((int id, int x, int y, int width, int height) in claims)
                {
                    var goodOne = true;
                    for (var j = x; j < (x + width); j++)
                    for (var k = y; k < (y + height); k++)
                        if (matrix[j, k] > 1)
                            goodOne = false;

                    if (goodOne)
                        return id;
                }

                return 0;
            }
        }
    }
}