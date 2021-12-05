

using System.Reflection;

namespace Solitons.Azure
{
    class TestDomain : Domain
    {
        private TestDomain() : base(Assembly.GetExecutingAssembly().ToEnumerable())
        {
        }

        public static readonly Domain Instance = new TestDomain();
    }
}
