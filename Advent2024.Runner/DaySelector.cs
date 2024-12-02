using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advent2024.Runner.Extensions;

namespace Advent2024.Runner
{
    public class DaySelector
    {
        private IList<Type> adventProblems;

        public DaySelector()
        {
            this.adventProblems = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                                        .Where(t => typeof(IAdventProblem).IsAssignableFrom(t) && t.IsClass).ToList();
        }

        public void PrintAvailableDays()
        {
            Console.WriteLine("Days\n===============================");

            var dayNames = this.adventProblems.Select(ap => ap.Name).OrderBy(ap => ap.Length).ThenBy(ap => ap);

            dayNames.ToList().ForEach(d => Console.WriteLine(d));
        }

        public void Run(string toRun)
        {
            this.Select(this.adventProblems, toRun)();
        }

        private Action Select(IEnumerable<Type> days, string toRun)
        {
            var day = GetDay(toRun);
            var part = GetPart(toRun).ToUpper();

            var targetTypeName = "Day" + day;

            var type = days.FirstOrDefault(d => d.Name.Equals(targetTypeName));

            if (type == null)
            {
                throw new ArgumentException($"Cannot find an IAdventProblem type for input '{toRun}'");
            }

            var adventProblem = Activator.CreateInstance(type) as IAdventProblem;

            return CreateMethodCall(adventProblem, part);
        }

        private string GetDay(string toRun)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"[1-2]?[0-9]");

            if (!regex.IsMatch(toRun))
            {
                throw new ArgumentException($"Unable to find a day based on the input '${toRun}'");
            }

            var day = regex.Match(toRun).Value;

            return day;
        }

        private string GetPart(string toRun)
        {
            return toRun.Right(1);
        }

        private Action CreateMethodCall(IAdventProblem problem, string part)
        {
            if (part.Equals("A"))
            {
                return () => problem.A();
            }

            if (part.Equals("B"))
            {
                return () => problem.B();
            }

            throw new ArgumentException($"Could not create method call expression for part '{part}'");
        }
    }
}
