using System;
using System.Diagnostics;
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
            return ThrowIf
                .NullOrWhiteSpaceArgument(text, nameof(text))
                .Convert(IBasicJsonDataTransferObject.Parse<T>);
        }
    }
}
