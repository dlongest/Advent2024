using Advent2024.Runner.Days;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2024.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var daySelector = new DaySelector();

            Console.Write("What problem do you want to run?    ");
            var input = Console.ReadLine();

            Console.WriteLine($"Running Day {input}");

            daySelector.Run(input);

            PrintFooter();
        }

        private static void PrintFooter()
        {
            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
