using System.Diagnostics;

namespace Solitons.Samples.Domain
{
    public class SampleDomainContext : DomainContext
    {
        [DebuggerStepThrough]
        private SampleDomainContext()
            : base(typeof(SampleDomainContext).Assembly)
        {
        }

        [DebuggerStepThrough]
        public static SampleDomainContext GetOrCreate() => GetOrCreate(()=> new SampleDomainContext());
    }
}