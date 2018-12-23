using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day18
        {
            private const int DIMENSION = 50;
            private const int MINUTES_1 = 10;
            private const long MINUTES_2 = 1000000000;

            private const int OPEN = 0;
            private const int TREE = 1;
            private const int LUMBER = 2;

            private static int[,] GetInput(IReadOnlyList<string> input)
            {
                var map = new int[DIMENSION, DIMENSION];

                for (int y = 0; y < DIMENSION; y++)
                for (int x = 0; x < DIMENSION; x++)
                {
                    char c = input[y][x];
                    switch (c)
                    {
                    case '.':
                        map[x, y] = OPEN;
                        break;

                    case '|':
                        map[x, y] = TREE;
                        break;

                    case '#':
                        map[x, y] = LUMBER;
                        break;
                    }
                }

                return map;
            }

            private static void Print(int[,] map)
            {
                for (int y = 0; y < DIMENSION; y++)
                {
                    for (int x = 0; x < DIMENSION; x++)
                    {
                        Console.Write(map[x, y] == TREE
                                          ? '|'
                                          : map[x, y] == LUMBER
                                              ? '#'
                                              : '.');
                    }

                    Console.WriteLine();
                }
            }

            private static List<int> GetNeighbors(int[,] map, int x, int y)
            {
                var output = new List<int>();

                if (x != 0 &&
                    y != 0)
                {
                    output.Add(map[x - 1, y - 1]);
                }

                if (x != 0)
                {
                    output.Add(map[x - 1, y]);
                }

                if (y != 0)
                {
                    output.Add(map[x, y - 1]);
                }

                if (x != 0 &&
                    y != DIMENSION - 1)
                {
                    output.Add(map[x - 1, y + 1]);
                }

                if (y != 0 &&
                    x < DIMENSION - 1)
                {
                    output.Add(map[x + 1, y - 1]);
                }

                if (x != DIMENSION - 1)
                {
                    output.Add(map[x + 1, y]);
                }

                if (y != DIMENSION - 1)
                {
                    output.Add(map[x, y + 1]);
                }

                if (x != DIMENSION - 1 &&
                    y != DIMENSION - 1)
                {
                    output.Add(map[x + 1, y + 1]);
                }

                return output;
            }

            public static int Day18_1(IReadOnlyList<string> input)
            {
                var map = GetInput(input);

                for (int i = 0; i < MINUTES_1; i++)
                {
                    var nextMap = new int[DIMENSION, DIMENSION];
                    Array.Copy(map, nextMap, map.Length);

                    for (int y = 0; y < DIMENSION; y++)
                    for (int x = 0; x < DIMENSION; x++)
                    {
                        var neighbors = GetNeighbors(map, x, y);
                        int trees     = neighbors.Count(c => c == TREE);
                        int lumber    = neighbors.Count(c => c == LUMBER);

                        switch (map[x, y])
                        {
                        case OPEN:
                            if (trees >= 3)
                            {
                                nextMap[x, y] = TREE;
                            }

                            break;

                        case TREE:
                            if (lumber >= 3)
                            {
                                nextMap[x, y] = LUMBER;
                            }

                            break;

                        case LUMBER:
                            if (lumber == 0 ||
                                trees  == 0)
                            {
                                nextMap[x, y] = OPEN;
                            }

                            break;
                        }
                    }

                    Array.Copy(nextMap, map, map.Length);
                }

                int lumberCount = 0;
                int treeCount   = 0;
                for (int y = 0; y < DIMENSION; y++)
                for (int x = 0; x < DIMENSION; x++)
                {
                    if (map[x, y] == TREE)
                    {
                        treeCount++;
                    }

                    if (map[x, y] == LUMBER)
                    {
                        lumberCount++;
                    }
                }

                return treeCount * lumberCount;
            }

            public static int Day18_2(IReadOnlyList<string> input)
            {
                var map      = GetInput(input);
                var set      = new HashSet<int>();
                var sequence = new List<int>();

                int index          = 0;
                int count          = 0;
                int sequenceLength = 0;

                for (long i = 0; i < MINUTES_2; i++)
                {
                    var nextMap = new int[DIMENSION, DIMENSION];
                    Array.Copy(map, nextMap, map.Length);

                    int lumberCount = 0;
                    int treeCount   = 0;

                    for (int y = 0; y < DIMENSION; y++)
                    for (int x = 0; x < DIMENSION; x++)
                    {
                        var neighbors = GetNeighbors(map, x, y);
                        int trees     = neighbors.Count(c => c == TREE);
                        int lumber    = neighbors.Count(c => c == LUMBER);

                        switch (map[x, y])
                        {
                        case OPEN:
                            if (trees >= 3)
                            {
                                nextMap[x, y] = TREE;
                                treeCount++;
                            }

                            break;

                        case TREE:
                            if (lumber >= 3)
                            {
                                nextMap[x, y] = LUMBER;
                                lumberCount++;
                                break;
                            }

                            treeCount++;
                            break;

                        case LUMBER:
                            if (lumber == 0 ||
                                trees  == 0)
                            {
                                nextMap[x, y] = OPEN;
                                break;
                            }

                            lumberCount++;
                            break;
                        }
                    }

                    int result = treeCount * lumberCount;

                    if (!set.Contains(result))
                    {
                        set.Add(result);
                        count = 0;
                    }
                    else if (count != 0)
                    {
                        if (sequence[index + count] == result)
                        {
                            count++;
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                    else
                    {
                        index = sequence.IndexOf(result);
                        count++;
                        sequenceLength = sequence.Count - index;
                    }

                    sequence.Add(result);

                    Array.Copy(nextMap, map, map.Length);

                    if (count == 5)
                    {
                        int Index = (int)((MINUTES_2 - index - 1) % sequenceLength) + index;
                        return sequence[Index];
                    }
                }

                return 0;
            }
        }
    }
}
