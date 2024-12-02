using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2024.Runner.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Takes the left-most count characters from s. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Left(this string s, int count)
        {
            if (count > s.Length)
            {
                return s;
            }

            return s.SubstringTo(0, count - 1);
        }

        /// <summary>
        /// Takes the right-most count characters from s. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Right(this string s, int count)
        {
            if (count > s.Length)
            {
                return s;
            }

            return s.SubstringTo(s.Length - count, s.Length);
        }

        /// <summary>
        /// Returns the substring of s from startIndex to endIndex inclusive of both endpoint characters. 
        /// This is a substitute for string.Substring if you already know the indices to make it easier
        /// instead of having to compute the count from the indices. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static string SubstringTo(this string s, int startIndex, int endIndex)
        { 
            if (endIndex < startIndex)
            {
                throw new ArgumentException("endIndex cannot be less than startIndex");
            }

            // If the user asks for an index that's beyond the maximum range, return the SubstringTo
            // the last index of the string
            if (endIndex >= s.Length)
            {
                return s.SubstringTo(startIndex, s.Length - 1);
            }

            // If startIndex and endIndex are the same, we still need to take 1 character (the character at that index)
            // hence adding 1 to the difference between them. 
            var length = endIndex - startIndex + 1;

            return s.Substring(startIndex, length);
        }

        /// <summary>
        /// Returns a version of s computed by shifting the characters in s to the left (beginning of the string)
        /// a distance number of times. Shifting by 0 or by the length of the string
        /// results in s being returned (as does shifting a multiple of length). 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static string RotateLeft(this string s, int distance)
        {
            // We don't need to loop the string, we only need to rotate it. 
            // We'll adjust the distance to be the true rotation distance
            // that won't result in a loop. 
            distance = distance % s.Length;

            // If the rotation would result in the string back in its original position, 
            // no need to do anything. This is based on distance being a multiple of length
            if (distance == 0)
            {
                return s;
            }

            var left = s.Left(distance);
            var right = s.Right(s.Length - distance);

            return right + left;
        }

        /// <summary>
        /// Returns a version of s computed by shifting the characters in s to the right (end of the string)
        /// a distance number of times. Returns s if distance is 0 or a multiple of s's length. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static string RotateRight(this string s, int distance)
        {
            // We can define RotateRight in terms of RotateLeft. 
            return s.RotateLeft(s.Length - distance);
        }

        /// <summary>
        /// Returns an array of strings from partitioning s into unique sets of partitionSize length. 
        /// Throws an exception if s's length is not a multiple of partitionSize. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="partitionSize"></param>
        /// <returns></returns>
        public static string[] Partition(this string s, int partitionSize)
        {
            if (s.Length % partitionSize != 0)
            {
                throw new ArgumentException($"Cannot partition string of length {s.Length} into partitions of size {partitionSize}");
            }

            var partitionCount = s.Length / partitionSize;

            return Enumerable.Range(0, partitionCount)
                             .Select(i => s.Substring(i * partitionSize, partitionSize)).ToArray();
        }

        /// <summary>
        /// Returns a string computed from removing the first character of s.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Chomp(this string s)
        {
            if (s.Length == 0)
            {
                return s;
            }

            return s.SubstringTo(1, s.Length);
        }

        /// <summary>
        /// Returns the indices of all occurrences of target in s. Returns empty array if none are found. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int[] IndexOfAll(this string s, char target)
        {
            return Enumerable.Range(0, s.Length)
                             .Where(i => s[i] == target)
                             .ToArray();
        }

        public static int[] SplitNumbers(this string s)
        {
            return s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(ss => Int32.Parse(ss))
                    .ToArray();
        }

        /// <summary>
        /// Returns the integer versions of all values that are convertible to int. 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<int> WhereInteger(this IEnumerable<string> values)
        {
            return values.Where(v => v.IsInteger())
                          .Select(v => Int32.Parse(v));
        }

        /// <summary>
        /// Returns the numeric (double) versions of all values that are convertible to double. 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<double> WhereNumeric(this IEnumerable<string> values)
        {
            return values.Where(v => v.IsNumeric())
                          .Select(v => Double.Parse(v));
        }

        public static bool IsInteger(this string s)
        {
            int num;

            return Int32.TryParse(s, out num);
        }

        public static bool IsNumeric(this string s)
        {
            double d;

            return Double.TryParse(s, out d);
        }
        
    }
}
