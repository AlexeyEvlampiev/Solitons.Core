using System;
using System.Reflection;
using Solitons.Collections;

namespace Solitons.Azure
{
    class MyDomainContext : DomainContext
    {
        private static readonly Lazy<MyDomainContext> _lazyInstance = new(() => new MyDomainContext());

        public static DomainContext Instance => _lazyInstance.Value;

        private MyDomainContext() 
            : base(FluentEnumerable.Yield(Assembly.GetExecutingAssembly()))
        {
        }
    }
}
