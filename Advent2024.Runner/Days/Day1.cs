using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent2024.Runner.Extensions;

namespace Advent2024.Runner.Days
{
    public class Day1 : IAdventProblem
    {
        public void A()
        {
            var numbers = FileSystem.MakeDataFilePath("day1")
                                    .Read(s => s.SplitNumbers());

            var first = numbers.Slice(0).OrderBy(f => f);
            var second = numbers.Slice(1).OrderBy(s => s);

            var distance = first.Zip(second, (f, s) => Math.Abs(f - s)).Sum();

            Console.WriteLine($"Distance = {distance}");

        }

        public void B()
        {
            var numbers = FileSystem.MakeDataFilePath("day1")
                                    .Read(s => s.SplitNumbers());

            var first = numbers.Slice(0).Counts();
            var second = numbers.Slice(1).Counts();

            var similarity = 0;

            foreach (var f in first)
            {
                if (second.ContainsKey(f.Key))
                {
                    similarity += (f.Key * second[f.Key] * f.Value);
                }
            }

            Console.WriteLine($"Similarity = {similarity}");
        }
    }
}
