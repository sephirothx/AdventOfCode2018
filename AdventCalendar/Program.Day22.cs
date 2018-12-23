using System;
using System.Collections.Generic;
using Priority_Queue;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day22
        {
            private const int ROCKY = 0;
            private const int WET = 1;
            private const int NARROW = 2;

            private const int NOTHING = 0;
            private const int TORCH = 1;
            private const int CLIMBING_GEAR = 2;

            private const int DIMENSION_X = 1000;
            private const int DIMENSION_Y = 2000;

            private const int DEPTH = 7863;
            private const int TARGET_X = 14;
            private const int TARGET_Y = 760;
            private const int MOD = 20183;
            
            private class State : FastPriorityQueueNode
            {
                public int X { get; private set; }
                public int Y { get; private set; }
                public int Equip { get; private set; }

                public State(int x, int y, int equip)
                {
                    X = x;
                    Y = y;
                    Equip = equip;
                }
            }

            private static int GetErosionLevel(int index)
            {
                return (index + DEPTH) % MOD;
            }

            private static int GetGeologicIndex(int x, int y, int[,] map)
            {
                if ((x == 0 &&
                     y == 0) ||
                    (x == TARGET_X &&
                     y == TARGET_Y))
                {
                    return 0;
                }

                if (x == 0)
                {
                    return (y * 48271) % MOD;
                }

                if (y == 0)
                {
                    return (x * 16807) % MOD;
                }

                return (map[x, y - 1] * map[x - 1, y]) % MOD;
            }

            private static int GetRegionType(int erosionLevel)
            {
                return erosionLevel % 3;
            }

            private static bool CorrectEquipForRegionType(int equip, int region)
            {
                switch (region)
                {
                case ROCKY:
                    return equip == CLIMBING_GEAR ||
                           equip == TORCH;
                case WET:
                    return equip == CLIMBING_GEAR ||
                           equip == NOTHING;
                case NARROW:
                    return equip == NOTHING ||
                           equip == TORCH;
                default:
                    return false;
                }
            }

            private static (int x, int y) GetDirectionVector(int direction)
            {
                switch (direction)
                {
                case 0:  return (1, 0);
                case 1:  return (0, 1);
                case 2:  return (-1, 0);
                case 3:  return (0, -1);
                default: return (0, 0);
                }
            }
            
            private static int FindShortestPathTime(int[,] erosionLevelMap, int x, int y, int equip, int time)
            {
                var queue = new FastPriorityQueue<State>(100000);
                var set   = new HashSet<(int, int, int)>();

                queue.Enqueue(new State(x, y, equip), time);

                while (true)
                {
                    var state = queue.Dequeue();

                    time  = (int)state.Priority;
                    x     = state.X;
                    y     = state.Y;
                    equip = state.Equip;

                    if (x     == TARGET_X &&
                        y     == TARGET_Y &&
                        equip == TORCH)
                    {
                        return time;
                    }

                    if (set.Contains((x, y, equip)))
                    {
                        continue;
                    }

                    set.Add((x, y, equip));

                    for (int newEquip = 0; newEquip < 3; newEquip++)
                    {
                        if (CorrectEquipForRegionType(newEquip, GetRegionType(erosionLevelMap[x, y]))) 
                        {
                            queue.Enqueue(new State(x, y, newEquip), (time + 7));
                        }
                    }

                    for (int direction = 0; direction < 4; direction++)
                    {
                        int dx, dy;
                        (dx, dy) = GetDirectionVector(direction);

                        if (x + dx >= DIMENSION_X ||
                            x + dx < 0            ||
                            y + dy >= DIMENSION_Y ||
                            y + dy < 0)
                        {
                            continue;
                        }

                        if (CorrectEquipForRegionType(equip, GetRegionType(erosionLevelMap[x + dx, y + dy])))
                        {
                            queue.Enqueue(new State(x + dx, y + dy, equip), (time + 1));
                        }
                    }
                }
            }

            private static void Print(int[,] erosionLevel)
            {
                for (int y = 0; y <= TARGET_Y; y++)
                {
                    for (int x = 0; x <= TARGET_X; x++)
                    {
                        Console.Write(erosionLevel[x, y] % 3 == ROCKY
                                          ? "."
                                          : erosionLevel[x, y] % 3 == WET
                                              ? "="
                                              : "|");
                    }

                    Console.WriteLine();
                }
            }

            public static void Day22_1()
            {
                var geologicIndexMap = new int[TARGET_X + 1, TARGET_Y + 1];
                var erosionLevelMap  = new int[TARGET_X + 1, TARGET_Y + 1];

                int solution = 0;

                for (int y = 0; y <= TARGET_Y; y++)
                for (int x = 0; x <= TARGET_X; x++)
                {
                    geologicIndexMap[x, y] = GetGeologicIndex(x, y, erosionLevelMap);
                    erosionLevelMap[x, y] = GetErosionLevel(geologicIndexMap[x, y]);

                    solution += GetRegionType(erosionLevelMap[x, y]);
                }
                
                Console.WriteLine(solution);
            }

            public static void Day22_2()
            {
                var geologicIndexMap = new int[DIMENSION_X, DIMENSION_Y];
                var erosionLevelMap  = new int[DIMENSION_X, DIMENSION_Y];

                for (int y = 0; y < DIMENSION_Y; y++)
                for (int x = 0; x < DIMENSION_X; x++)
                {
                    geologicIndexMap[x, y] = GetGeologicIndex(x, y, erosionLevelMap);
                    erosionLevelMap[x, y]  = GetErosionLevel(geologicIndexMap[x, y]);
                }

                int time = FindShortestPathTime(erosionLevelMap, 0, 0, TORCH, 0);
                Console.WriteLine(time);
            }
        }
    }
}
