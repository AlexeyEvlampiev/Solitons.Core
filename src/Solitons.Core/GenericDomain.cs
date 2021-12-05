using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Solitons
{
    internal sealed class GenericDomain : Domain
    {
        public GenericDomain(IEnumerable<Assembly> assemblies) 
            : base(assemblies)
        {

        }
    }
}
