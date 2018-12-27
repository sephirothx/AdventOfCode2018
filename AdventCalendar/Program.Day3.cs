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

            private static List<(int, int, int, int, int)> GetClaims(IEnumerable<string> input)
            {
                var regex = new Regex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)");

                var claims = input.Select(s => regex.Match(s))
                                  .Select(match => (int.Parse(match.Groups[1].Value),
                                                    int.Parse(match.Groups[2].Value),
                                                    int.Parse(match.Groups[3].Value),
                                                    int.Parse(match.Groups[4].Value),
                                                    int.Parse(match.Groups[5].Value)))
                                  .ToList();
                return claims;
            }

            private static int[,] GetClaimsMatrix(IEnumerable<(int, int, int, int, int)> claims)
            {
                var matrix = new int[MAX_DIM, MAX_DIM];

                foreach ((int _, int x, int y, int width, int height) in claims)
                {
                    for (var j = x; j < (x + width); j++)
                    for (var k = y; k < (y + height); k++)
                    {
                        matrix[j, k]++;
                    }
                }

                return matrix;
            }

            public static int Day3_1(IEnumerable<string> input)
            {
                var claims = GetClaims(input);
                var matrix = GetClaimsMatrix(claims);

                var sum = 0;

                for (var i = 0; i < MAX_DIM; i++)
                for (var j = 0; j < MAX_DIM; j++)
                {
                    if (matrix[i, j] > 1)
                        sum++;
                }

                return sum;
            }

            public static int Day3_2(IEnumerable<string> input)
            {
                var claims = GetClaims(input);
                var matrix = GetClaimsMatrix(claims);

                foreach ((int id, int x, int y, int width, int height) in claims)
                {
                    var goodOne = true;

                    for (var j = x; j < (x + width); j++)
                    for (var k = y; k < (y + height); k++)
                    {
                        if (matrix[j, k] > 1)
                            goodOne = false;
                    }

                    if (goodOne)
                        return id;
                }

                return 0;
            }
        }
    }
}