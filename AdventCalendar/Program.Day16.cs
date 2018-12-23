using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day16
        {
            private const int OPCODES = 16;

            public static int Day16_1(IReadOnlyCollection<string> input)
            {
                var regexB    = new Regex(@"Before: \[(\d+).*?(\d+).*?(\d+).*?(\d+)\]");
                var regexA    = new Regex(@"After:  \[(\d+).*?(\d+).*?(\d+).*?(\d+)\]");
                var regexC    = new Regex(@"^(\d+).*?(\d+).*?(\d+).*?(\d+)");
                var before    = GetInputs(input, regexB);
                var after     = GetInputs(input, regexA);
                var op        = GetInputs(input, regexC);

                var operators = GetOperators();

                int ret = 0;
                for (int i = 0; i < op.Length; i++)
                {
                    int count = operators.Select(@operator => @operator(before[i], op[i]))
                                         .Count(registers => registers.SequenceEqual(after[i]));

                    if (count >= 3)
                    {
                        ret++;
                    }
                }

                return ret;
            }

            public static int Day16_2(IReadOnlyCollection<string> input)
            {
                var registers = new int[4];

                var opcodes   = GetOpcodes(input);
                var operators = GetOperators();
                var program   = GetProgram();

                foreach (var i in program)
                {
                    int opcode    = opcodes[i[0]];
                    var @operator = operators[opcode];
                    registers = @operator(registers, i);
                }

                return registers[0];
            }

            private static IEnumerable<int[]> GetProgram()
            {
                var regex = new Regex(@"(\d+) (\d+) (\d+) (\d+)");
                var input  = File.ReadAllLines(@"C:\Users\Stefano\Documents\input16.txt");
                var output = new int[input.Length][];

                int i = 0;
                foreach (string line in input)
                {
                    var match = regex.Match(line);

                    if (!match.Success) continue;

                    output[i] = new int[4];

                    for (int index = 0; index < 4; index++)
                    {
                        int value = int.Parse(match.Groups[index + 1].Value);
                        output[i][index] = value;
                    }

                    i++;
                }

                return output;
            }

            private static Dictionary<int, int> GetOpcodes(IReadOnlyCollection<string> input)
            {
                var opcodes = new Dictionary<int, int>();

                var regexB = new Regex(@"Before: \[(\d+).*?(\d+).*?(\d+).*?(\d+)\]");
                var regexA = new Regex(@"After:  \[(\d+).*?(\d+).*?(\d+).*?(\d+)\]");
                var regexC = new Regex(@"^(\d+).*?(\d+).*?(\d+).*?(\d+)");
                var before = GetInputs(input, regexB);
                var after  = GetInputs(input, regexA);
                var op     = GetInputs(input, regexC);

                var test = new bool[OPCODES, OPCODES];
                for (int i = 0; i < OPCODES; i++)
                for (int j = 0; j < OPCODES; j++)
                {
                    test[i, j] = true;
                }

                var operators = GetOperators();

                for (int i = 0; i < op.Length; i++)
                {
                    for (int opcode = 0; opcode < operators.Length; opcode++)
                    {
                        var @operator = operators[opcode];
                        var registers = @operator(before[i], op[i]);

                        if (!registers.SequenceEqual(after[i]))
                        {
                            test[op[i][0], opcode] = false;
                        }
                    }
                }

                while (opcodes.Count != OPCODES)
                {
                    for (int i = 0; i < OPCODES; i++)
                    {
                        int count = 0;
                        int code  = 0;

                        for (int j = 0; j < OPCODES; j++)
                        {
                            if (test[i, j])
                            {
                                count++;
                                code = j;
                            }
                        }

                        if (count != 1) continue;

                        opcodes.Add(i, code);
                        for (int j = 0; j < OPCODES; j++)
                        {
                            test[j, code] = false;
                        }
                    }
                }

                return opcodes;
            }

            private static Func<int[], int[], int[]>[] GetOperators()
            {
                var operators = new Func<int[], int[], int[]>[]
                                {
                                    (a, b) => // addr
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] + a[b[2]];

                                        return output;
                                    },

                                    (a, b) => // addi
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] + b[2];

                                        return output;
                                    },

                                    (a, b) => // mulr
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] * a[b[2]];

                                        return output;
                                    },

                                    (a, b) => // muli
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] * b[2];

                                        return output;
                                    },

                                    (a, b) => // banr
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] & a[b[2]];

                                        return output;
                                    },

                                    (a, b) => // bani
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] & b[2];

                                        return output;
                                    },

                                    (a, b) => // borr
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] | a[b[2]];

                                        return output;
                                    },

                                    (a, b) => // bori
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] | b[2];

                                        return output;
                                    },

                                    (a, b) => // setr
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]];

                                        return output;
                                    },

                                    (a, b) => // seti
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = b[1];

                                        return output;
                                    },

                                    (a, b) => // gtir
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = b[1] > a[b[2]] ? 1 : 0;

                                        return output;
                                    },

                                    (a, b) => // gtri
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] > b[2] ? 1 : 0;

                                        return output;
                                    },

                                    (a, b) => // gtrr
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] > a[b[2]] ? 1 : 0;

                                        return output;
                                    },

                                    (a, b) => // eqir
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = b[1] == a[b[2]] ? 1 : 0;

                                        return output;
                                    },

                                    (a, b) => // eqri
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] == b[2] ? 1 : 0;

                                        return output;
                                    },

                                    (a, b) => // eqrr
                                    {
                                        var output = new int[4];
                                        Array.Copy(a, output, a.Length);

                                        output[b[3]] = a[b[1]] == a[b[2]] ? 1 : 0;

                                        return output;
                                    }
                                };
                return operators;
            }

            private static int[][] GetInputs(IReadOnlyCollection<string> input, Regex regex)
            {
                var output = new int[input.Count / 4 + 1][];

                int i = 0;
                foreach (string line in input)
                {
                    var match = regex.Match(line);

                    if (!match.Success) continue;
                    
                    output[i] = new int[4];

                    for (int index = 0; index < 4; index++)
                    {
                        int value = int.Parse(match.Groups[index + 1].Value);
                        output[i][index] = value;
                    }

                    i++;
                }

                return output;
            }
        }
    }
}
