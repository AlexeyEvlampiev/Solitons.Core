using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Solitons.Data;

/// <summary>
/// Represents a builder for configuring and constructing an <see cref="IDataContractSerializer"/> instance.
/// </summary>
public partial interface IDataContractSerializerBuilder
{
    /// <summary>
    /// Specifies whether to ignore missing custom GUID annotations during serialization and deserialization.
    /// </summary>
    /// <param name="switch">A flag indicating whether to ignore missing custom GUID annotations. True to ignore, false otherwise.</param>
    /// <returns>The current instance of <see cref="IDataContractSerializerBuilder"/>.</returns>
    IDataContractSerializerBuilder IgnoreMissingCustomGuidAnnotation(bool @switch);

    /// <summary>
    /// Adds a custom data transfer object (DTO) type and its corresponding <see cref="IMediaTypeSerializer"/> implementation to the serializer.
    /// </summary>
    /// <param name="dtoType">The type of the DTO to add.</param>
    /// <param name="mediaTypeSerializer">The <see cref="IMediaTypeSerializer"/> implementation that corresponds to the DTO type.</param>
    /// <returns>The current instance of <see cref="IDataContractSerializerBuilder"/>.</returns>
    IDataContractSerializerBuilder Add(Type dtoType, IMediaTypeSerializer mediaTypeSerializer);

    /// <summary>
    /// Adds custom data transfer object (DTO) types and their corresponding <see cref="IMediaTypeSerializer"/> implementations to the serializer, based on types found in a collection of assemblies.
    /// </summary>
    /// <param name="assemblies">The collection of assemblies to scan for DTO types and their corresponding <see cref="IMediaTypeSerializer"/> implementations.</param>
    /// <param name="mediaTypeSelector">Optional. A function to select the <see cref="IMediaTypeSerializer"/> implementation for each DTO type.</param>
    /// <returns>The current instance of <see cref="IDataContractSerializerBuilder"/>.</returns>
    IDataContractSerializerBuilder AddAssemblyTypes(IEnumerable<Assembly> assemblies, Func<Type, IEnumerable<IMediaTypeSerializer>>? mediaTypeSelector = null);

    /// <summary>
    /// Builds and returns an instance of <see cref="IDataContractSerializer"/> based on the current configuration of the builder.
    /// </summary>
    /// <returns>An instance of <see cref="IDataContractSerializer"/> based on the current configuration of the builder.</returns>
    IDataContractSerializer Build();

        
}

public partial interface IDataContractSerializerBuilder
{
    /// <summary>
    /// Adds the specified DTO type with the given media type serializers.
    /// </summary>
    /// <param name="dtoType">The type of the DTO to add.</param>
    /// <param name="mediaTypeSerializers">The collection of media type serializers for the specified DTO.</param>
    /// <returns>The current instance of the <see cref="IDataContractSerializerBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="dtoType"/> or <paramref name="mediaTypeSerializers"/> are null.</exception>
    [DebuggerStepThrough]
    IDataContractSerializerBuilder Add(Type dtoType, params IMediaTypeSerializer[] mediaTypeSerializers)
    {
        if (dtoType == null) throw new ArgumentNullException(nameof(dtoType));
        if (mediaTypeSerializers == null) throw new ArgumentNullException(nameof(mediaTypeSerializers));
        if (mediaTypeSerializers.Length == 0)
        {
            var implicitSerializers = new List<IMediaTypeSerializer>();

            if (typeof(BasicJsonDataTransferObject).IsAssignableFrom(dtoType) ||
                typeof(BasicJsonDataTransferRecord).IsAssignableFrom(dtoType))
            {
                implicitSerializers.Add(new BasicJsonMediaTypeSerializer());
            }
               
            if (typeof(BasicXmlDataTransferObject).IsAssignableFrom(dtoType))
            {
                implicitSerializers.Add(new BasicXmlMediaTypeSerializer());
            }

            if (typeof(IBasicJsonDataTransferObject).IsAssignableFrom(dtoType) &&
                implicitSerializers.OfType<BasicJsonMediaTypeSerializer>().Any() == false)
            {
                implicitSerializers.Add(new BasicJsonMediaTypeSerializer());
            }

            if (typeof(IBasicXmlDataTransferObject).IsAssignableFrom(dtoType) &&
                implicitSerializers.OfType<BasicXmlMediaTypeSerializer>().Any() == false)
            {
                implicitSerializers.Add(new BasicXmlMediaTypeSerializer());
            }

            if (implicitSerializers.Any())
            {
                mediaTypeSerializers = implicitSerializers.ToArray();
            }
        }

        if (mediaTypeSerializers.Length == 0)
        {
            throw new ArgumentException(new StringBuilder($"Media type serializers collection is required.")
                .Append($" Implicit media type serializer for {dtoType} could not be determined.")
                .ToString(), nameof(mediaTypeSerializers));
        }
        Array.ForEach(mediaTypeSerializers, mts=> Add(dtoType, mts));
        return this;
    }

    /// <summary>
    /// Adds the types defined in the specified assemblies with their associated media type serializers.
    /// </summary>
    /// <param name="assembly">The collection of assemblies containing types to add.</param>
    /// <param name="mediaTypeSelector">The optional function to determine the media type serializers for each type.</param>
    /// <returns>The current instance of the <see cref="IDataContractSerializerBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="assembly"/> are null.</exception>
    [DebuggerStepThrough]
    public IDataContractSerializerBuilder AddAssemblyTypes(Assembly assembly, Func<Type, IEnumerable<IMediaTypeSerializer>>? mediaTypeSelector = null)
    {
        if (assembly == null) throw new ArgumentNullException(nameof(assembly));
        return AddAssemblyTypes(new Assembly[] { assembly }, mediaTypeSelector);
    }

}