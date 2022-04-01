using Solitons.Samples.Domain;

namespace Solitons.Samples.Database.Scripts.PostDeployment
{
    public partial class RegisterDataContractsSqlRtt
    {
        sealed record ContentTypeEntry(Guid TypeId, string ContentType, string Schema);
        public RegisterDataContractsSqlRtt()
        {
            var serializer = SampleDataContractSerializer.Instance;
                
            DataContractTypes = serializer.GetSupportedTypes().ToList();

            DataContractEntries = DataContractTypes
                .SelectMany(type=> serializer
                    .GetSupportedContentTypes(type.GUID)
                    .Select(ct=> new ContentTypeEntry(type.GUID, ct, null)))
                .ToList();
        }

        public SampleDataContractSerializer ContractSerializer { get; }

        public List<Type> DataContractTypes { get; }

        private List<ContentTypeEntry> DataContractEntries { get; }
    }
}
