

using System.Reflection;
using Solitons.Collections;

namespace Solitons.Azure
{
    class TestDomainContext : DomainContext
    {
        private TestDomainContext() : base(FluentEnumerable.Yield(Assembly.GetExecutingAssembly()))
        {
        }

        public static readonly DomainContext Instance = new TestDomainContext();
    }
}
