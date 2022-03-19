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
    /// </summary>
    public sealed class ProjectActivityCollection : IEnumerable<ProjectActivity>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly HashSet<ProjectActivity> _project = new();

        /// <summary>
        /// 
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
            /// 
            /// </summary>
            public string ActivityId => _innerActivity.Id;

            /// <summary>
            /// 
            /// </summary>
            public int EffortInDays => _innerActivity.EffortInDays;

            /// <summary>
            /// 
            /// </summary>
            public int StartDate { get; }

            /// <summary>
            /// 
            /// </summary>
            public int EndDate => StartDate + _innerActivity.EffortInDays;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode() => _innerActivity.GetHashCode();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object? obj)
            {
                return ReferenceEquals(this, obj) 
                       || (obj is CriticalPathActivity other && _innerActivity.Equals(other._innerActivity));
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="cpa"></param>
            /// <returns></returns>
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
        /// Collections aggregate critical path.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CriticalPathActivity> GetCriticalPath() => _project
            .Select(activity => activity.CriticalPath)
            .OrderByDescending(criticalPath => criticalPath.Sum(a => a.EffortInDays))
            .Take(1)
            .SelectMany(CriticalPathActivity.BuildPath);

        public static IEnumerable<CriticalPathActivity> GetCriticalPath(ProjectActivity activity)
        {
            if (activity == null) throw new ArgumentNullException(nameof(activity));
            return CriticalPathActivity.BuildPath(activity.CriticalPath);
        }

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
