using System;
using System.Net.Http;

namespace Solitons.Web
{
    public interface IWebQueryConverter
    {
        object ToDataTransferObject(IWebRequest request);

        HttpRequestMessage ToHttpRequestMessage(object dto, HttpMethod method, Version apiVersion);

    }
}
