using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day19
        {
            private const int REGISTERS = 6;

            public static Func<int[], int[], int[]>[] GetOperators()
            {
                var operators = new Func<int[], int[], int[]>[]
                                {
                                    (a, b) => // addr
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] + a[b[2]];

                                        return output;
                                    },

                                    (a, b) => // addi
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] + b[2];

                                        return output;
                                    },

                                    (a, b) => // mulr
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] * a[b[2]];

                                        return output;
                                    },

                                    (a, b) => // muli
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] * b[2];

                                        return output;
                                    },

                                    (a, b) => // banr
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] & a[b[2]];

                                        return output;
                                    },

                                    (a, b) => // bani
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] & b[2];

                                        return output;
                                    },

                                    (a, b) => // borr
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] | a[b[2]];

                                        return output;
                                    },

                                    (a, b) => // bori
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] | b[2];

                                        return output;
                                    },

                                    (a, b) => // setr
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]];

                                        return output;
                                    },

                                    (a, b) => // seti
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = b[1];

                                        return output;
                                    },

                                    (a, b) => // gtir
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = b[1] > a[b[2]] ? 1 : 0;

                                        return output;
                                    },

                                    (a, b) => // gtri
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] > b[2] ? 1 : 0;

                                        return output;
                                    },

                                    (a, b) => // gtrr
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] > a[b[2]] ? 1 : 0;

                                        return output;
                                    },

                                    (a, b) => // eqir
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = b[1] == a[b[2]] ? 1 : 0;

                                        return output;
                                    },

                                    (a, b) => // eqri
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] == b[2] ? 1 : 0;

                                        return output;
                                    },

                                    (a, b) => // eqrr
                                    {
                                        var output = new int[REGISTERS];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] == a[b[2]] ? 1 : 0;

                                        return output;
                                    }
                                };
                return operators;
            }

            private static int GetOpcode(string op)
            {
                switch (op)
                {
                case "addr": return 0;
                case "addi": return 1;
                case "mulr": return 2;
                case "muli": return 3;
                case "banr": return 4;
                case "bani": return 5;
                case "borr": return 6;
                case "bori": return 7;
                case "setr": return 8;
                case "seti": return 9;
                case "gtir": return 10;
                case "gtri": return 11;
                case "gtrr": return 12;
                case "eqir": return 13;
                case "eqri": return 14;
                case "eqrr": return 15;
                default:     return -1;
                }
            }

            public static int GetIpRegister(IReadOnlyList<string> input)
            {
                var regex = new Regex(@"\d+");
                var match = regex.Match(input[0]);

                return int.Parse(match.Value);
            }

            public static int[][] GetInstructions(IReadOnlyList<string> input)
            {
                var regex        = new Regex(@"(\w+) (\d+) (\d+) (\d+)");
                var instructions = new int[input.Count - 1][];

                for (int i = 1; i < input.Count; i++)
                {
                    var match = regex.Match(input[i]);

                    int opcode = GetOpcode(match.Groups[1].Value);
                    int first  = int.Parse(match.Groups[2].Value);
                    int second = int.Parse(match.Groups[3].Value);
                    int third  = int.Parse(match.Groups[4].Value);

                    instructions[i - 1] = new[]
                                          {
                                              opcode,
                                              first,
                                              second,
                                              third
                                          };
                }

                return instructions;
            }

            private static IEnumerable<int> GetFactors(int number)
            {
                var factors = new List<int>();

                for (int i = 1; i <= number; i++)
                {
                    if (number % i == 0)
                    {
                        factors.Add(i);
                    }
                }

                return factors;
            }

            public static int Day19_1(IReadOnlyList<string> input)
            {
                var registers    = new int[REGISTERS];
                var ipRegister   = GetIpRegister(input);
                var instructions = GetInstructions(input);
                var operators    = GetOperators();

                while (registers[ipRegister] < instructions.Length)
                {
                    var instruction = instructions[registers[ipRegister]];
                    registers = operators[instruction[0]](registers, instruction);

                    registers[ipRegister]++;
                }

                return registers[0];
            }

            public static int Day19_2(IReadOnlyList<string> input)
            {
                var registers    = new int[REGISTERS];
                var ipRegister   = GetIpRegister(input);
                var instructions = GetInstructions(input);
                var operators    = GetOperators();

                registers[0] = 1;

                while (registers[ipRegister] < instructions.Length)
                {
                    var instruction = instructions[registers[ipRegister]];
                    registers = operators[instruction[0]](registers, instruction);

                    registers[ipRegister]++;

                    if (registers[3] == 20)
                    {
                        return GetFactors(registers[5]).Sum();
                    }
                }

                return registers[0];
            }
        }
    }
}
