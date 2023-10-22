using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Collections.Specialized;

/// <summary>
/// Represents an individual activity in a project, providing methods for determining its role in the critical path.
/// </summary>
/// <remarks>
/// The critical path represents the longest path of planned activities to the end of the project, and the earliest and latest that each activity can start and end without making the project longer.
/// </remarks>
public sealed class ProjectActivity
{
    [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
    private readonly List<ProjectActivity> _dependencies;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectActivity"/> class with the specified ID, effort in days, and dependencies.
    /// </summary>
    /// <param name="id">Unique identifier for the activity.</param>
    /// <param name="effortInDays">Effort in days required to complete the activity.</param>
    /// <param name="dependencies">List of activities that must be completed before this one can start.</param>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="id"/> parameter is null or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="effortInDays"/> parameter is negative.</exception>
    /// <remarks>
    /// The constructor is designed to be invoked internally, as part of the factory pattern. It performs basic input validation and initializes the state of the activity.
    /// </remarks>
    internal ProjectActivity(
        string id, 
        int effortInDays,
        List<ProjectActivity> dependencies)
    {
        Debug.Assert(!id.IsNullOrWhiteSpace());
        Debug.Assert(effortInDays >= 0);
        Debug.Assert(dependencies != null);

        Id = id;
        EffortInDays = effortInDays;
        _dependencies = dependencies;

    }

    internal IEnumerable<ProjectActivity> Dependencies => _dependencies;

    /// <summary>
    /// Gets the unique identifier for this activity.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the effort in days required to complete this activity.
    /// </summary>
    public int EffortInDays { get; }


    /// <summary>
    /// Calculates and returns the critical path leading to this activity.
    /// </summary>
    /// <returns>
    /// A stack of <see cref="ProjectActivity"/> objects representing the critical path to this activity, ordered from the first to be executed to this activity.
    /// </returns>
    /// <remarks>
    /// The critical path is determined by evaluating the longest path (in terms of effort in days) among the dependencies leading to this activity. The function is recursive and takes into account the critical path of each dependency.
    /// </remarks>
    /// <example>
    /// <![CDATA[
    /// var criticalPathStack = someActivity.CriticalPath;
    /// foreach (var activity in criticalPathStack)
    /// {
    ///     Console.WriteLine($"{activity.Id} - {activity.EffortInDays} days");
    /// }
    /// ]]>
    /// </example>
    public Stack<ProjectActivity> CriticalPath
    {
        get
        {
            var criticalPath = new Stack<ProjectActivity>();

            criticalPath.Push(this);

            _dependencies
                .Select(dependency => dependency.CriticalPath)
                .OrderByDescending(path => path.Sum(a => a.EffortInDays))
                .Take(1)
                .SelectMany(p => p)
                .Reverse()
                .ForEach(activity => criticalPath.Push(activity));

            return criticalPath;
        }
    }

    /// <summary>
    /// Calculates the Float Time (Slack Time) for this activity. Float time is the amount of time that 
    /// this activity can be delayed without affecting the project's completion date.
    /// </summary>
    /// <param name="projectEndTime">The calculated end time (in days since the project's start date) of the entire project.</param>
    /// <param name="earliestStart">The earliest start time (in days since the project's start date) for this specific activity.</param>
    /// <param name="dependencyFilter">An optional predicate for filtering dependencies. If provided, only dependencies that satisfy this condition will be considered in the calculation. If null, all dependencies are considered.</param>
    /// <returns>The float time (slack time) for this activity, measured in days.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if projectEndTime or earliestStart is negative.</exception>
    /// <example>
    /// <![CDATA[
    /// int floatTime = someActivity.GetFloatTime(projectEndTime: 100, earliestStart: 10);
    /// Console.WriteLine($"Float time: {floatTime} days");
    /// ]]>
    /// </example>
    public int GetFloatTime(int projectEndTime, int earliestStart, Func<ProjectActivity, bool>? dependencyFilter = null)
    {
        // Validate the input parameters
        if (projectEndTime < 0 || earliestStart < 0)
        {
            throw new ArgumentOutOfRangeException($@"{nameof(projectEndTime)} and {nameof(earliestStart)} must be non-negative");
        }
        dependencyFilter ??= (_) => true;
        // Calculate the latest start time for this activity.
        int latestStart = CalculateLatestStart(projectEndTime, dependencyFilter);
        Debug.WriteLine($"Latest Start: {latestStart}, Earliest Start: {earliestStart}");  // Debugging line

        // Calculate and return the float time (slack time).
        return latestStart - earliestStart;
    }

    /// <summary>
    /// Determines if the activity is critical based on the provided parameters.
    /// </summary>
    /// <param name="projectEndTime">The calculated end time (in days since the project's start date) of the entire project.</param>
    /// <param name="earliestStart">The earliest start time (in days since the project's start date) for this specific activity.</param>
    /// <param name="dependencyFilter">An optional predicate for filtering dependencies. If provided, only dependencies that satisfy this condition will be considered in the calculation. If null, all dependencies are considered.</param>
    /// <returns>A boolean indicating whether the activity is critical.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if projectEndTime or earliestStart is negative.</exception>
    /// <example>
    /// <![CDATA[
    /// bool isCritical = someActivity.IsCritical(projectEndTime: 100, earliestStart: 10);
    /// Console.WriteLine($"Is activity critical? {isCritical}");
    /// ]]>
    /// </example>
    public bool IsCritical(
        int projectEndTime, 
        int earliestStart, 
        Func<ProjectActivity, bool>? dependencyFilter = null)
    {
        return GetFloatTime(projectEndTime, earliestStart, dependencyFilter) == 0;
    }


    /// <summary>
    /// Calculates the Latest Start Time (LS) for the activity.
    /// </summary>
    /// <param name="projectEndTime">The end time of the project in days since project start.</param>
    /// <param name="dependencyFilter">An optional filter function for considering specific dependencies. All dependencies are considered if null.</param>
    /// <returns>The latest start time in days since project start.</returns>
    private int CalculateLatestStart(int projectEndTime, Func<ProjectActivity, bool> dependencyFilter)
    {
        if (_dependencies.Count == 0 || !_dependencies.Any(dependencyFilter))
        {
            Debug.WriteLine($"Entered the if block: projectEndTime = {projectEndTime}, EffortInDays = {EffortInDays}"); // Debugging line
            return projectEndTime - EffortInDays;
        }

        int minLatestStartAmongDependencies = _dependencies
            .Where(dependencyFilter)
            .Min(d => d.CalculateLatestStart(projectEndTime, dependencyFilter));

        return minLatestStartAmongDependencies - EffortInDays;
    }



    /// <summary>
    /// Returns the unique identifier for this activity.
    /// </summary>
    /// <returns>A string that uniquely identifies this activity.</returns>
    public override string ToString() => Id;

    /// <inheritdoc />
    public override int GetHashCode() => Id.ToUpper().GetHashCode();

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if(obj == null) return false;
        if(ReferenceEquals(this, obj)) return true;
        if(obj is ProjectActivity other) 
            return Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
        return false;
    }
}