using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Solitons.Data
{
    sealed class BasicJsonDataContractSerializer : DataContractSerializer
    {
        public BasicJsonDataContractSerializer(DataContractSerializerBehaviour behaviour, IEnumerable<Assembly> assemblies) 
            : base(behaviour)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            var types = assemblies
                .Distinct()
                .SelectMany(a => a.GetTypes())
                .Skip(t => t.IsAbstract || t.IsInterface);

            foreach (var type in types)
            {
                if (typeof(IBasicJsonDataTransferObject).IsAssignableFrom(type))
                {
                    Register(type, IMediaTypeSerializer.BasicJsonSerializer);
                }
                if (typeof(IBasicXmlDataTransferObject).IsAssignableFrom(type))
                {
                    if (type.GetConstructor(Array.Empty<Type>()) != null)
                    {
                        Register(type, IMediaTypeSerializer.BasicXmlSerializer);
                    }
                }
            }
        }
    }
}
