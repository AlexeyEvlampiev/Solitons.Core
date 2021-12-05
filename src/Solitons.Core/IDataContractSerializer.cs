using System;
using Solitons.Common;

namespace Solitons
{
    public interface IDataContractSerializer
    {
        public string ContentType { get; }
        string Serialize(object obj);
        object Deserialize(string content, Type targetType);

        public static IDataContractSerializer DefaultJsonSerializer => new BasicJsonDataContractSerializer();
        public static IDataContractSerializer DefaultXmlSerializer => new BasicXmlDataContractSerializer();
    }
}
