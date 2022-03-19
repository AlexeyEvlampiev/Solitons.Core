using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Collections.Specialized
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ProjectActivity
    {
        private readonly List<ProjectActivity> _dependencies;

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
        /// 
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 
        /// </summary>
        public int EffortInDays { get; }


        /// <summary>
        /// Gets the critical path to this activity.
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Id.ToUpper().GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            if(ReferenceEquals(this, obj)) return true;
            if(obj is ProjectActivity other) 
                return Id.Equals(other.Id, StringComparison.OrdinalIgnoreCase);
            return false;
        }
    }
}
