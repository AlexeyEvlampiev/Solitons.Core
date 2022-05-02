using System;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DatabaseApiCommandDataContractInfo : IDatabaseApiCommandDataContractInfo
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SchemaValidationCallback? _schemaValidationCallback;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="contentType"></param>
        /// <param name="infoSet"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal DatabaseApiCommandDataContractInfo(Guid contractId, string contentType, IDbApiInfoSet infoSet)
        {
            ContractId = contractId.ThrowIfEmptyArgument(nameof(contractId));
            ContentType = contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)).Trim();
            if (infoSet == null) throw new ArgumentNullException(nameof(infoSet));
            _schemaValidationCallback = infoSet.ThrowIfNullArgument(nameof(infoSet))
                .GetSchemaValidationCallback(ContractId, ContentType);
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid ContractId { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; }

        public bool? IsValid(string content, out string comment)
        {
            if (_schemaValidationCallback is not null)
            {
                return _schemaValidationCallback.Invoke(content, out comment);
            }

            if (StringComparer.OrdinalIgnoreCase.Equals("text/plain", ContentType))
            {
                comment = "Plain text";
                return true;
            }

            if (content.IsNullOrWhiteSpace())
            {
                comment = "Empty content";
                return null;
            }



            if (StringComparer.OrdinalIgnoreCase.Equals("application/json", ContentType))
            {
                try
                {
                    var jsonDocument = JsonDocument.Parse(content);
                    comment = "Valid JSON";
                    return null;
                }
                catch (Exception e)
                {
                    comment = e.Message;
                    return false;
                }
            }

            if (StringComparer.OrdinalIgnoreCase.Equals("application/xml", ContentType))
            {
                try
                {
                    var xmlDocument = XDocument.Parse(content);
                    comment = "Valid XML";
                    return null;
                }
                catch (Exception e)
                {
                    comment = e.Message;
                    return false;
                }
            }


            comment = "Content could not be validated";
            return null;
        }
    }
}
