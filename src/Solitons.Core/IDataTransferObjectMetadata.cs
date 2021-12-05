using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Solitons
{
    public interface IDataTransferObjectMetadata
    {
        Type SerializerType { get; }

        bool IsDefault { get; }

        public static IEnumerable<Type> Discover(IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .ThrowIfNullArgument(nameof(assemblies))
                .SelectMany(a => a.GetTypes())
                .Where(type => type.GetCustomAttributes().OfType<IDataTransferObjectMetadata>().Any())
                .Do(type =>
                {
                    var guidAtt = type.GetCustomAttribute<GuidAttribute>();
                    if (guidAtt is null)
                        throw new InvalidOperationException($"{typeof(GuidAttribute)} is missing. See type {type}");
                });
        }

        public static bool IsMatch(Type type) => type.GetCustomAttributes().OfType<IDataTransferObjectMetadata>().Any();
    }
}
