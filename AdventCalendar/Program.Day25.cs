using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day25
        {
            private class Point
            {
                private (int x, int y, int z, int t) Location { get; }

                public Point(int x, int y, int z, int t)
                {
                    Location = (x, y, z, t);
                }

                public int Distance(Point point)
                {
                    return ManhattanDistance(Location, point.Location);
                }
            }

            private static List<Point> GetInput(IEnumerable<string> input)
            {
                var regex = new Regex(@"(-?\d+),(-?\d+),(-?\d+),(-?\d+)");
                var points = new List<Point>();

                foreach (string line in input)
                {
                    var match = regex.Match(line);

                    int x = int.Parse(match.Groups[1].Value);
                    int y = int.Parse(match.Groups[2].Value);
                    int z = int.Parse(match.Groups[3].Value);
                    int t = int.Parse(match.Groups[4].Value);

                    points.Add(new Point(x, y, z, t));
                }

                return points;
            }

            private static int ManhattanDistance((int x, int y, int z, int t) point1,
                                                 (int x, int y, int z, int t) point2)
            {
                return Math.Abs(point1.x - point2.x) +
                       Math.Abs(point1.y - point2.y) +
                       Math.Abs(point1.z - point2.z) +
                       Math.Abs(point1.t - point2.t);
            }
            public static void Day25_1(IEnumerable<string> input)
            {
                var points = GetInput(input);
                var Constellations = new List<List<Point>>();

                foreach (var point in points)
                {
                    var  constellations     = new List<List<Point>>(Constellations);
                    var  newConstellation   = new List<Point>();

                    foreach (var constellation in constellations)
                    {
                        foreach (var point1 in constellation)
                        {
                            if (point.Distance(point1) <= 3)
                            {
                                newConstellation.AddRange(constellation);
                                Constellations.Remove(constellation);
                                break;
                            }
                        }
                    }

                    newConstellation.Add(point);
                    Constellations.Add(newConstellation);
                }

                Console.WriteLine(Constellations.Count);
            }
        }
    }
}
