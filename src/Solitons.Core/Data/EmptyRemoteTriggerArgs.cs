using System;
using System.Runtime.InteropServices;

namespace Solitons.Data;

[Guid("92dddc48-0ccf-4151-ba2f-16dd62cba379")]
sealed class EmptyRemoteTriggerArgs : RemoteTriggerArgs
{
    public EmptyRemoteTriggerArgs(Guid intentId) : base(intentId)
    {
    }

    public override string ToString()
    {
        return @$"{{ ""intentId"": {IntentId}}}";
    }
}