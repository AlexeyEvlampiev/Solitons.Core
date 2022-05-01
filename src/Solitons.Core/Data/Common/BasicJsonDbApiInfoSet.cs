

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BasicJsonDbApiInfoSet : IDbApiInfoSet
    {
        sealed record SchemaValidationKey(Guid DataContractId, string ContentType);

        private readonly Dictionary<Guid, DataContractData> _dataContractById;
        private readonly Dictionary<Guid, CommandData> _commandById;
        private readonly Dictionary<SchemaValidationKey, SchemaValidationCallback?> _schemaValidationCallbackById;


        private BasicJsonDbApiInfoSet(InfoSetData data, ISchemaValidationCallbackBuilder builder)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            _dataContractById = data.DataContracts.ToDictionary(dc => dc.Id);
            _commandById = data.Commands.ToDictionary(c => c.Id);
            var validationKeys = data.Commands.SelectMany(c => new[]
            {
                new SchemaValidationKey(c.Request.ContractId, c.Request.ContentType),
                new SchemaValidationKey(c.Response.ContractId, c.Response.ContentType)
            }).ToHashSet();

            _schemaValidationCallbackById = validationKeys.ToDictionary(
                key => key,
                key =>
                {
                    if (_dataContractById.TryGetValue(key.DataContractId, out var contract))
                    {
                        if (contract.Base64SchemaByContentType.TryGetValue(key.ContentType, out var schema))
                        {
                            schema = Encoding.UTF8.GetString(schema.AsBase64Bytes());
                            return builder.Build(key.ContentType, schema);
                        }

                        return null;
                    }

                    throw new InvalidOperationException($"Invalid {data.GetType()} structure.");
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IDbApiInfoSet Parse(string json, SchemaValidationCallbackBuilder builder)
        {
            json = json.ThrowIfNullOrWhiteSpaceArgument(nameof(json));
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            var data = JsonSerializer
                .Deserialize<InfoSetData>(json)
                .ThrowIfNull(() => new FormatException("Invalid InfoSet json"));
            return new BasicJsonDbApiInfoSet(data, builder);
        }

        private CommandData GetCommand(Guid commandId)
        {
            return _commandById.TryGetValue(commandId, out var command)
                ? command
                : throw new KeyNotFoundException($"Command info not found. Command ID: {commandId}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Guid> GetCommandIds() => _dataContractById.Keys;

        public Guid GetRequestContractId(Guid commandId) => GetCommand(commandId).Request.ContractId;

        public Guid GetResponseContractId(Guid commandId) => GetCommand(commandId).Response.ContractId;

        public string GetRequestContentType(Guid commandId) => GetCommand(commandId).Request.ContentType;

        public string GetResponseContentType(Guid commandId) => GetCommand(commandId).Response.ContentType;

        public SchemaValidationCallback? GetSchemaValidationCallback(Guid contractId, string contentType)
        {
            var key = new SchemaValidationKey(contractId, contentType);
            return _schemaValidationCallbackById.TryGetValue(key, out var callback)
                ? callback
                : throw new KeyNotFoundException($"Schema validator not found. Content type: \"{contentType}\", Contract ID: {contractId}");
        }

        public sealed class InfoSetData : BasicJsonDataTransferObject
        {
            [JsonPropertyName("dataContracts")]
            public DataContractData[] DataContracts { get; set; } = Array.Empty<DataContractData>();

            [JsonPropertyName("commands")]
            public CommandData[] Commands { get; set; } = Array.Empty<CommandData>();
        }

        public sealed class DataContractData
        {

            [JsonPropertyName("Id")]
            public Guid Id { get; set; }

            [JsonPropertyName("schemaBase64")]
            public Dictionary<string, string> Base64SchemaByContentType { get; set; } = new ();
        }

        public sealed class CommandData
        {
            [JsonPropertyName("Id")]
            public Guid Id { get; set; }

            [JsonPropertyName("request")]
            public CommandDataContractData Request { get; set; } = new CommandDataContractData();

            [JsonPropertyName("response")]
            public CommandDataContractData Response { get; set; } = new CommandDataContractData();
        }

        public sealed class CommandDataContractData
        {
            [JsonPropertyName("contractId")]
            public Guid ContractId { get; set; }

            [JsonPropertyName("contentType")]
            public string ContentType { get; set; }
        }
    }
}
