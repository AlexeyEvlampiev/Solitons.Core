using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Solitons.Collections;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed record DatabaseRpcCommandMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DtoType"></param>
        /// <param name="ContentType"></param>
        public sealed record Parameter(Type DtoType, string ContentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static DatabaseRpcCommandMetadata Get(Type type) => new(type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<DatabaseRpcCommandMetadata> Get(Assembly assembly) =>
            Get(FluentArray.Create(assembly));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<DatabaseRpcCommandMetadata> Get(params Assembly[] assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            var commandTypesByOid = new Dictionary<Guid, Type>();
            var commandTypesByProcedure = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            return assembly
                .SkipNulls()
                .SelectMany(a => a.GetTypes())
                .Where(type =>
                {
                    if (type.IsAbstract) return false;
                    if (Attribute.IsDefined(type, typeof(DatabaseRpcCommandAttribute)))
                        return true;

                    if (false == typeof(DatabaseRpcCommand).IsAssignableFrom(type)) return false;
                    for (var baseType = type.BaseType;
                         baseType != typeof(object) && baseType != null;
                         baseType = baseType.BaseType ?? typeof(object))
                    {
                        if (baseType.IsGenericType &&
                            baseType.GetGenericTypeDefinition() == typeof(DatabaseRpcCommand<,>))
                        {
                            return true;
                        }
                    }

                    return false;
                })
                .Select(type =>
                {
                    var metadata = Get(type);
                    if (commandTypesByOid.TryGetValue(metadata.CommandOid, out var duplicate))
                    {
                        throw new InvalidOperationException(new StringBuilder($"Detected duplicate {typeof(GuidAttribute)} declaration.")
                            .Append($" {duplicate} and {type} types share same type GUID value of '{metadata.CommandOid}'.")
                            .ToString());
                    }

                    if (commandTypesByProcedure.TryGetValue(metadata.Procedure, out duplicate))
                    {
                        throw new InvalidOperationException(new StringBuilder($"Detected duplicate database procedure declaration.")
                            .Append($" See types {duplicate} and {type}.")
                            .Append($" Procedure: {metadata.Procedure}")
                            .ToString());
                    }

                    commandTypesByOid[metadata.CommandOid] = type;
                    commandTypesByProcedure[metadata.Procedure] = type;
                    return metadata;
                });

        }

        private DatabaseRpcCommandMetadata(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var attribute = (DatabaseRpcCommandAttribute)type.GetCustomAttribute(typeof(DatabaseRpcCommandAttribute))!;
            if (attribute is null)
                throw new ArgumentException(nameof(type), $"{GetType()} required attribute is missing. See type {type}.");
            if (type.IsAbstract)
                throw new ArgumentException(nameof(type), $"{GetType()} may not be applied to abstract classes. See type {type}.");
            if (false == typeof(DatabaseRpcCommand).IsAssignableFrom(type))
                throw new ArgumentException(nameof(type), $"The given type is not a subtype of {typeof(DatabaseRpcCommand<,>)}. See type {type}.");

            Type? genericType = null;
            for (var baseType = type.BaseType;
                 baseType != typeof(object) && baseType != null;
                 baseType = baseType.BaseType ?? typeof(object))
            {
                if (baseType.IsGenericType &&
                    baseType.GetGenericTypeDefinition() == typeof(DatabaseRpcCommand<,>))
                {
                    genericType = baseType;
                    break;
                }
            }

            if (genericType is null)
                throw new ArgumentException(nameof(type), $"The given type is not a subtype of {typeof(DatabaseRpcCommand<,>)}. See type {type}.");
            if (false == Attribute.IsDefined(type, typeof(GuidAttribute)))
                throw new ArgumentException(nameof(type), $"{typeof(GuidAttribute)} required attribute is missing. See type {type}.");

            var genericArgs = genericType.GetGenericArguments();
            var (requestType, responseType) = (genericArgs[0], genericArgs[1]);


            var description = type.GetCustomAttribute<DescriptionAttribute>();

            CommandOid = type.GUID;
            CommandType = type;
            Procedure = attribute.Procedure;
            Request = new Parameter(requestType, attribute.RequestContentType);
            Response = new Parameter(responseType, attribute.ResponseContentType);
            IsolationLevel = attribute.IsolationLevel;
            OperationTimeout = TimeSpan.Parse(attribute.OperationTimeout);
            Description = description?.Description
                .DefaultIfNullOrWhiteSpace($"{requestType}:{attribute.RequestContentType} => {responseType}:{attribute.ResponseContentType}")!;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Procedure { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type CommandType { get; }

        /// <summary>
        /// 
        /// </summary>
        public Guid CommandOid { get; }

        /// <summary>
        /// 
        /// </summary>
        public Parameter Request { get; }

        /// <summary>
        /// 
        /// </summary>
        public Parameter Response { get; }

        /// <summary>
        /// 
        /// </summary>
        public IsolationLevel IsolationLevel { get; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan OperationTimeout { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="onError"></param>
        public void Validate(IDataContractSerializer serializer, Action<string> onError)
        {
            if (false == serializer.CanSerialize(Request.DtoType, Request.ContentType))
            {
                onError.Invoke($"{Request.DtoType} cannot be serilized to '{Request.ContentType}' with {serializer.GetType()}");
            }

            if (false == serializer.CanSerialize(Response.DtoType, Response.ContentType))
            {
                onError.Invoke($"{Response.DtoType} cannot be deserialized from '{Response.ContentType}' with {serializer.GetType()}");
            }
        }
    }
}
