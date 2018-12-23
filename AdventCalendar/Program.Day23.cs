using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day23
        {
            private class Drone
            {
                private (int x, int y, int z) Location { get; set; }
                public int Radius { get; private set; }

                public Drone(int x, int y, int z, int radius)
                {
                    Location = (x, y, z);
                    Radius   = radius;
                }

                private int CalculateDistance((int x, int y, int z) point)
                {
                    return ManhattanDistance(Location, point);
                }

                private IEnumerable<(int x, int y, int z)> GetVertices()
                {
                    var list = new List<(int x, int y, int z)>
                               {
                                   (Location.x, Location.y, Location.z + Radius),
                                   (Location.x, Location.y, Location.z - Radius),
                                   (Location.x, Location.y + Radius, Location.z),
                                   (Location.x, Location.y - Radius, Location.z),
                                   (Location.x + Radius, Location.y, Location.z),
                                   (Location.x - Radius, Location.y, Location.z)
                               };

                    return list;
                }

                public IEnumerable<(int x, int y, int z)> GetPointsOfInterest(int distance)
                {
                    var list = new List<(int x, int y, int z)>();
                    foreach (var point in GetVertices())
                    {
                        list.AddRange(GetNeighbors(point, distance).Where(neighbor => CalculateDistance(neighbor) == Radius));
                    }
                    return list;
                }

                public bool IsInRange((int x, int y, int z) point)
                {
                    return ManhattanDistance(point, Location) <= Radius;
                }

                public bool IsInRange(Drone d)
                {
                    return ManhattanDistance(Location, d.Location) <= Radius;
                }
            }

            private static List<Drone> GetInput(IEnumerable<string> input)
            {
                var regex = new Regex(@"pos=<(-?\d+),(-?\d+),(-?\d+)>, r=(\d+)");
                var drones = (from line in input
                              select regex.Match(line) into match
                              let x = int.Parse(match.Groups[1].Value)
                              let y = int.Parse(match.Groups[2].Value)
                              let z = int.Parse(match.Groups[3].Value)
                              let r = int.Parse(match.Groups[4].Value)
                              select new Drone(x, y, z, r)).ToList();
                return drones;
            }

            private static IEnumerable<(int, int, int)> GetNeighbors((int x, int y, int z) point, int distance)
            {
                var list = new List<(int, int, int)>();

                for (int x = point.x - distance; x <= point.x + distance; x++)
                for (int y = point.y - distance; y <= point.y + distance; y++)
                {
                    list.Add((x, y, point.z));
                }

                for (int x = point.x - distance; x <= point.x + distance; x++)
                for (int z = point.z - distance; z <= point.z + distance; z++)
                {
                    list.Add((x, point.y, z));
                }

                for (int y = point.y - distance; y <= point.y + distance; y++)
                for (int z = point.z - distance; z <= point.z + distance; z++)
                {
                    list.Add((point.x, y, z));
                }

                return list;
            }

            private static int ManhattanDistance((int x, int y, int z) point1, (int x, int y, int z) point2)
            {
                return Math.Abs(point1.x - point2.x) +
                       Math.Abs(point1.y - point2.y) +
                       Math.Abs(point1.z - point2.z);
            }

            public static void Day23_1(IEnumerable<string> input)
            {
                var drones         = GetInput(input);
                var strongestDrone = drones.First(d => d.Radius == drones.Max(dd => dd.Radius));
                int count          = 0;

                foreach (var drone in drones)
                {
                    if (strongestDrone.IsInRange(drone))
                    {
                        count++;
                    }
                }

                Console.WriteLine(count);
            }

            public static void Day23_2(IEnumerable<string> input)
            {
                var drones = GetInput(input);
                var points = new Dictionary<(int,int,int), int>();

                int distanceFromZero    = int.MaxValue;
                int maxIntersection     = int.MinValue;

                (int x, int y, int z) targetPoint         = (0, 0, 0);
                (int x, int y, int z) previousTargetPoint = (0, 0, 0);

                int i = 0;
                while (true)
                {
                    foreach (var drone in drones)
                    foreach (var point in drone.GetPointsOfInterest(i))
                    {
                        if (points.ContainsKey(point)) continue;

                        points.Add(point, 0);
                        foreach (var drone1 in drones)
                        {
                            if (drone1.IsInRange(point))
                            {
                                points[point]++;
                            }
                        }

                        if (points[point] > maxIntersection)
                        {
                            maxIntersection = points[point];
                            int distance = ManhattanDistance((0, 0, 0), point);

                            targetPoint      = point;
                            distanceFromZero = distance;
                        }
                        else if (points[point] == maxIntersection)
                        {
                            distanceFromZero = Math.Min(distanceFromZero, ManhattanDistance((0, 0, 0), point));
                        }
                    }

                    if (targetPoint == previousTargetPoint)
                    {
                        Console.WriteLine($"{targetPoint} = {distanceFromZero}, {maxIntersection}");
                        return;
                    }

                    previousTargetPoint = targetPoint;
                    i++;
                }
            }
        }
    }
}
