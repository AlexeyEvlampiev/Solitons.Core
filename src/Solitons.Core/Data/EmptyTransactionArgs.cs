using System;
using System.Runtime.InteropServices;

namespace Solitons.Data
{
    [Guid("92dddc48-0ccf-4151-ba2f-16dd62cba379")]
    sealed class EmptyTransactionArgs : TransactionArgs
    {
        public EmptyTransactionArgs(Guid commandId) : base(commandId)
        {
        }

        public override string ToString()
        {
            return @$"{{ ""commandId"": {CommandId}}}";
        }
    }
}
