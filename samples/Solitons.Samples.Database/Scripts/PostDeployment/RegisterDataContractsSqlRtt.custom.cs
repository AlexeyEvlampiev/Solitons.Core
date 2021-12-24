using Solitons.Samples.Domain;

namespace Solitons.Samples.Database.Scripts.PostDeployment
{
    public partial class RegisterDataContractsSqlRtt
    {
        sealed record ContentTypeEntry(Guid TypeId, string ContentType, string Schema);
        public RegisterDataContractsSqlRtt()
        {
            var serializer = SampleDomainContext
                .GetOrCreate()
                .GetSerializer();
                
            DataContractTypes = serializer.GetTypes().ToList();

            DataContractEntries = DataContractTypes
                .SelectMany(type=> serializer
                    .GetSupportedContentTypes(type)
                    .Select(ct=> new ContentTypeEntry(type.GUID, ct, null)))
                .ToList();
        }

        public IDomainSerializer Serializer { get; }

        public List<Type> DataContractTypes { get; }

        private List<ContentTypeEntry> DataContractEntries { get; }
    }
}
