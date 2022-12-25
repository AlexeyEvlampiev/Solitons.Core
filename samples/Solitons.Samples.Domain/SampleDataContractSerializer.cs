

using Solitons.Data;
using Solitons.Data.Common;

namespace Solitons.Samples.Domain;

public sealed class SampleDataContractSerializer : DataContractSerializerBase
{
    public static readonly SampleDataContractSerializer Instance = new();

    private SampleDataContractSerializer() : base(Config)
    {
    }

    static void Config(IDataContractSerializerBuilder builder)
    {
        builder.AddAssemblyTypes(typeof(SampleDataContractSerializer).Assembly);
    }
}