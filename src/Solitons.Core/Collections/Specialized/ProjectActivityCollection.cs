using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Solitons.Collections.FluentEnumerable;

namespace Solitons.Collections.Specialized
{
    /// <summary>
    ///  A collection of interdependent <see cref="ProjectActivity"/> items sorted by the item's longest duration variant.
    /// <para>Computed properties:</para>
    /// <list type="bullet">
    /// <item>
    /// <see cref="CriticalPath"/>
    /// </item>
    /// </list>
    /// </summary>
    public sealed class ProjectActivityCollection : IEnumerable<ProjectActivity>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly HashSet<ProjectActivity> _project = new();


        /// <summary>
        /// Collections aggregate critical path.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectActivity.CriticalPathActivity> CriticalPath => _project
                .Select(activity => activity.CriticalPath)
                .OrderByDescending(criticalPath => criticalPath.Max(a => a.EndDate))
                .Take(1)
                .SelectMany(path => path)
                .OrderBy(a=> a.StartDate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="effortInDays"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public ProjectActivity Add(string id, int effortInDays) => Add(id, effortInDays, Enumerable.Empty<ProjectActivity>());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="effortInDays"></param>
        /// <param name="dependency"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public ProjectActivity Add(string id, int effortInDays, ProjectActivity dependency) => 
            Add(id, effortInDays, Yield(dependency
                .ThrowIfNullArgument(nameof(dependency))));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="effortInDays"></param>
        /// <param name="dependencies"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public ProjectActivity Add(string id, int effortInDays, params ProjectActivity[] dependencies) =>
            Add(id, effortInDays, dependencies.AsEnumerable());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="effortInDays"></param>
        /// <param name="dependencies"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ProjectActivity Add(string id, int effortInDays, IEnumerable<ProjectActivity> dependencies)
        {
            var activity = new ProjectActivity(
                id
                    .ThrowIfNullOrWhiteSpaceArgument(nameof(id)),
                effortInDays
                    .ThrowIfArgumentLessThan(0, nameof(effortInDays)),
                dependencies
                    .ThrowIfNullArgument(nameof(dependencies))
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ProjectActivity> GetEnumerator()
        {
            var sorted =
                from a in _project
                orderby a.CriticalPath.Sum(a => a.EffortInDays)
                select a;
            return sorted.GetEnumerator();
        }

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
