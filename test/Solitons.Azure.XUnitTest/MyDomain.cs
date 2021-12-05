using System;
using System.Reflection;

namespace Solitons.Azure
{
    class MyDomain : Domain
    {
        private static readonly Lazy<MyDomain> _lazyInstance = new Lazy<MyDomain>(() => new MyDomain());

        public static Domain Instance => _lazyInstance.Value;

        private MyDomain() 
            : base(Assembly.GetExecutingAssembly().ToEnumerable())
        {
        }
    }
}
