

using System.Reflection;

namespace Solitons.Azure
{
    class TestDomainContext : DomainContext
    {
        private TestDomainContext() : base(Assembly.GetExecutingAssembly().ToEnumerable())
        {
        }

        public static readonly DomainContext Instance = new TestDomainContext();
    }
}
