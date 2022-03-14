using System;
using System.Diagnostics;

namespace Solitons.Data
{
    sealed class TestDataContractSerializer : DataContractSerializer
    {

        [DebuggerNonUserCode]
        public static DataContractSerializer Create(DataContractSerializerBehaviour behaviour) =>
            new TestDataContractSerializer(behaviour);

        [DebuggerNonUserCode]
        private TestDataContractSerializer(DataContractSerializerBehaviour behaviour) : base(behaviour)
        {
        }


        [DebuggerStepThrough]
        public new TestDataContractSerializer Register(Type type, IMediaTypeSerializer serializer)
        {
            base.Register(type, serializer);
            return this;
        }
    }
}
