using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day9
        {
            private class Circle
            {
                private LinkedListNode<int> Current { get; set; }
                private LinkedList<int> List { get; }

                public Circle()
                {
                    List    = new LinkedList<int>();
                    Current = List.AddFirst(0);
                }

                public void Add(int number, int cycle = 0)
                {
                    if (cycle == 0)
                    {
                        Current = List.AddFirst(number);
                    }
                    else
                    {
                        for (int i = 1; i < cycle; i++)
                        {
                            Current = Current.Next ?? List.First;
                        }

                        Current = List.AddAfter(Current, number);
                    }
                }

                public int Remove(int cycle)
                {
                    for (int i = 0; i < cycle; i++)
                    {
                        Current = Current.Previous ?? List.Last;
                    }

                    int ret = Current.Value;
                    var tmp = Current.Next;
                    List.Remove(Current);
                    Current = tmp;

                    return ret;
                }
            }

            private static long CalculateHighScore(int numberOfElves, int numberOfMarbles)
            {
                var elves   = new long[numberOfElves];
                var marbles = new Circle();

                for (int i = 1; i <= numberOfMarbles; i++)
                {
                    if ((i % 23) == 0)
                    {
                        elves[i % numberOfElves] += i + marbles.Remove(7);
                    }
                    else
                    {
                        marbles.Add(i, 2);
                    }
                }

                long highScore = elves.Max();

                return highScore;
            }

            public static long Day9_1()
            {
                var inputs = Console.ReadLine()?
                                    .Split(' ')
                                    .Select(int.Parse)
                                    .ToArray();

                int numberOfElves   = inputs?[0] ?? 0;
                int numberOfMarbles = inputs?[1] ?? 0;

                return CalculateHighScore(numberOfElves, numberOfMarbles);
            }

            public static long Day9_2()
            {
                var inputs = Console.ReadLine()?
                                    .Split(' ')
                                    .Select(int.Parse)
                                    .ToArray();

                int numberOfElves   = inputs?[0] ?? 0;
                int numberOfMarbles = (inputs?[1] ?? 0) * 100;

                return CalculateHighScore(numberOfElves, numberOfMarbles);
            }
        }
    }
}
