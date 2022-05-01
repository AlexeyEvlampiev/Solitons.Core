using System;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;


namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DatabaseApiCommandInfo
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SchemaValidationCallback? _requestSchemaValidationCallback;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SchemaValidationCallback? _responseSchemaValidationCallback;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="infoSet"></param>
        public DatabaseApiCommandInfo(Guid commandId, IDbApiInfoSet infoSet)
        {
            CommandId = commandId.ThrowIfEmptyArgument(nameof(commandId));
            RequestContractId = infoSet.GetRequestContractId(commandId);
            RequestContentType = infoSet.GetRequestContentType(commandId);
            ResponseContractId = infoSet.GetResponseContractId(commandId);
            ResponseContentType = infoSet.GetResponseContentType(commandId);
            _requestSchemaValidationCallback = infoSet.GetSchemaValidationCallback(RequestContractId, RequestContentType);
            _responseSchemaValidationCallback = infoSet.GetSchemaValidationCallback(ResponseContractId, ResponseContentType);
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid CommandId { get; }

        /// <summary>
        /// 
        /// </summary>
        public Guid RequestContractId { get; }

        /// <summary>
        /// 
        /// </summary>
        public string RequestContentType { get; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ResponseContractId { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ResponseContentType { get; }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool? IsValidRequest(string content, out string comment) 
            => IsValidContent(content, RequestContentType, _requestSchemaValidationCallback, out comment);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool? IsValidResponse(string content, out string comment)
            => IsValidContent(content, ResponseContentType, _responseSchemaValidationCallback, out comment);

        private bool? IsValidContent(string content, string contentType, SchemaValidationCallback? callback, out string comment)
        {
            if (callback is not null)
            {
                return callback.Invoke(content, out comment);
            }

            if (StringComparer.OrdinalIgnoreCase.Equals("text/plain", contentType))
            {
                comment = "Plain text";
                return true;
            }

            if (content.IsNullOrWhiteSpace())
            {
                comment = "Empty content";
                return null;
            }



            if (StringComparer.OrdinalIgnoreCase.Equals("application/json", contentType))
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

            if (StringComparer.OrdinalIgnoreCase.Equals("application/xml", contentType))
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
