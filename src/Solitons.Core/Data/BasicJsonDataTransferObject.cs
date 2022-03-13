using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Solitons.Common;

namespace Solitons.Data
{

    /// <summary>
    /// JSON- first Data Transfer Object. When used as a base class, ensures that the overriden <see cref="object.ToString"/> returns the objects json representation.    
    /// </summary>
    /// <seealso cref="IBasicJsonDataTransferObject"/>
    public abstract class BasicJsonDataTransferObject : SerializationCallback, IBasicJsonDataTransferObject, ICloneable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public sealed override string ToString() => this.ToJsonString();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected BasicJsonDataTransferObject Clone() => ((BasicJsonDataTransferObject)JsonSerializer.Deserialize(ToString(), GetType())!)!;

        [DebuggerStepThrough]
        object ICloneable.Clone() => Clone();


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        protected static T Parse<T>(string text) where T : BasicJsonDataTransferObject
        {
            return IBasicJsonDataTransferObject.Parse<T>(text
                .ThrowIfNullOrWhiteSpaceArgument(nameof(text)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="behaviour"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DataContractSerializer BuildSerializer(DataContractSerializerBehaviour behaviour, IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            return new BasicJsonDataContractSerializer(behaviour, assemblies);
        }
    }
}
