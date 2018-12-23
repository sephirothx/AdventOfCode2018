using System.Collections.Generic;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day14
        {
            private const int INPUT         = 846021;
            private const int FIRST_RECIPE  = 3;
            private const int SECOND_RECIPE = 7;

            public static string Day14_1()
            {
                var list            = new LinkedList<int>();
                var firstElf        = list.AddFirst(FIRST_RECIPE);
                var secondElf       = list.AddLast(SECOND_RECIPE);
                var numberOfRecipes = 2;

                while (true)
                {
                    int nextRecipes = firstElf.Value + secondElf.Value;
                    if (nextRecipes < 10)
                    {
                        list.AddLast(nextRecipes);
                        numberOfRecipes++;
                    }
                    else
                    {
                        list.AddLast(nextRecipes / 10);
                        list.AddLast(nextRecipes % 10);
                        numberOfRecipes += 2;
                    }

                    int firstElfValue = firstElf.Value + 1;
                    for (int i = 0; i < firstElfValue; i++)
                    {
                        firstElf = firstElf.Next ?? list.First;
                    }

                    int secondElfValue = secondElf.Value + 1;
                    for (int i = 0; i < secondElfValue; i++)
                    {
                        secondElf = secondElf.Next ?? list.First;
                    }

                    if (numberOfRecipes >= INPUT + 10)
                    {
                        string ret     = "";
                        var    pointer = list.Last;

                        for (int i = 0; i < 10 - 1; i++)
                        {
                            pointer = pointer.Previous ?? list.Last;
                        }

                        for (int i = 0; i < 10; i++)
                        {
                            ret     += pointer.Value.ToString();
                            pointer =  pointer.Next ?? list.First;
                        }

                        return ret;
                    }
                }
            }

            public static long Day14_2()
            {
                var list      = new LinkedList<int>();
                var firstElf  = list.AddFirst(FIRST_RECIPE);
                var secondElf = list.AddLast(SECOND_RECIPE);

                long numberOfRecipes = 2;
                while (true)
                {
                    int nextRecipes = firstElf.Value + secondElf.Value;
                    if (nextRecipes < 10)
                    {
                        list.AddLast(nextRecipes);
                        numberOfRecipes++;
                    }
                    else
                    {
                        list.AddLast(nextRecipes / 10);
                        list.AddLast(nextRecipes % 10);
                        numberOfRecipes += 2;
                    }

                    int firstElfValue = firstElf.Value + 1;
                    for (int i = 0; i < firstElfValue; i++)
                    {
                        firstElf = firstElf.Next ?? list.First;
                    }

                    int secondElfValue = secondElf.Value + 1;
                    for (int i = 0; i < secondElfValue; i++)
                    {
                        secondElf = secondElf.Next ?? list.First;
                    }

                    int ret     = 0;
                    var pointer = list.Last;

                    for (int i = 0; i < 9; i++)
                    {
                        pointer = pointer.Previous ?? list.Last;
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        ret     = pointer.Value + 10 * ret;
                        pointer = pointer.Next ?? list.First;
                    }

                    if (ret == INPUT)
                    {
                        return numberOfRecipes - 10;
                    }
                }
            }
        }
    }
}
