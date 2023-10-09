using System;
using System.Collections.Generic;
using Xunit;

namespace Solitons.Collections.Specialized;

// ReSharper disable once InconsistentNaming
public sealed class ProjectActivity_GetFloatTime_Should
{
    [Fact]
    public void DetectNoSlack()
    {
        var activity1 = new ProjectActivity("Activity 1", 5, new List<ProjectActivity>());
        var floatTime = activity1.GetFloatTime(5, 0);

        Assert.Equal(0, floatTime);
    }

    [Fact]
    public void ThrowExceptionForNegativeProjectEndTime()
    {
        var activity1 = new ProjectActivity("Activity 1", 5, new List<ProjectActivity>());
        Assert.Throws<ArgumentOutOfRangeException>(() => activity1.GetFloatTime(-1, 0));
    }

    [Fact]
    public void ThrowExceptionForNegativeEarliestStart()
    {
        var activity1 = new ProjectActivity("Activity 1", 5, new List<ProjectActivity>());
        Assert.Throws<ArgumentOutOfRangeException>(() => activity1.GetFloatTime(5, -1));
    }



    [Fact]
    public void CalculateSlack()
    {
        var activity1 = new ProjectActivity("Activity 1", 3, new List<ProjectActivity>());
        var floatTime = activity1.GetFloatTime(5, 0);

        Assert.Equal(2, floatTime);
    }

    [Fact]
    public void ReturnZeroFloatTime_WhenActivityIsCritical()
    {
        var activity1 = new ProjectActivity("Activity 1", 3, new List<ProjectActivity>());

        // Set the project end time to exactly match activity1's effort in days,
        // making it critical.
        var projectEndTime = activity1.EffortInDays;

        var floatTime = activity1.GetFloatTime(projectEndTime, 0);

        Assert.Equal(0, floatTime);
    }


    [Fact]
    public void ReturnFloatTime_WhenDependencyFilterIsApplied()
    {
        var activity1 = new ProjectActivity("Activity 1", 3, new List<ProjectActivity>());
        var activity2 = new ProjectActivity("Activity 2", 2, new List<ProjectActivity>());

        // Assume activity3 depends on both activity1 and activity2
        var activity3 = new ProjectActivity("Activity 3", 4, new List<ProjectActivity> { activity1, activity2 });

        // Only consider activity1 as a dependency. This would mean that the project would end at 7 (3 + 4).
        bool Filter(ProjectActivity activity) => activity.Id.Equals(activity1.Id);

        // Changed earliestStart from 3 to 0 to align with the logic.
        var floatTime = activity3.GetFloatTime(7, 0, Filter);

        Assert.Equal(0, floatTime);  // Should be on the critical path given the filter
    }
}