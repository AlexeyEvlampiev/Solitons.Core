using System.Diagnostics;

namespace Solitons.Samples.Domain
{
    public class SampleDomainContext : DomainContext
    {
        [DebuggerStepThrough]
        public static SampleDomainContext GetOrCreate() => GetOrCreate(() => new SampleDomainContext());

        [DebuggerStepThrough]
        private SampleDomainContext()
            : base(typeof(SampleDomainContext).Assembly)
        {
            
        }
    }
}