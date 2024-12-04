using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent2024.Runner.Days
{
    public class Day3 : IAdventProblem
    {
        public void A()
        {
            var input = FileSystem.MakeDataFilePath("day3")
                                   .Read();                                  

            var result = input.SelectMany(p => FindAllMulInstructions(p))
                              .Select(i => ExtractNumbers(i))
                              .Sum(t => t.Item1 * t.Item2);

            Console.WriteLine($"Instruction Result = {result}");
        }


        private IEnumerable<Match> FindAllMulInstructions(string input)
        {
            var pattern = @"mul\([0-9]{1,3}\,[0-9]{1,3}\)";

            var matches = Regex.Matches(input, pattern);

            return matches.Cast<Match>();
        }

        private (int, int) ExtractNumbers(Match mulInstruction)
        {
            var numberStrings = Regex.Matches(mulInstruction.Value, @"\d+");

            var numbers = numberStrings.Cast<Match>().Select(m => Int32.Parse(m.Value));

            if (numbers.Count() != 2)
            {
                throw new ArgumentException($"Instruction '{mulInstruction.Value}' does not have 2 numbers in it.");
            }

            return (numbers.First(), numbers.Last());
        }


        public void B()
        {
            var instruction = FileSystem.MakeDataFilePath("day3").ReadCombined();

            var enabledInstructionSections = FindEnabledInstructionSections(instruction);

            var result = enabledInstructionSections
                                                 .SelectMany(i => FindAllMulInstructions(i.Value))
                                                 .Select(m => ExtractNumbers(m))
                                                 .Select(m => m.Item1 * m.Item2)
                                                 .Sum();

            Console.WriteLine($"Instruction Result = {result}");
        }      

        private IEnumerable<Match> FindEnabledInstructionSections(string instruction)
        {
            var pattern = @"(^.*?don't\(\))|(do\(\).*?don't\(\))|(do\(\).*?$)";

            var matches = Regex.Matches(instruction, pattern);

            return matches.Cast<Match>();
        }

        private IEnumerable<Match> GetMulInstructionsBeforeFirstDont(string instruction)
        {
            var matches = Regex.Matches(instruction, @"mul\(\d+\,\d+\).*don't\(\)");

            return matches.Cast<Match>();
        }        
    }
}
