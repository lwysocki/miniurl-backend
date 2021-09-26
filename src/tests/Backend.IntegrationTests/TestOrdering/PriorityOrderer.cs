using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace MiniUrl.IntegrationTests.TestOrdering
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var sortedTests = new SortedDictionary<int, List<TTestCase>>();

            foreach (var testCase in testCases)
            {
                var priority = testCase.TestMethod.Method.GetCustomAttributes(typeof(TestPriorityAttribute))
                    .Last().GetNamedArgument<int>("Priority");

                if (!sortedTests.ContainsKey(priority))
                    sortedTests.Add(priority, new List<TTestCase>());

                sortedTests[priority].Add(testCase);
            }

            foreach (var tests in sortedTests.Keys.Select(priority => sortedTests[priority]))
            {
                var ordered = tests.OrderBy(t => t.TestMethod.Method.Name);

                foreach (var testCase in ordered) yield return testCase;
            }
        }
    }
}
