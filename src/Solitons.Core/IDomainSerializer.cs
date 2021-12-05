using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IDomainSerializer
    {
        bool CanSerialize(Type type, string contentType);

        bool CanSerialize(Type type, out string contentType);

        bool CanDeserialize(Guid typeId, string contentType);

        IEnumerable<string> GetSupportedContentTypes(Guid typeId);

        string Serialize(object dto, string contentType);

        string Serialize(object dto, out string contentType);

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
        /// <seealso cref="Domain.GetSerializer"/>
        [DebuggerStepThrough]
        public static IDomainSerializer FromAssemblies(Assembly assembly)
        {
            var genericDomain = new GenericDomain(assembly.ToEnumerable());
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
            var genericDomain = new GenericDomain(assemblies);
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
            var genericDomain = new GenericDomain(assemblies);
            return genericDomain.GetSerializer();
        }
    }
}
