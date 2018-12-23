using System;
using System.Collections.Generic;
using System.IO;
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
                public (int x, int y, int z) Location { get; set; }
                public int Radius { get; set; }

                public Drone(int x, int y, int z, int radius)
                {
                    Location = (x, y, z);
                    Radius   = radius;
                }

                public int CalculateDistance(int x, int y, int z)
                {
                    return Math.Abs(Location.x - x) +
                           Math.Abs(Location.y - y) +
                           Math.Abs(Location.z - z);
                }

                public int CalculateDistance(Drone d)
                {
                    return Math.Abs(Location.x - d.Location.x) +
                           Math.Abs(Location.y - d.Location.y) +
                           Math.Abs(Location.z - d.Location.z);
                }

                public IEnumerable<(int x, int y, int z)> GetCoordinatesInRange()
                {
                    var list = new List<(int, int, int)>();
                    for (int x = Location.x - Radius; x < Location.x + Radius; x++)
                    for (int y = Location.y - Radius; x < Location.y + Radius; x++)
                    for (int z = Location.z - Radius; x < Location.z + Radius; x++)
                    {
                        if (CalculateDistance(x, y, z) <= Radius)
                        {
                            list.Add((x, y, z));
                        }
                    }

                    return list;
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

            public static void Day23_1(IEnumerable<string> input)
            {
                var drones = GetInput(input);

                var maxRadius = drones.Max(d => d.Radius);
                var strongestDrone = drones.First(d => d.Radius == drones.Max(dd => dd.Radius));

                int count = 0;
                foreach (var drone in drones)
                {
                    int distance = strongestDrone.CalculateDistance(drone);
                    if (distance <= maxRadius)
                    {
                        count++;
                    }
                }

                Console.WriteLine(count);
            }

            public static void Day23_2(IEnumerable<string> input)
            {
                var drones = GetInput(input);

                int minX = int.MaxValue;
                int minY = int.MaxValue;
                int minZ = int.MaxValue;

                int maxX = int.MinValue;
                int maxY = int.MinValue;
                int maxZ = int.MinValue;

                foreach (var drone in drones)
                {
                    minX = Math.Min(minX, drone.Location.x);
                    minY = Math.Min(minY, drone.Location.y);
                    minZ = Math.Min(minZ, drone.Location.z);

                    maxX = Math.Max(maxX, drone.Location.x);
                    maxY = Math.Max(maxY, drone.Location.y);
                    maxZ = Math.Max(maxZ, drone.Location.z);
                }

                int X = maxX - minX;
                int Y = maxY - minY;
                int Z = maxZ - minZ;

                var map = new int[X, Y, Z];
                int maxDronesInRange = 0;

                foreach (var drone in drones)
                {
                    foreach ((int x, int y, int z) in drone.GetCoordinatesInRange())
                    {
                        map[x, y, z]++;
                        maxDronesInRange = Math.Max(maxDronesInRange, map[x, y, z]);
                    }
                }

                Console.WriteLine(maxDronesInRange);
            }
        }
        private static void Main()
        {
            const string PATH  = @"C:\Users\Stefano\Documents\input.txt";
            var          input = File.ReadAllLines(PATH);

            Console.WriteLine(DateTime.Now.TimeOfDay);
            Day23.Day23_2(input);
            Console.WriteLine(DateTime.Now.TimeOfDay);
        }
    }
}
