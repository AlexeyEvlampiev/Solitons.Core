

using Solitons.Data;
using Solitons.Data.Common;

namespace Solitons.Samples.Domain
{
    public sealed class SampleDataContractSerializer : DataContractSerializerProxy
    {
        public static readonly SampleDataContractSerializer Instance = new();
        private SampleDataContractSerializer() : base(IDataContractSerializer
            .CreateBuilder()
            .AddAssemblyTypes(typeof(SampleDataContractSerializer).Assembly)
            .Build())
        {
        }
    }
}
