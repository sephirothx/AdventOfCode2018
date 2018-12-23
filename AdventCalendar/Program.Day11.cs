using System;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day11
        {
            private const int SERIAL = 1955;
            private const int DIMENSION = 300;

            public static (int x, int y) Day11_1()
            {
                var matrix = PopulateMatrix(SERIAL);

                (int x, int y, int level) cell = (0, 0, int.MinValue);
                for (int i = 0; i < DIMENSION - 2; i++)
                for (int j = 0; j < DIMENSION - 2; j++)
                {
                    int powerLevel = 0;

                    for (int k = 0; k < 3; k++)
                    for (int l = 0; l < 3; l++)
                    {
                        powerLevel += matrix[i + k, j + l];
                    }

                    cell = cell.level < powerLevel
                               ? (i, j, powerLevel)
                               : cell;
                }

                return (cell.x, cell.y);
            }

            public static (int x, int y, int dimension) Day11_2()
            {
                var matrix = PopulateMatrix(SERIAL);

                (int x, int y, int level, int dimension) cell = (0, 0, int.MinValue, 0);
                for (int i = 0; i < DIMENSION - 2; i++)
                for (int j = 0; j < DIMENSION - 2; j++)
                {
                    int powerLevel = 0;

                    for (int q = 1; q < Math.Min(DIMENSION - i, DIMENSION - j); q++)
                    {
                        for (int k = 0; k < q; k++)
                        {
                            powerLevel += matrix[i + q - 1, j     + k] +
                                          matrix[i     + k, j + q - 1];
                        }

                        powerLevel -= matrix[i + q - 1, j + q - 1];

                        cell = cell.level < powerLevel
                                   ? (i, j, powerLevel, q)
                                   : cell;
                    }
                }

                return (cell.x, cell.y, cell.dimension);
            }

            private static int[,] PopulateMatrix(int serial)
            {
                var matrix = new int[DIMENSION, DIMENSION];

                for (int i = 0; i < DIMENSION; i++)
                for (int j = 0; j < DIMENSION; j++)
                {
                    int rack  = 10 + i;
                    int level = rack * j;

                    level += serial;
                    level *= rack;

                    matrix[i, j] = (level / 100) % 10 - 5;
                }

                return matrix;
            }
        }
    }
}
