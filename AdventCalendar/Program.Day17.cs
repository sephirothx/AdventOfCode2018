using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day17
        {
            private const int CLAY = -1;
            private const int SAND = 0;
            private const int MOVING_WATER = 1;
            private const int STILL_WATER = 2;

            private const int X = 569;
            private const int Y = 2043;
            private const int Y_MIN = 4;

            public static (int part1, int part2) Day17_1_2(IEnumerable<string> input)
            {
                var map     = GetInput(input);
                int count_1 = 0;
                int count_2 = 0;

                FlowWater(map, 500, 0);

                for (int y = Y_MIN + 1; y < Y; y++)
                for (int x = 0; x < X; x++)
                {
                    if (map[x, y] == MOVING_WATER)
                    {
                        count_1++;
                    }
                    else if (map[x, y] == STILL_WATER)
                    {
                        count_1++;
                        count_2++;
                    }
                }

                return (count_1, count_2);
            }

            private static void FlowWater(int[,] map, int x, int y)
            {
                bool stop = false;

                while (!stop)
                {
                    y++;

                    if (y == Y - 1)
                    {
                        map[x, y] = MOVING_WATER;
                        break;
                    }

                    switch (map[x, y + 1])
                    {
                    case MOVING_WATER:
                        map[x, y] = MOVING_WATER;
                        stop      = true;
                        break;

                    case SAND:
                        map[x, y] = MOVING_WATER;
                        break;

                    case STILL_WATER:
                    case CLAY:
                        int  i = x;
                        int  end;
                        bool toStill = true;

                        while (map[i + 1, y] != CLAY)
                        {
                            map[i, y] = MOVING_WATER;

                            if (map[i, y + 1] == CLAY ||
                                map[i, y + 1] == STILL_WATER)
                            {
                                i++;
                                if (i == X - 1)
                                {
                                    map[i, y] = MOVING_WATER;
                                    break;
                                }

                                continue;
                            }

                            FlowWater(map, i, y);
                            i--;
                            stop = true;
                            if (map[i, y] == MOVING_WATER)
                            {
                                toStill = false;
                            }

                            break;
                        }

                        end = i;

                        while (map[i, y] != CLAY)
                        {
                            if (map[i, y + 1] == CLAY ||
                                map[i, y + 1] == STILL_WATER)
                            {
                                map[i, y] = MOVING_WATER;
                                i--;
                                continue;
                            }

                            x = i;
                            y++;

                            stop    = false;
                            toStill = false;
                            break;
                        }

                        if (toStill)
                        {
                            for (int j = i + 1; j <= end; j++)
                            {
                                map[j, y] = STILL_WATER;
                            }
                        }

                        y -= 2;
                        break;
                    }
                }
            }

            private static int[,] GetInput(IEnumerable<string> input)
            {
                var regex = new Regex(@"(\w)=(\d+), \w=(\d+)\.\.(\d+)");
                var map   = new int[X, Y];

                foreach (string line in input)
                {
                    var match = regex.Match(line);

                    int first  = int.Parse(match.Groups[2].Value);
                    int second = int.Parse(match.Groups[3].Value);
                    int third  = int.Parse(match.Groups[4].Value);

                    if (match.Groups[1].Value == "x")
                    {
                        for (int y = second; y <= third; y++)
                        {
                            map[first, y] = CLAY;
                        }
                    }
                    else
                    {
                        for (int x = second; x <= third; x++)
                        {
                            map[x, first] = CLAY;
                        }
                    }
                }

                return map;
            }
        }
    }
}
