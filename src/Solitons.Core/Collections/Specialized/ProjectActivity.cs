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
        /// 
        /// </summary>
        public ActivitySummary Summary => new(this);

        /// <summary>
        /// Activity critical path.
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

        #region Nested Types

        /// <summary>
        /// 
        /// </summary>
        public sealed record ActivitySummary
        {

            private readonly ProjectActivity _activity;

            internal ActivitySummary(ProjectActivity activity)
            {
                _activity = activity;
                var startDate = activity.CriticalPath
                    .SkipLast(1)
                    .Sum(_ => _.EffortInDays);
                var endDate = startDate + EffortInDays;
            }

            /// <summary>
            /// 
            /// </summary>
            public string ActivityId => _activity.Id;

            /// <summary>
            /// 
            /// </summary>
            public int EffortInDays => _activity.EffortInDays;


            /// <summary>
            /// 
            /// </summary>
            public int StartDate => _activity
                .CriticalPath
                .SkipLast(1)
                .Sum(_ => _.EffortInDays);

            /// <summary>
            /// 
            /// </summary>
            public int EndDate => StartDate + EffortInDays;

            /// <summary>
            /// 
            /// </summary>
            public IEnumerable<ActivitySummary> CriticalPath => _activity
                .CriticalPath
                .Select(a=> new ActivitySummary(a));
        }

        #endregion
    }
}
