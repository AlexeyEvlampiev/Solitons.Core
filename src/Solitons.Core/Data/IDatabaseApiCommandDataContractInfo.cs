using System;

namespace Solitons.Data
{
    public interface IDatabaseApiCommandDataContractInfo
    {
        Guid ContractId { get; }

        string ContentType { get; }

        bool? IsValid(string content, out string comment);
    }
}
