using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2024.Runner.Days
{
    public class Day2 : IAdventProblem
    {
        public void A()
        {
            var reports = FileSystem.MakeDataFilePath("day2")
                                     .Read(s => s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(st => Int32.Parse(st)).ToArray());

            var monitor = new LevelSafetyMonitor();

            var howManySafe = reports.Where(r => monitor.IsSafe(r)).Count();

            Console.WriteLine($"How Many Safe?    {howManySafe}");
        }

        public void B()
        {
            throw new NotImplementedException();
        }
    }

    public class LevelSafetyMonitor
    {

        public bool IsSafe(int[] levels)
        {
            var diffs = GetPairDifferences(levels);

            // If the differences between all the pairs aren't all the same sign, then the levels
            // aren't monontonic so aren't safe. 
            if (diffs.Any(d => Math.Sign(d) == 1) && diffs.Any(d => Math.Sign(d) == -1))
            {
                return false;
            }

            // All the pair differences should be bewteen 1 and 3 inclusive. 
            // Have to remember to take the absolute value of the difference because we 
            // didn't care above if the sequence was increasing or decreasing
            return diffs.All(d => Math.Abs(d) >= 1 && Math.Abs(d) <= 3);
        }


        public bool IsSafeWithDampening(int[] levels)
        {
            if (IsSafe(levels))
            {
                return true;
            }

            var sequenceType = WhatIsSequenceType(levels);

            // Determine if sequence increases or decreases based on comparing first and last elements. Hopefully that's enough
            // UNLESS one of those elements is the one that violates the rule. 

            // A mixed sequence (one with more than 1 case of each trend) cannot be made safe via dampening. 
            if (sequenceType == SequenceType.Mixed)
            {
                return false;
            }

            return false;

        }

        enum SequenceType { Mixed, Strictly_Increasing, Strictly_Decreasing, Increasing_Except_1, Decreasing_Except_1 };


        private int[] GetPairDifferences(int[] levels)
        {
            return Enumerable.Range(0, levels.Count() - 1)
                             .Select(i => levels[i] - levels[i + 1]).ToArray();
        }

        private SequenceType WhatIsSequenceType(int[] levels)
        {
            var diffs = GetPairDifferences(levels);

            // If there are 2 or more levels that increase AND 2 or more that decrease, there's no way
            // to apply dampening. Dampening will only work if there's only a single increase/decrease violator. 

            var howManyIncrease = diffs.Count(d => Math.Sign(d) == 1);
            var howManyDecrease = diffs.Count(d => Math.Sign(d) == -1);

            if (howManyIncrease == 0 && howManyDecrease > 0)
            {
                return SequenceType.Strictly_Decreasing;
            }

            if (howManyIncrease > 0 && howManyDecrease == 0)
            {
                return SequenceType.Strictly_Increasing;
            }

            if (howManyIncrease == 1 && howManyDecrease > 0)
            {
                return SequenceType.Decreasing_Except_1;
            }

            if (howManyIncrease > 0 && howManyDecrease == 0)
            {
                return SequenceType.Increasing_Except_1;
            }

            return SequenceType.Mixed;
        }


        private bool IsSafe(int[] levels, int removeIndex)
        {
            // Have to deal with the fact that 0 or 1 could be the removeIndex. 
            var increasing = levels[0] < levels[1];

            var i = 0;
             
            while (i < levels.Count() - 1)

            //for (int i = 0; i < levels.Count() - 1; ++i)
            {
                // 3 cases to consider:
                // 1. We're on removeIndex, which we can avoid
                // 2. removeIndex is one behind the current one, which we can avoid. 
                // 3. removeIndex is the next one from the current one. This case we have to handle 

               // var checkInex = levels + 1

                // If the levels are increasing and the current level is bigger than the next level, unsafe
                if (increasing && levels[i] > levels[i + 1])
                {
                    return false;
                }

                // If the levels are decreasing and the current level is smaller than the next level, unsafe
                if (!increasing && levels[i] < levels[i + 1])
                {
                    return false;
                }

                // The difference between the current level and the next level must be from 1 to 3, inclusive
                var diff = Math.Abs(levels[i] - levels[i + 1]);

                if (diff < 1 || diff > 3)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
