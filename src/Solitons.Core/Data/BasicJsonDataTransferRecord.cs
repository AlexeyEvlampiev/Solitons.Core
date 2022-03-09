using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public abstract record BasicJsonDataTransferRecord : IBasicJsonDataTransferObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => this.ToJsonString();

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
            return new BasicJsonDataContractSerializer(behaviour,assemblies);
        }
    }
}
