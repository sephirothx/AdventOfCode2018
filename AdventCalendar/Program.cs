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
            Day24.Day24_2();
            Console.WriteLine(DateTime.Now.TimeOfDay);
        }
    }
}
