using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Solitons;

namespace Solitons.Collections.Specialized
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ProjectActivity
    {
        private readonly List<ProjectActivity> _dependencies = new();
        private readonly IEnumerable<ProjectActivity> _project;

        internal ProjectActivity(
            string id, 
            int effortInDays,
            IEnumerable<ProjectActivity> project,
            IEnumerable<ProjectActivity> dependencies)
        {
            Debug.Assert(!id.IsNullOrWhiteSpace());
            Debug.Assert(effortInDays >= 0 && effortInDays <= int.MaxValue);
            Debug.Assert(project != null);
            Debug.Assert(dependencies != null);

            Id = id;
            EffortInDays = effortInDays;
            _project = project;
            _dependencies.AddRange(dependencies);

        }

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 
        /// </summary>
        public int EffortInDays { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Id;

        public override int GetHashCode() => Id.ToUpper().GetHashCode();

        public override bool Equals(object obj)
        {
            if(obj == null) return false;
            if(ReferenceEquals(this, obj)) return true;
            if(obj is ProjectActivity other) 
                return Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
            return false;
        }
    }
}
