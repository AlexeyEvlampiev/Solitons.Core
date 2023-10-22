using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Solitons.Collections.FluentEnumerable;

namespace Solitons.Collections.Specialized;

/// <summary>
/// Represents an ordered collection of interdependent <see cref="ProjectActivity"/> items,
/// optimized for critical path analysis using the Critical Path Method (CPM).
/// </summary>
/// <remarks>
/// <para>
/// This collection is designed to model and analyze a project's activities, allowing for
/// the determination of the project's critical path, which is the longest path through the project
/// with the least amount of slack.
/// </para>
/// <example>
/// Below is an example of how to use the <see cref="ProjectActivityCollection"/> class to model and analyze a project:
/// <![CDATA[
/// using Solitons.Collections.Specialized;
///
/// namespace UsageExamples.Collections.Specialized
/// {
///     public sealed class ExampleModelingSoftwareProject
///     {
///         public void Example()
///         {
///             var project = new ProjectActivityCollection();
///             var projectKickOff = project.Add("Project Kick-off", 5);
///             var setupAzureResourceGroup = project.Add("Setup Azure Resource Group", 15, projectKickOff);
///             //... other activities
///             
///             var criticalPath = project.GetCriticalPath();
///             Console.WriteLine("Critical Path Activities:");
///             foreach (var activity in criticalPath)
///             {
///                 Console.WriteLine($"{activity.ActivityId} - {activity.EffortInDays} days - Start: {activity.StartDate} - End: {activity.EndDate}");
///             }
///         }
///     }
/// }
/// ]]>
/// </example>
/// </remarks>
public sealed class ProjectActivityCollection : IEnumerable<ProjectActivity>
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly HashSet<ProjectActivity> _project = new();

    /// <summary>
    /// Models an activity on the project's critical path,
    /// encapsulating both its inherent attributes and temporal characteristics.
    /// </summary>
    public sealed class CriticalPathActivity
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ProjectActivity _innerActivity;

        /// <summary>
        /// Initializes a new instance of the <see cref="CriticalPathActivity"/> class, 
        /// associating it with a base project activity and its start date.
        /// </summary>
        /// <param name="innerActivity">The underlying project activity.</param>
        /// <param name="startDate">The earliest start date of the activity in the project schedule.</param>
        internal CriticalPathActivity(ProjectActivity innerActivity, int startDate)
        {
            StartDate = startDate;
            _innerActivity = innerActivity;
        }

        /// <summary>
        /// Gets the unique identifier that distinguishes this activity within the project scope.
        /// </summary>
        public string ActivityId => _innerActivity.Id;

        /// <summary>
        /// Gets the effort in days required to complete the activity.
        /// </summary>
        public int EffortInDays => _innerActivity.EffortInDays;

        /// <summary>
        /// Gets the start date of the activity.
        /// </summary>
        public int StartDate { get; }

        /// <summary>
        /// Gets the end date of the activity.
        /// </summary>
        public int EndDate => StartDate + _innerActivity.EffortInDays;

        /// <inheritdoc />
        public override int GetHashCode() => _innerActivity.GetHashCode();

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) 
                   || (obj is CriticalPathActivity other && _innerActivity.Equals(other._innerActivity));
        }

        /// <summary>
        /// Implicitly converts a <see cref="CriticalPathActivity"/> object to a <see cref="ProjectActivity"/> object.
        /// </summary>
        /// <param name="cpa">The <see cref="CriticalPathActivity"/> object to convert.</param>
        /// <returns>The <see cref="ProjectActivity"/> object represented by the <see cref="CriticalPathActivity"/> object, or null if the <paramref name="cpa"/> parameter is null.</returns>
        public static implicit operator ProjectActivity?(CriticalPathActivity? cpa) => cpa?._innerActivity;

        internal static IEnumerable<CriticalPathActivity> BuildPath(IEnumerable<ProjectActivity> path)
        {
            var criticalPath = new List<CriticalPathActivity>();
            var startDate = 0;
            foreach (var activity in path)
            {
                criticalPath.Add(new CriticalPathActivity(activity, startDate));
                startDate += activity.EffortInDays;
            }
            return criticalPath;
        }

        internal ProjectActivity InnerActivity => _innerActivity;
    };

    /// <summary>
    /// Retrieves the project's critical path, a sequence of dependent activities with the longest cumulative duration, 
    /// dictating the minimum time required for project completion.
    /// </summary>
    /// <returns>
    /// A linearly ordered, enumerable collection of <see cref="CriticalPathActivity"/> objects that constitute the project's critical path.
    /// </returns>
    /// <example>
    /// Below is an example of how to retrieve the critical path of a project:
    /// <![CDATA[
    /// var criticalPath = project.GetCriticalPath();
    /// Console.WriteLine("Critical Path Activities:");
    /// foreach (var activity in criticalPath)
    /// {
    ///     Console.WriteLine($"{activity.ActivityId} - {activity.EffortInDays} days - Start: {activity.StartDate} - End: {activity.EndDate}");
    /// }
    /// ]]>
    /// </example>
    public IEnumerable<CriticalPathActivity> GetCriticalPath() => _project
        .Select(activity => activity.CriticalPath)
        .OrderByDescending(criticalPath => criticalPath.Sum(a => a.EffortInDays))
        .Take(1)
        .SelectMany(CriticalPathActivity.BuildPath);

    /// <summary>
    /// Returns the critical path for a given <see cref="ProjectActivity"/> object.
    /// </summary>
    /// <param name="activity">The <see cref="ProjectActivity"/> object to get the critical path for.</param>
    /// <returns>
    /// An enumerable collection of <see cref="CriticalPathActivity"/> objects representing the critical path for the specified <see cref="ProjectActivity"/> object.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="activity"/> is null.</exception>
    /// <example>
    /// Below is an example of how to retrieve the critical path for a specific <see cref="ProjectActivity"/> object:
    /// <![CDATA[
    /// var specificActivity = project.Add("Specific Activity", 10);
    /// var criticalPathForActivity = ProjectActivityCollection.GetCriticalPath(specificActivity);
    /// Console.WriteLine("Critical Path Activities for Specific Activity:");
    /// foreach (var activity in criticalPathForActivity)
    /// {
    ///     Console.WriteLine($"{activity.ActivityId} - {activity.EffortInDays} days - Start: {activity.StartDate} - End: {activity.EndDate}");
    /// }
    /// ]]>
    /// </example>
    public static IEnumerable<CriticalPathActivity> GetCriticalPath(ProjectActivity activity)
    {
        if (activity == null) throw new ArgumentNullException(nameof(activity));
        return CriticalPathActivity.BuildPath(activity.CriticalPath);
    }

    /// <summary>
    /// Incorporates a new <see cref="ProjectActivity"/> into the project schedule, 
    /// identifying it by a unique identifier and specifying its effort duration in days.
    /// </summary>
    /// <param name="id">A unique identifier for the activity within the project.</param>
    /// <param name="effortInDays">The intrinsic duration of the activity, expressed in days.</param>
    /// <returns>
    /// A <see cref="ProjectActivity"/> object encapsulating the newly added activity's details.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="id"/> is null or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="effortInDays"/> is less than 0.</exception>
    /// <example>
    /// Below is an example of how to add a new activity to the project:
    /// <![CDATA[
    /// var project = new ProjectActivityCollection();
    /// var projectKickOff = project.Add("Project Kick-off", 5);
    /// Console.WriteLine($"{projectKickOff.Id} - {projectKickOff.EffortInDays} days");
    /// ]]>
    /// </example>
    [DebuggerStepThrough]
    public ProjectActivity Add(string id, int effortInDays) => Add(id, effortInDays, Enumerable.Empty<ProjectActivity>());

    /// <summary>
    /// Adds a new <see cref="ProjectActivity"/> object with the specified identifier, effort in days, and dependency to the collection.
    /// </summary>
    /// <param name="id">The identifier for the activity.</param>
    /// <param name="effortInDays">The effort in days required to complete the activity.</param>
    /// <param name="dependency">The <see cref="ProjectActivity"/> object that the new activity depends on.</param>
    /// <returns>The new <see cref="ProjectActivity"/> object that was added to the collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="id"/> is null or whitespace, or when the <paramref name="dependency"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="effortInDays"/> is less than 0, or if the dependency does not belong to this project.</exception>
    /// <example>
    /// Below is an example of how to add a new activity with a dependency to the project:
    /// <![CDATA[
    /// var project = new ProjectActivityCollection();
    /// var projectKickOff = project.Add("Project Kick-off", 5);
    /// var setupInfrastructure = project.Add("Setup Infrastructure", 10, projectKickOff);
    /// Console.WriteLine($"{setupInfrastructure.Id} - {setupInfrastructure.EffortInDays} days - Depends on: {projectKickOff.Id}");
    /// ]]>
    /// </example>
    [DebuggerStepThrough]
    public ProjectActivity Add(string id, int effortInDays, ProjectActivity dependency) => 
        Add(id, effortInDays, Yield(ThrowIf.ArgumentNull(dependency, nameof(dependency))));

    /// <summary>
    /// Adds a new <see cref="ProjectActivity"/> object with the specified identifier, effort in days, and dependencies to the collection.
    /// </summary>
    /// <param name="id">The identifier for the activity.</param>
    /// <param name="effortInDays">The effort in days required to complete the activity.</param>
    /// <param name="dependencies">The collection of <see cref="ProjectActivity"/> objects that the new activity depends on.</param>
    /// <returns>The new <see cref="ProjectActivity"/> object that was added to the collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="id"/> or <paramref name="dependencies"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when a dependency in the <paramref name="dependencies"/> collection does not belong to this project.</exception>
    /// <example>
    /// Below is an example of how to add a new activity with dependencies to the project:
    /// <![CDATA[
    /// var project = new ProjectActivityCollection();
    /// var projectKickOff = project.Add("Project Kick-off", 5);
    /// var setupAzureResourceGroup = project.Add("Setup Azure Resource Group", 15, projectKickOff);
    /// var setupVNet = project.Add("Setup VNet", 20, setupAzureResourceGroup);
    /// Console.WriteLine($"{setupVNet.Id} - {setupVNet.EffortInDays} days");
    /// foreach (var dependency in setupVNet.Dependencies)
    /// {
    ///     Console.WriteLine($"Dependency: {dependency.Id}");
    /// }
    /// ]]>
    /// </example>
    [DebuggerStepThrough]
    public ProjectActivity Add(string id, int effortInDays, params ProjectActivity[] dependencies) =>
        Add(id, effortInDays, dependencies.AsEnumerable());

    /// <summary>
    /// Adds a new <see cref="ProjectActivity"/> object with the specified identifier, effort in days, and dependencies to the collection.
    /// </summary>
    /// <param name="id">The identifier for the activity.</param>
    /// <param name="effortInDays">The effort in days required to complete the activity.</param>
    /// <param name="dependencies">The collection of <see cref="ProjectActivity"/> objects that the new activity depends on.</param>
    /// <returns>The new <see cref="ProjectActivity"/> object that was added to the collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="id"/> or <paramref name="dependencies"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when a dependency in the <paramref name="dependencies"/> collection does not belong to this project.</exception>
    /// <example>
    /// Below is an example of how to add a new activity with dependencies to the project:
    /// <![CDATA[
    /// var project = new ProjectActivityCollection();
    /// var projectKickOff = project.Add("Project Kick-off", 5);
    /// var setupAzureResourceGroup = project.Add("Setup Azure Resource Group", 15, new[] { projectKickOff });
    /// Console.WriteLine($"{setupAzureResourceGroup.Id} - {setupAzureResourceGroup.EffortInDays} days");
    /// ]]>
    /// </example>
    public ProjectActivity Add(string id, int effortInDays, IEnumerable<ProjectActivity> dependencies)
    {
        var activity = new ProjectActivity(
            ThrowIf.ArgumentNullOrWhiteSpace(id, "Activity id is required", nameof(id)),
            effortInDays
                .ThrowIfArgumentLessThan(0, nameof(effortInDays)),
            ThrowIf.ArgumentNull(dependencies, nameof(dependencies))
                .Do(dependency=>
                {
                    if (_project.Contains(dependency) == false)
                        throw new ArgumentOutOfRangeException($"{dependency.Id} dependency does not belong to this project.",
                            nameof(dependencies));
                })
                .ToList());

        _project.Add(activity);

        return activity;
    }


    /// <inheritdoc />
    public IEnumerator<ProjectActivity> GetEnumerator()
    {
        var sorted =
            from a in _project
            orderby a.CriticalPath.Sum(a => a.EffortInDays)
            select a;
        return sorted.GetEnumerator();
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    /// <summary>
    /// Determines whether a given <see cref="ProjectActivity"/> is part of the project's critical path.
    /// </summary>
    /// <param name="activity">The <see cref="ProjectActivity"/> to check.</param>
    /// <returns>
    /// <c>true</c> if the <see cref="ProjectActivity"/> is part of the critical path; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="activity"/> is null.</exception>
    /// <example>
    /// Below is an example of how to check if a specific activity is part of the project's critical path:
    /// <![CDATA[
    /// var project = new ProjectActivityCollection();
    /// var projectKickOff = project.Add("Project Kick-off", 5);
    /// // ... other activities
    /// 
    /// bool isCritical = project.IsCriticalPathActivity(projectKickOff);
    /// Console.WriteLine($"Is Project Kick-off on the critical path? {isCritical}");
    /// ]]>
    /// </example>
    public bool IsCriticalPathActivity(ProjectActivity activity)
    {
        var path = GetCriticalPath();
        return path.Any(cpa => Equals(cpa.InnerActivity, activity));
    }
}