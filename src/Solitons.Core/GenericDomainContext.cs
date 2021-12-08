using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Solitons
{
    internal sealed class GenericDomainContext : DomainContext
    {
        [DebuggerNonUserCode]
        public GenericDomainContext(IEnumerable<Assembly> assemblies) 
            : base(assemblies)
        {
        }

        [DebuggerNonUserCode]
        public GenericDomainContext(IEnumerable<Type> types)
            : base(types)
        {

        }
    }
}
