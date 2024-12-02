using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2024.Runner
{
    /// <summary>
    /// FileSystem is here to assist in opening and reading files off the local file system. Out of the box it expects
    /// files to be in a directory called Data that is accessible from the execution point
    /// and will intuit all file names as being within that directory. You can change this through FileSystem.DataDirectory.
    /// Note that some of these functions will read the entire file into memory so if the file is particularly large, 
    /// it will put a lot of pressure on the system memory. Generally in this project, input files are limited in size and
    /// this isn't an issue. In particular, Read streams its results back via the enuemrator, but ReadGroups reads it
    /// all into memory. 
    /// 
    /// The primary functions are:
    /// 
    /// * MakeDataFilePath(string) is used to take a simplified filename (filename only, no extension, .txt assumed) 
    ///     and turn it into a path that includes the DataDirectory static member pre-pended to it. 
    ///     
    /// * Read(string csvFilePath, Func<string, T> lineConverter) takes a CSV file path and a function to generate
    ///     type T from a line in the file and returns a collection of T's by calling lineConverter on each line. 
    ///     
    /// * ReadGroups(string csvFilePath, Func<string, bool> groupSeparator) is used when the CSV file contains lines
    ///     that form some sort of structure within the file such that each line is not equivalent in form. The provided
    ///     Func is used to indicate when a given group has been concluded. ReadGroups will then bunch up each set of 
    ///     lines into a group and return that as a collection. It will then keep processing through the file beginning
    ///     with a new group. 
    /// 
    /// The standard usage of FileSystem is similar to:
    ///     var n = FileSystem.MakeDataFilePath("sample")
    ///                       .Read(line => Int32.Parse(line));
    ///                       
    /// This will read the file sample.txt and assuming each row in the file contains a single number, n will
    /// be the collection containing each of those numbers, as an integer. 
    /// 
    /// If the line is more complicated than a simple conversion, this form becomes more useful:
    ///     var output = FileSystem.MakeDataFilePath("sample.txt")
    ///                            .Read(line => DoSomethingTo(line));
    ///                            
    /// Now, DoSomethingTo is a function that takes a single string as input and is responsible for parsing the line
    /// into the right form and then orchestrating the next layer of logic. Many times, DoSomethingTo is a constructor
    /// function (a static function on a class of interest) that performs the parsing and validation before delegating
    /// to an instance of the class it creates. However, there's flexibility in how you combine these together. 
    ///                            
    /// A basic grouped file that separates each group by a blank line can be read as:
    ///     var input = FileSystem.MakeDataFilePath("day4")
    ///                           .ReadGroups(s => s.Length == 0);
    /// 
    /// The format of each group can be different and it's up to the subsequent parsing logic to know the structure
    /// and parse/validate/transform the groups as necessary. 
    /// 
    /// 
    /// </summary>
    public static class FileSystem
    {
        public static string DataDirectory = @"Data\";

        /// <summary>
        /// Returns the relative path to the text file whose name (without extension) is filenamePrefix.
        /// </summary>
        /// <param name="filenamePrefix"></param>
        /// <returns></returns>
        public static string MakeDataFilePath(string filenamePrefix)
        {
            return $"{DataDirectory}{filenamePrefix}.txt";
        }

        /// <summary>
        /// Returns each line in csvFilePath after it's processed by lineConverter. 
        /// Evaluation is lazy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvFilePath"></param>
        /// <param name="lineConverter"></param>
        /// <returns></returns>
        public static IEnumerable<T> Read<T>(this string csvFilePath, Func<string, T> lineConverter)
        {
            using (var reader = new StreamReader(csvFilePath))
            {
                while (!reader.EndOfStream)
                {
                    yield return lineConverter(reader.ReadLine());
                }
            }
        }

        /// <summary>
        /// Reads and returns each line from csvFilePath, as-is. 
        /// </summary>
        /// <param name="csvFilePath"></param>
        /// <returns></returns>
        public static IEnumerable<string> Read(this string csvFilePath)
        {
            return csvFilePath.Read(s => s);
        }

        /// <summary>
        /// Returns the contents of csvFilePath grouped into separate string arrays based on
        /// detecting a separator via groupSeparator. File read is greedy and processed to the end before returned. 
        /// </summary>
        /// <param name="csvFilePath"></param>
        /// <param name="groupSeparator"></param>
        /// <returns></returns>
        public static IEnumerable<string[]> ReadGroups(this string csvFilePath, Func<string, bool> groupSeparator)
        {
            var allLines = csvFilePath.Read().ToArray();

            var grouped = new List<string[]>();

            var inProgress = new List<string>();

            for (var index = 0; index < allLines.Length; ++index)
            {
                if (!groupSeparator(allLines[index]))
                {
                    inProgress.Add(allLines[index]);
                }
                else
                {
                    grouped.Add(inProgress.ToArray());
                    inProgress.Clear();
                }
            }

            if (inProgress.Any())
            {
                grouped.Add(inProgress.ToArray());
            }

            return grouped;
        }    
    }
}
