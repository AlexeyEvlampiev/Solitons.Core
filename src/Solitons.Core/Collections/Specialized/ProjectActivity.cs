using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Collections.Specialized;

/// <summary>
/// Represents a project activity.
/// </summary>
public sealed class ProjectActivity
{
    private readonly List<ProjectActivity> _dependencies;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectActivity"/> class.
    /// </summary>
    /// <param name="id">The identifier for the activity.</param>
    /// <param name="effortInDays">The effort in days required to complete the activity.</param>
    /// <param name="dependencies">The list of dependencies for the activity.</param>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="id"/> parameter is null or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="effortInDays"/> parameter is negative.</exception>
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

    /// <summary>
    /// Gets the identifier for the activity.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the effort in days required to complete the activity.
    /// </summary>
    public int EffortInDays { get; }


    /// <summary>
    /// Gets the critical path to this activity.
    /// </summary>
    /// <returns>A stack of project activities representing the critical path to this activity.</returns>
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
    /// Returns the identifier for the activity.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
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