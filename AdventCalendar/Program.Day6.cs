using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day6
        {
            private class Coordinate
            {
                public int X { get; }
                public int Y { get; }
                public int? Area { get; set; }

                public Coordinate(int x, int y)
                {
                    X    = x;
                    Y    = y;
                    Area = 0;
                }

                public int GetDistance(int x, int y)
                {
                    return Math.Abs(X - x) + Math.Abs((Y - y));
                }
            }

            public static int Day6_1(IEnumerable<string> input)
            {
                var coordinates = input.Select(s => s.Split(',').Select(int.Parse).ToArray())
                                       .Select(pair => new Coordinate(pair[0], pair[1])).ToArray();

                int maxX = coordinates.Max(c => c.X);
                int maxY = coordinates.Max(c => c.Y);

                for (int i = 0; i <= maxX; i++)
                for (int j = 0; j <= maxY; j++)
                {
                    int        minDistance           = int.MaxValue;
                    Coordinate minDistanceCoordinate = null;

                    foreach (var coordinate in coordinates)
                    {
                        int distance = coordinate.GetDistance(i, j);
                        if (distance < minDistance)
                        {
                            minDistanceCoordinate = coordinate;
                            minDistance           = distance;
                        }

                        else if (distance == minDistance)
                        {
                            minDistanceCoordinate = null;
                        }
                    }

                    if (minDistanceCoordinate?.Area != null)
                    {
                        minDistanceCoordinate.Area = minDistanceCoordinate.Area + 1;

                        if (i == 0    ||
                            j == 0    ||
                            i == maxX ||
                            j == maxY)
                        {
                            minDistanceCoordinate.Area = null;
                        }
                    }
                }

                return coordinates.Max(c => c.Area ?? 0);
            }

            public static int Day6_2(IEnumerable<string> input)
            {
                const int MAX_ALLOWED_DISTANCE = 10000;

                var coordinates = input.Select(s => s.Split(',').Select(int.Parse).ToArray())
                                       .Select(pair => new Coordinate(pair[0], pair[1])).ToArray();

                int maxX = coordinates.Max(c => c.X) + 1;
                int maxY = coordinates.Max(c => c.Y) + 1;

                int safeArea = 0;

                for (int i = 0; i < maxX; i++)
                for (int j = 0; j < maxY; j++)
                {
                    int totalDistance = coordinates.Aggregate(0, (agg, c) => agg + c.GetDistance(i, j));

                    if (totalDistance >= MAX_ALLOWED_DISTANCE) continue;

                    safeArea++;
                }

                return safeArea;
            }
        }
    }
}