﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Solitons.Collections.FluentEnumerable;

namespace Solitons.Collections.Specialized;

/// <summary>
/// Represents a collection of interdependent <see cref="ProjectActivity"/> items sorted by the item's longest duration variant.
/// </summary>
public sealed class ProjectActivityCollection : IEnumerable<ProjectActivity>
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly HashSet<ProjectActivity> _project = new();

    /// <summary>
    /// Represents a critical path activity.
    /// </summary>
    public sealed class CriticalPathActivity
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ProjectActivity _innerActivity;

        internal CriticalPathActivity(ProjectActivity innerActivity, int startDate)
        {
            StartDate = startDate;
            _innerActivity = innerActivity;
        }

        /// <summary>
        /// Gets the identifier for the activity.
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
   
    };

    /// <summary>
    /// Returns the aggregate critical path for all activities in the collection.
    /// </summary>
    /// <returns>An enumerable collection of <see cref="CriticalPathActivity"/> objects representing the aggregate critical path for all activities in the collection.</returns>
    public IEnumerable<CriticalPathActivity> GetCriticalPath() => _project
        .Select(activity => activity.CriticalPath)
        .OrderByDescending(criticalPath => criticalPath.Sum(a => a.EffortInDays))
        .Take(1)
        .SelectMany(CriticalPathActivity.BuildPath);

    /// <summary>
    /// Returns the critical path for a given <see cref="ProjectActivity"/> object.
    /// </summary>
    /// <param name="activity">The <see cref="ProjectActivity"/> object to get the critical path for.</param>
    /// <returns>An enumerable collection of <see cref="CriticalPathActivity"/> objects representing the critical path for the specified <see cref="ProjectActivity"/> object.</returns>
    public static IEnumerable<CriticalPathActivity> GetCriticalPath(ProjectActivity activity)
    {
        if (activity == null) throw new ArgumentNullException(nameof(activity));
        return CriticalPathActivity.BuildPath(activity.CriticalPath);
    }

    /// <summary>
    /// Adds a new <see cref="ProjectActivity"/> object with the specified identifier and effort in days to the collection.
    /// </summary>
    /// <param name="id">The identifier for the activity.</param>
    /// <param name="effortInDays">The effort in days required to complete the activity.</param>
    /// <returns>The new <see cref="ProjectActivity"/> object that was added to the collection.</returns>
    [DebuggerStepThrough]
    public ProjectActivity Add(string id, int effortInDays) => Add(id, effortInDays, Enumerable.Empty<ProjectActivity>());

    /// <summary>
    /// Adds a new <see cref="ProjectActivity"/> object with the specified identifier, effort in days, and dependency to the collection.
    /// </summary>
    /// <param name="id">The identifier for the activity.</param>
    /// <param name="effortInDays">The effort in days required to complete the activity.</param>
    /// <param name="dependency">The <see cref="ProjectActivity"/> object that the new activity depends on.</param>
    /// <returns>The new <see cref="ProjectActivity"/> object that was added to the collection.</returns>
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
}