using Solitons.Collections;

namespace Solitons.Web
{
    public interface IUriParser
    {
        bool TryParse(string rawUri, out string resourceUri, out string queryString);

        bool TryParseQueryString(string queryString, out KeyValuePairCollection<string, string> queryParameters);
    }



}
