using System;

namespace Solitons
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]

    public sealed class ResponseTypeAttribute : Attribute
    {
        public ResponseTypeAttribute(Type contractType)
        {
            ContractType = contractType;
        }

        public Type ContractType { get; }
    }
}
