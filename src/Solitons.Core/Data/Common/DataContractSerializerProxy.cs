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
        public object Deserialize(Type targetType, string content, string contentType) => _innerSerializer.Deserialize(targetType, content, contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T Deserialize<T>(string content, string contentType) => _innerSerializer.Deserialize<T>(content, contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public object Deserialize(Guid typeId, string content, string contentType) => _innerSerializer.Deserialize(typeId, content, contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public DataTransferPackage Pack(object dto)
        {
            return _innerSerializer.Pack(dto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public object Unpack(DataTransferPackage package)
        {
            return _innerSerializer.Unpack(package);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public string Pack(IDistributedEventArgs args) => _innerSerializer.Pack(args);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IEnumerable<Type> GetSupportedTypes() => _innerSerializer.GetSupportedTypes();

        public Type GetType(Guid dtoTypeId)
        {
            return _innerSerializer.GetType(dtoTypeId);
        }

        public Type? GetTypeIfExists(Guid dtoTypeId)
        {
            return _innerSerializer.GetTypeIfExists(dtoTypeId);
        }
    }
}
