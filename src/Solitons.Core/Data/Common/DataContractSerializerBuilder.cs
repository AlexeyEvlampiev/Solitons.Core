using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    sealed class DataContractSerializerBuilder : IDataContractSerializerBuilder
    {
        private bool _requireCustomGuidAnnotation = false;
        private readonly HashSet<Registration> _cache = new();
        private readonly List<Registration> _registrations = new();
        sealed record Registration(Type DtoType, IMediaTypeSerializer Serializer);

        sealed class Serializer : DataContractSerializer
        {
            public Serializer(IEnumerable<Registration> registrations) 
                : base(DataContractSerializerBehaviour.Default)
            {
                foreach (var registration in registrations)
                {
                    Register(registration.DtoType, registration.Serializer);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requireCustomGuidAnnotation"></param>
        public DataContractSerializerBuilder(bool requireCustomGuidAnnotation)
        {
            _requireCustomGuidAnnotation = requireCustomGuidAnnotation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requireCustomGuidAnnotation"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static DataContractSerializerBuilder Create(bool requireCustomGuidAnnotation = true) => new(requireCustomGuidAnnotation);


        [DebuggerNonUserCode]
        public IDataContractSerializerBuilder RequireCustomGuidAnnotation(bool @switch)
        {
            _requireCustomGuidAnnotation = @switch;
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

            if (_requireCustomGuidAnnotation &&
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

            return this;
        }

        public IDataContractSerializerBuilder AddAssemblyTypes(IEnumerable<Assembly> assemblies, Func<Type, IEnumerable<IMediaTypeSerializer>>? mediaTypeSelector)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            mediaTypeSelector ??= (type) => Enumerable.Empty<IMediaTypeSerializer>();

            var types = assemblies
                .SkipNulls()
                .Distinct()
                .SelectMany(a => a.GetTypes())
                .ToList();

            types.ForEach(type => mediaTypeSelector(type)
                .SkipNulls()
                .Distinct()
                .ForEach(serializer => Add(type, serializer)));

            var registeredContentTypes = _registrations
                .Select(r => KeyValuePair.Create(r.DtoType, r.Serializer.ContentType.ToUpper()))
                .ToHashSet();

            types
                .ForEach(type =>
                {
                    var jsonTypeRegistration = KeyValuePair.Create(type, IMediaTypeSerializer.BasicJsonSerializer.ContentType.ToUpper());
                    var xmlTypeRegistration = KeyValuePair.Create(type, IMediaTypeSerializer.BasicXmlSerializer.ContentType.ToUpper());

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
        public IDataContractSerializer Build() => new Serializer(_registrations);

    }
}
