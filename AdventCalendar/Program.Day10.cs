using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day10
        {
            private class Point
            {
                public (int X, int Y) Position { get; private set; }
                private (int velX, int velY) Velocity { get; }

                public Point(int x, int y, int velx, int vely)
                {
                    Position = (x, y);
                    Velocity = (velx, vely);
                }

                public void Move()
                {
                    Position = (Position.X + Velocity.velX,
                                Position.Y + Velocity.velY);
                }
            }

            public static void Day10_1(IEnumerable<string> inputs)
            {
                var regex = new Regex(@".*<\s*(-?\d+),\s*(-?\d+)>.*<\s*(-?\d+),\s*(-?\d+)>");
                var points = inputs.Select(input =>
                                           {
                                               var match = regex.Match(input);
                                               return new Point(int.Parse(match.Groups[1].Value),
                                                                int.Parse(match.Groups[2].Value),
                                                                int.Parse(match.Groups[3].Value),
                                                                int.Parse(match.Groups[4].Value));
                                           }).ToArray();
                
                while (true)
                {
                    int maxX = int.MinValue;
                    int maxY = int.MinValue;

                    int minX = int.MaxValue;
                    int minY = int.MaxValue;

                    foreach (var point in points)
                    {
                        point.Move();

                        maxX = Math.Max(maxX, point.Position.X);
                        maxY = Math.Max(maxY, point.Position.Y);

                        minX = Math.Min(minX, point.Position.X);
                        minY = Math.Min(minY, point.Position.Y);
                    }

                    if ((maxY - minY) == 9)
                    {
                        for (int i = 0; i < ((maxY - minY) + 1); i++)
                        {
                            for (int j = 0; j < ((maxX - minX) + 1); j++)
                            {
                                bool ispoint = points.Any(point => (point.Position.X - minX) == j &&
                                                                   (point.Position.Y - minY) == i);

                                Console.Write(ispoint ? '*' : ' ');
                            }

                            Console.WriteLine();
                        }

                        return;
                    }
                }
            }

            public static int Day10_2(IEnumerable<string> inputs)
            {
                var regex  = new Regex(@".*<\s*(-?\d+),\s*(-?\d+)>.*<\s*(-?\d+),\s*(-?\d+)>");
                var points = inputs.Select(input =>
                                           {
                                               var match = regex.Match(input);
                                               return new Point(int.Parse(match.Groups[1].Value),
                                                                int.Parse(match.Groups[2].Value),
                                                                int.Parse(match.Groups[3].Value),
                                                                int.Parse(match.Groups[4].Value));
                                           }).ToArray();
                int seconds = 0;

                while (true)
                {
                    seconds++;

                    int maxY = int.MinValue;
                    int minY = int.MaxValue;

                    foreach (var point in points)
                    {
                        point.Move();

                        maxY = Math.Max(maxY, point.Position.Y);
                        minY = Math.Min(minY, point.Position.Y);
                    }

                    if ((maxY - minY) == 9)
                    {
                        return seconds;
                    }
                }
            }
        }
    }
}
