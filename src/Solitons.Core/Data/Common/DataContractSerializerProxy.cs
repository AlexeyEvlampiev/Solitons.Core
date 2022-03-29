using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataContractSerializerProxy : IDataContractSerializer
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IDataContractSerializer _innerSerializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerSerializer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected DataContractSerializerProxy(IDataContractSerializer innerSerializer)
        {
            _innerSerializer = innerSerializer ?? throw new ArgumentNullException(nameof(innerSerializer));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanSerialize(Type type, string contentType) => _innerSerializer.CanSerialize(type, contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanSerialize(Type type, out string? contentType) => _innerSerializer.CanSerialize(type, out contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanDeserialize(Guid typeId, string contentType) => _innerSerializer.CanDeserialize(typeId, contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanDeserialize(Type type, string contentType) => _innerSerializer.CanDeserialize(type, contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IEnumerable<string> GetSupportedContentTypes(Guid typeId) => _innerSerializer.GetSupportedContentTypes(typeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IEnumerable<string> GetSupportedContentTypes(Type type) => _innerSerializer.GetSupportedContentTypes(type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public string Serialize(object obj, string contentType) => _innerSerializer.Serialize(obj, contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public string Serialize(object obj, out string contentType) => _innerSerializer.Serialize(obj, out contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public object Deserialize(Type targetType, string contentType, string content) => _innerSerializer.Deserialize(targetType, contentType, content);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T Deserialize<T>(string contentType, string content) => _innerSerializer.Deserialize<T>(contentType, content);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public object Deserialize(Guid typeId, string contentType, string content) => _innerSerializer.Deserialize(typeId, contentType, content);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="commandId"></param>
        /// <param name="writer"></param>
        [DebuggerStepThrough]
        public void Pack(object dto, Guid commandId, IDataTransferPackageWriter writer)
        {
            _innerSerializer.Pack(dto, commandId, writer);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="commandId"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public string Pack(object dto, Guid commandId) => _innerSerializer.Pack(dto, commandId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <param name="commandId"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public object Unpack(string package, out Guid commandId) => _innerSerializer.Unpack(package, out commandId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IEnumerable<Type> GetSupportedTypes() => _innerSerializer.GetSupportedTypes();
    }
}
