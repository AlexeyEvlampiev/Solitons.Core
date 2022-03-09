using System.Diagnostics;
using System.Runtime.Serialization;
using Solitons.Data;

namespace Solitons.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SerializationCallback :
        ISerializationCallback
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        protected virtual void OnDeserialization(object sender) {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        protected virtual void OnSerialized(object sender) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        protected virtual void OnSerializing(object sender) { }

        [DebuggerStepThrough]
        void IDeserializationCallback.OnDeserialization(object sender) => OnDeserialization(sender);

        [DebuggerStepThrough]
        void ISerializationCallback.OnSerialized(object sender) => OnSerialized(sender);

        [DebuggerStepThrough]
        void ISerializationCallback.OnSerializing(object sender) => OnSerializing(sender);
    }
}
