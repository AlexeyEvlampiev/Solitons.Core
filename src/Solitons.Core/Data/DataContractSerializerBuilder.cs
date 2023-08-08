using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
sealed class DataContractSerializerBuilder : IDataContractSerializerBuilder
{
    private bool _ignoreMissingCustomGuidAnnotation = false;
    private readonly HashSet<Registration> _cache = new();
    private readonly List<Registration> _registrations = new();
    private readonly HashSet<Type> _dataContractTypes = new();

    [DebuggerNonUserCode]
    sealed record Registration(Type DtoType, IMediaTypeSerializer Serializer);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ignoreMissingCustomGuidAnnotation"></param>
    [DebuggerStepThrough]
    public DataContractSerializerBuilder(bool ignoreMissingCustomGuidAnnotation = false)
    {
        _ignoreMissingCustomGuidAnnotation = ignoreMissingCustomGuidAnnotation;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ignoreMissingCustomGuidAnnotation"></param>
    /// <returns></returns>
    [DebuggerNonUserCode]
    public static DataContractSerializerBuilder Create(bool ignoreMissingCustomGuidAnnotation = false) => new(ignoreMissingCustomGuidAnnotation);


    [DebuggerNonUserCode]
    public IDataContractSerializerBuilder IgnoreMissingCustomGuidAnnotation(bool @switch)
    {
        _ignoreMissingCustomGuidAnnotation = @switch;
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="serializer"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public IDataContractSerializerBuilder Add(Type type, IMediaTypeSerializer serializer)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (serializer == null) throw new ArgumentNullException(nameof(serializer));

        if (!_ignoreMissingCustomGuidAnnotation &&
            false == Attribute.IsDefined(type, typeof(GuidAttribute)))
        {
            throw new InvalidOperationException(new StringBuilder($"{typeof(GuidAttribute)} annotation is missing.")
                .Append($" See type {type}.")
                .ToString());
        }

        var registration = new Registration(type, serializer);
        if (_cache.Add(registration))
        {
            _registrations.Add(registration);
        }

        _dataContractTypes.Add(type);

        return this;
    }

    public IDataContractSerializerBuilder AddAssemblyTypes(
        IEnumerable<Assembly> assemblies, 
        Func<Type, IEnumerable<IMediaTypeSerializer>>? mediaTypeSelector)
    {
        if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
        mediaTypeSelector ??= (type) => Enumerable.Empty<IMediaTypeSerializer>();

        var types = assemblies
            .SkipNulls()
            .Distinct()
            .SelectMany(a => a.GetTypes())
            .Skip(t => _dataContractTypes.Contains(t))
            .ToList();


        types
            .ForEach(type => mediaTypeSelector(type)
                .SkipNulls()
                .Distinct()
                .ForEach(serializer => Add(type, serializer)));

        var registeredContentTypes = _registrations
            .Select(r => KeyValuePair.Create(r.DtoType, r.Serializer.TargetContentType.ToUpper()))
            .ToHashSet();

        types
            .ForEach(type =>
            {
                var jsonTypeRegistration = KeyValuePair.Create(type, IMediaTypeSerializer.BasicJsonSerializer.TargetContentType.ToUpper());
                var xmlTypeRegistration = KeyValuePair.Create(type, IMediaTypeSerializer.BasicXmlSerializer.TargetContentType.ToUpper());

                if (false == registeredContentTypes.Contains(jsonTypeRegistration) && 
                    typeof(BasicJsonDataTransferObject).IsAssignableFrom(type) ||
                    typeof(BasicJsonDataTransferRecord).IsAssignableFrom(type))
                {
                    Add(type, IMediaTypeSerializer.BasicJsonSerializer);
                }
                else if (false == registeredContentTypes.Contains(xmlTypeRegistration) && 
                         typeof(BasicXmlDataTransferObject).IsAssignableFrom(type))
                {
                    Add(type, IMediaTypeSerializer.BasicXmlSerializer);
                }

                if (false == registeredContentTypes.Contains(jsonTypeRegistration) && 
                    typeof(IBasicJsonDataTransferObject).IsAssignableFrom(type))
                {
                    Add(type, IMediaTypeSerializer.BasicJsonSerializer);
                }

                if (false == registeredContentTypes.Contains(xmlTypeRegistration) && 
                    typeof(IBasicXmlDataTransferObject).IsAssignableFrom(type))
                {
                    Add(type, IMediaTypeSerializer.BasicXmlSerializer);
                }
            });

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IDataContractSerializer Build()
    {
        var serializer = new DataContractSerializer();
        var types = new HashSet<Type>();
        foreach (var registration in _registrations)
        {
            types.Add(registration.DtoType);
            serializer.Register(registration.DtoType, registration.Serializer);
        }

        var uniqueTypeGuidViolations = types
            .GroupBy(type => type.GUID)
            .Where(group => group.Count() > 1)
            .Select(group => group.Select(type => type.ToString()).Join())
            .ToList();
        if (uniqueTypeGuidViolations.Any())
        {
            throw new InvalidOperationException(
                new StringBuilder($"{typeof(Type)}.{nameof(Type.GUID)} uniqueness vialation.")
                    .Append($" See types: {uniqueTypeGuidViolations.Join("; ")}")
                    .ToString());
        }

        return serializer;
    }

}