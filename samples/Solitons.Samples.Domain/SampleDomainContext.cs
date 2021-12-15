using System.Diagnostics;

namespace Solitons.Samples.Domain
{
    public class SampleDomainContext : DomainContext
    {
        private static readonly WeakReference<SampleDomainContext> Singleton = new WeakReference<SampleDomainContext>(new SampleDomainContext());

        [DebuggerStepThrough]
        private SampleDomainContext() 
            : base(typeof(SampleDomainContext).Assembly)
        {
        }

        [DebuggerStepThrough]
        public static SampleDomainContext GetOrCreate()
        {
            if(Singleton.TryGetTarget(out var context))
                return context;
            var instance = new SampleDomainContext();
            Singleton.SetTarget(instance);
            return instance; ;
        }
    }
}