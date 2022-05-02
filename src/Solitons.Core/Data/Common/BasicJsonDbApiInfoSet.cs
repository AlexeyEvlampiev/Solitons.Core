

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
            ETag = data.ETag;
            _dataContractById = data.DataContracts;
            _commandById = data.Commands;
            var validationKeys = data.Commands.Values.SelectMany(c => new[]
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

        public string ETag { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Guid> GetCommandIds() => _commandById.Keys;

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

        /// <summary>
        /// 
        /// </summary>
        public sealed class InfoSetData : BasicJsonDataTransferObject
        {
            /// <summary>
            /// 
            /// </summary>
            [JsonPropertyName("dataContracts")] public Dictionary<Guid, DataContractData> DataContracts { get; set; } = new();

            /// <summary>
            /// 
            /// </summary>
            [JsonPropertyName("commands")] public Dictionary<Guid, CommandData> Commands { get; set; } = new();

            /// <summary>
            /// 
            /// </summary>
            [JsonPropertyName("eTag")] public string ETag { get; set; } = String.Empty;
        }

        public sealed class DataContractData
        {

            [JsonPropertyName("Id")]
            public string Id { get; set; }

            [JsonPropertyName("contentTypes")]
            public Dictionary<string, string> Base64SchemaByContentType { get; set; } = new ();
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class CommandData
        {
            /// <summary>
            /// 
            /// </summary>
            [JsonPropertyName("procedure")] public string Procedure { get; set; } = String.Empty;

            /// <summary>
            /// 
            /// </summary>
            [JsonPropertyName("scheduable")] public bool Scheduable { get; set; }

            [JsonPropertyName("request")] public CommandDataContractData Request { get; set; } = new();

            [JsonPropertyName("response")] public CommandDataContractData Response { get; set; } = new();
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class CommandDataContractData
        {
            /// <summary>
            /// 
            /// </summary>
            [JsonPropertyName("oid")]
            public Guid ContractId { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonPropertyName("contentType")]
            public string ContentType { get; set; }
        }
    }
}
