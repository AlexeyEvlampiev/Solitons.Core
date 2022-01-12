using System.Diagnostics;
using Solitons.Samples.Domain.Contracts;

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
            RegisterTransactionScriptApi(typeof(IDatabaseApi));
        }


        
    }
}