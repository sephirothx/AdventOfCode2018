using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day25
        {
            private class Point
            {
                private (int x0, int x1, int x2, int x3) Location { get; }

                public Point(int x0, int x1, int x2, int x3)
                {
                    Location = (x0, x1, x2, x3);
                }

                public int Distance(Point point)
                {
                    return ManhattanDistance(Location, point.Location);
                }
            }

            private static IEnumerable<Point> GetInput(IEnumerable<string> input)
            {
                var points = new List<Point>();

                foreach (string line in input)
                {
                    var x = line.Split(',').Select(int.Parse).ToArray();

                    points.Add(new Point(x[0], x[1], x[2], x[3]));
                }

                return points;
            }

            private static int ManhattanDistance((int x0, int x1, int x2, int x3) point1,
                                                 (int x0, int x1, int x2, int x3) point2)
            {
                return Math.Abs(point1.x0 - point2.x0) +
                       Math.Abs(point1.x1 - point2.x1) +
                       Math.Abs(point1.x2 - point2.x2) +
                       Math.Abs(point1.x3 - point2.x3);
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
