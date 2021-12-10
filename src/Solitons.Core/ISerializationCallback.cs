using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
