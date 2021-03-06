using System.Runtime.Serialization;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISerializationCallback : IDeserializationCallback
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        void OnSerializing(object? sender);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        void OnSerialized(object? sender);

    }
}
