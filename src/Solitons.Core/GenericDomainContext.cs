using System.Collections.Generic;
using System.Reflection;

namespace Solitons
{
    internal sealed class GenericDomainContext : DomainContext
    {
        public GenericDomainContext(IEnumerable<Assembly> assemblies) 
            : base(assemblies)
        {

        }
    }
}
