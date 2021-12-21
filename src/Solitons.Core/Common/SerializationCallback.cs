using System.Diagnostics;
using System.Runtime.Serialization;

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
        protected virtual void OnDeserialization(object sender) => Debug.WriteLine($"{GetType()}.{nameof(OnDeserialization)} called.");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        protected virtual void OnSerialized(object sender) => Debug.WriteLine($"{GetType()}.{nameof(OnSerialized)} called.");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        protected virtual void OnSerializing(object sender) => Debug.WriteLine($"{GetType()}.{nameof(OnSerializing)} called.");

        [DebuggerStepThrough]
        void IDeserializationCallback.OnDeserialization(object sender) => OnDeserialization(sender);

        [DebuggerStepThrough]
        void ISerializationCallback.OnSerialized(object sender) => OnSerialized(sender);

        [DebuggerStepThrough]
        void ISerializationCallback.OnSerializing(object sender) => OnSerializing(sender);
    }
}
