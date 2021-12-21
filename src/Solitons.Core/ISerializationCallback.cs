using System.Runtime.Serialization;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISerializationCallback : IDeserializationCallback
    {
        void OnSerializing(object sender);

        void OnSerialized(object sender);
    }
}
