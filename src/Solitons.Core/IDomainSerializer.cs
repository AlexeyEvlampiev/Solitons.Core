﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Solitons
{
    /// <summary>
    /// Represents a domain- specific serializer. 
    /// </summary>
    public partial interface IDomainSerializer
    {
        /// <summary>
        /// Determines whether this instance can serialize objects of the specified type, applying the specified content type serialization rules.
        /// </summary>
        /// <param name="type">Target object type</param>
        /// <param name="contentType">Target content type</param>
        /// <returns><c>true</c> if the serialization is supported and <c>false</c> otherwise.</returns>
        bool CanSerialize(Type type, string contentType);

        /// <summary>
        /// Determines whether this instance can serialize objects of the specified type.
        /// Passes back the default content type value via the <paramref name="defaultContentType"/> output parameter.
        /// </summary>
        /// <param name="type">Target object type</param>
        /// <param name="defaultContentType">Default content type</param>
        /// <returns><c>true</c> if the serialization is supported and <c>false</c> otherwise.</returns>
        bool CanSerialize(Type type, out string defaultContentType);

        /// <summary>
        /// Determines whether this instance can deserialize objects of the specified type id (<see cref="Type.GUID"/>), applying the specified content type serialization rules.
        /// </summary>
        /// <param name="typeId">Target object type id (<see cref="Type.GUID"/>)</param>
        /// <param name="contentType">Target content type</param>
        /// <returns><c>true</c> if the serialization is supported and <c>false</c> otherwise.</returns>
        /// <seealso cref="GuidAttribute"/>
        bool CanDeserialize(Guid typeId, string contentType);

        /// <summary>
        /// Gets all content types supported by this serialize for object of the specified type id (<see cref="Type.GUID"/>).
        /// </summary>
        /// <param name="typeId">Target type id (<see cref="Type.GUID"/>)</param>
        /// <returns>Supported content types</returns>
        /// <seealso cref="GuidAttribute"/>
        IEnumerable<string> GetSupportedContentTypes(Guid typeId);

        /// <summary>
        /// Returns a string representation of the specified object, encoded according to the specified content type rules.
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <param name="contentType">Target content type such as application/json</param>
        /// <returns>Objects string representation</returns>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/></exception>
        /// <exception cref="ArgumentException"><paramref name="contentType"/></exception>
        /// <exception cref="NotSupportedException"></exception>
        string Serialize(object obj, string contentType);

        /// <summary>
        /// Returns a string representation of the specified object.
        /// Passes back the applied by default content type via the <paramref name="defaultContentType"/> output parameter.
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <param name="defaultContentType">The applied content type</param>
        /// <returns>Objects string representation</returns>
        /// <exception cref="ArgumentNullException"><paramref name="obj"/></exception>
        /// <exception cref="NotSupportedException"></exception>
        string Serialize(object obj, out string defaultContentType);

        object Deserialize(Guid typeId, string contentType, string content);

        [DebuggerStepThrough]
        public IEnumerable<string> GetSupportedContentTypes(Type dtoType) => 
            GetSupportedContentTypes(dtoType
                .ThrowIfNullArgument(nameof(dtoType))
                .GUID);
    }

    public partial interface IDomainSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="receipt"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanDeserialize(DomainTransientStorageReceipt receipt)
        {
            return receipt is not null &&
                   CanDeserialize(receipt.DtoTypeId, receipt.ContentType);
        }

        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanSerialize(object dto, string contentType) =>
            dto is not null &&
            false == contentType.IsNullOrWhiteSpace() &&
            CanSerialize(dto.GetType(), contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanSerialize(object dto, out string contentType)
        {
            if (dto is null)
            {
                contentType = null;
                return false;
            }

            return CanSerialize(dto.GetType(), out contentType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanSerialize(object dto) => CanSerialize(dto, out var _);

        [DebuggerStepThrough]
        public byte[] Pack(object dto) => Pack(dto, out var _);

        [DebuggerStepThrough]
        public byte[] Pack(object dto, out string contentType)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (CanSerialize(dto, out contentType))
                return Pack(dto, contentType);
            throw new NotSupportedException($"{dto.GetType()}");
        }


        public byte[] Pack(object dto, string contentType)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var base64 = Serialize(dto, contentType)
                .ToBase64(Encoding.UTF8);
            var envelop = new Dictionary<string, string>
            {
                ["Body"] = base64,
                ["ContentType"] = contentType,
                ["Schema"] = dto.GetType().GUID.ToString("N"),
            };
            return JsonSerializer.SerializeToUtf8Bytes(envelop);
        }


        public object Unpack(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            var envelop = JsonSerializer
                .Deserialize<Dictionary<string, string>>(new ReadOnlySpan<byte>(bytes))
                .ThrowIfNull(()=> new ArgumentException("Invalid content", nameof(bytes)));
            if (envelop.TryGetValue("Body", out var base64) &&
                envelop.TryGetValue("ContentType", out var contentType) &&
                envelop.TryGetValue("Schema", out var schema) &&
                Guid.TryParse(schema, out var typeId))
            {
                var content = Convert
                    .FromBase64String(base64)
                    .ToString(Encoding.UTF8);

                return Deserialize(typeId, contentType, content);
            }

            throw new ArgumentException("Invalid content", nameof(bytes));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        /// <seealso cref="DomainContext.GetSerializer"/>
        [DebuggerStepThrough]
        public static IDomainSerializer FromAssemblies(Assembly assembly)
        {
            var genericDomain = new GenericDomainContext(assembly.ToEnumerable());
            return genericDomain.GetSerializer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IDomainSerializer FromAssemblies(params Assembly[] assemblies)
        {
            var genericDomain = new GenericDomainContext(assemblies);
            return genericDomain.GetSerializer();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IDomainSerializer FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            var genericDomain = new GenericDomainContext(assemblies);
            return genericDomain.GetSerializer();
        }
    }
}
