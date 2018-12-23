using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar
{
    partial class Program
    {
        private static class Day21
        {
            private const int REGISTERS = 6;

            public static void Day21_1_2(IReadOnlyList<string> input)
            {
                var registers    = new int[REGISTERS];
                var ipRegister   = Day19.GetIpRegister(input);
                var instructions = Day19.GetInstructions(input);
                var operators    = Day19.GetOperators();

                var set         = new HashSet<int>();
                int lastNewSeen = 0;

                while (registers[ipRegister] < instructions.Length)
                {
                    var instruction = instructions[registers[ipRegister]];
                    registers = operators[instruction[0]](registers, instruction);

                    if (registers[ipRegister] == 28 &&
                        !set.Contains(registers[3]))
                    {
                        lastNewSeen = registers[3];
                        if (!set.Any())
                            Console.WriteLine($"Part 1: {lastNewSeen}");
                        set.Add(lastNewSeen);
                    }
                    else if (registers[ipRegister] == 28)
                    {
                        Console.WriteLine($"Part 2: {lastNewSeen}");
                        break;
                    }

                    registers[ipRegister]++;
                }
            }
        }
    }
}
