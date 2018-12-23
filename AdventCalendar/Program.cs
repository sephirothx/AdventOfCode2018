using System;
using System.IO;

namespace AdventCalendar
{
    partial class Program
    {
        private static void Main()
        {
            const string PATH  = @"C:\Users\User\Documents\input.txt";
            var          input = File.ReadAllLines(PATH);

            Console.WriteLine(DateTime.Now.TimeOfDay);
            Day23.Day23_2(input);
            Console.WriteLine(DateTime.Now.TimeOfDay);
        }
    }
}
