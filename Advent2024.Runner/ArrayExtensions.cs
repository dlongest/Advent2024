using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2024.Runner.Extensions
{
    public static partial class ArrayExtensions
    {
        /// <summary>
        /// Create a sequence of consecutively paired values. The values overlap i.e. a sequence [1,2,3] becomes [[1,2],[2,3]] 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<T, T>> Pair<T>(this IEnumerable<T> values)
        {
            var count = values.Count();

            return Enumerable.Range(0, count - 1)
                             .Select(i => Tuple.Create<T, T>(values.ElementAt(i), values.ElementAt(i + 1)));

        }

        /// <summary>
        /// Create a sequence of collections by breaking the provided set of values into a collection of size windowSize. 
        /// This can result in the final collection being a different size than the others depending on if the size of values
        /// is a multiple of windowSize. This condition is not checked. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static IEnumerable<T[]> Window<T>(this IEnumerable<T> values, int windowSize)
        {
            var ar = values.ToArray();

            return Enumerable.Range(0, ar.Length - windowSize + 1)
                             .Select(i => ar.TakeFrom(i, windowSize));
        }

        /// <summary>
        /// For the given array ar, takes count items beginning at startIndex and returns that subset
        /// as a new array. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ar"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T[] TakeFrom<T>(this T[] ar, int startIndex, int count)
        {
            var subset = new List<T>();

            for (int offset = 0; offset < count; ++offset)
            {
                subset.Add(ar[startIndex + offset]);
            }

            return subset.ToArray();
        }

        /// <summary>
        /// For ar, a collection of row arrays, returns the collection generated from columnIndex in each row array. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ar"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static T[] Slice<T>(this IEnumerable<T[]> ar, int columnIndex)
        {
            return ar.Select(v => v[columnIndex]).ToArray();
        }

        /// <summary>
        /// For ar, a collection of row arrays, returns the collection generated from columnIndex in each row array. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ar"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static IEnumerable<char> Slice(this string[] ar, int columnIndex)
        {
            return ar.Select(v => v[columnIndex]).ToArray();
        }

        /// <summary>
        /// Returns the subset of ar beginning at startIndex and consuming size elements. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ar"></param>
        /// <param name="startIndex"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static T[] Subset<T>(this T[] ar, int startIndex, int size)
        {
            var r = new T[size];

            foreach (var index in Enumerable.Range(0, size))
            {
                r[index] = ar[startIndex + index];
            }

            return r;
        }


        /// <summary>
        /// Given an array of numbers, returns a dictionary where each value in ar is the key whose value
        /// is the number of times that value appears in ar. 
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        public static IDictionary<int, int> Counts(this IEnumerable<int> ar)
        {
            var dict = new Dictionary<int, int>();

            foreach (var n in ar)
            {
                if (!dict.ContainsKey(n))
                {
                    dict.Add(n, 0);
                }

                dict[n]++;
            }

            return dict;
        }
    }


    public static partial class QueueExtensions
    {
        /// <summary>
        /// Enqueues each item, in order, onto q. Has the same result as calling q.Enqueue() for each item. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="items"></param>
        public static void EnqueueAll<T>(this Queue<T> q, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                q.Enqueue(item);
            }
        }
    }
}
