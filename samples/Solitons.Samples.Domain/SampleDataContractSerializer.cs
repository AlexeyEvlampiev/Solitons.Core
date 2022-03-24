

using Solitons.Data;
using Solitons.Data.Common;

namespace Solitons.Samples.Domain
{
    public sealed class SampleDataContractSerializer : DataContractSerializerProxy
    {
        public SampleDataContractSerializer() : base(IDataContractSerializer
            .CreateBuilder()
            .AddAssemblyTypes(typeof(SampleDataContractSerializer).Assembly)
            .Build())
        {

        }
    }
}
