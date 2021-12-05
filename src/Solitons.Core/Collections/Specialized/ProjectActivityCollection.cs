using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Collections.Specialized
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ProjectActivityCollection : IEnumerable<ProjectActivity>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly HashSet<ProjectActivity> _project = new();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectActivity> CriticalPath => _project
                .Select(activity => activity.CriticalPath)
                .OrderByDescending(path => path.Sum(a => a.EffortInDays))
                .Take(1)
                .SelectMany(path => path);

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
            Add(id, effortInDays, dependency
                .ThrowIfNullArgument(nameof(dependency))
                .ToEnumerable());

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
            dependencies
                .ThrowIfNullArgument(nameof(dependencies))
                .Skip(_project.Contains)
                .ForEach(a=> throw new ArgumentOutOfRangeException($"{a.Id} dependency does not belong to this project.", nameof(dependencies)));

            var activity = new ProjectActivity(
                id.ThrowIfNullOrWhiteSpaceArgument(nameof(id)),
                effortInDays.ThrowIfArgumentOutOfRange(1, int.MaxValue, nameof(effortInDays)),
                _project.AsEnumerable(),
                dependencies.ThrowIfNullArgument(nameof(dependencies)));

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
