using System.Diagnostics;

namespace Solitons.Samples.Domain
{
    public class SampleDomainContext : DomainContext
    {
        [DebuggerStepThrough]
        private SampleDomainContext()
            : base(typeof(SampleDomainContext).Assembly)
        {
            ApiVersion = Version.Parse("1.0");

        }

        /// <summary>
        /// Gets the current API version
        /// </summary>
        public Version ApiVersion { get; }

        [DebuggerStepThrough]
        public static SampleDomainContext GetOrCreate() => GetOrCreate(()=> new SampleDomainContext());
    }
}