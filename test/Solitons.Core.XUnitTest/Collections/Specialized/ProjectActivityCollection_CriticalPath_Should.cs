using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Solitons.Collections.Specialized;

// ReSharper disable once InconsistentNaming
public sealed  class ProjectActivityCollection_CriticalPath_Should
{
    [Theory]
    [InlineData(1, 1, 1, 1, 1, 1, "1 2 3 5")]
    [InlineData(1, 1, 1, 100, 1, 1, "4 3 5")]
    [InlineData(1, 1, 1, 1, 1, 100, "6 5")]
    public void HandleSixStepsDemoCase(int duration1, int duration2, int duration3, int duration4, int duration5, int duration6, string expectedPathExpression)
    {
        var project = new ProjectActivityCollection();
        var activity1 = project.Add("Activity 1", duration1);
        var activity4 = project.Add("Activity 4", duration4);
        var activity6 = project.Add("Activity 6", duration6);
        var activity2 = project.Add("Activity 2", duration2, activity1);
        var activity3 = project.Add("Activity 3", duration3, activity2, activity4);
        var activity5 = project.Add("Activity 5", duration5, activity3, activity6);

        var expectedPath = Regex
            .Split(expectedPathExpression, @"\s+")
            .Skip(string.IsNullOrWhiteSpace)
            .Select(int.Parse)
            .Select(id => project.Single(a=> a.Id == $"Activity {id}"))
            .ToList();

        var actualPath = project
            .GetCriticalPath()
            .ToList();

        Assert.Equal(expectedPath.Count, actualPath.Count);
        for(int i = 0; i < actualPath.Count; i++)
        {
            Assert.Equal(expectedPath[i], actualPath[i]);
        }
    }
}