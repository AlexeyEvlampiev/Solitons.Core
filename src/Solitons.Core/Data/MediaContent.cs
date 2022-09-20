using System;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public readonly record struct MediaContent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        public MediaContent(string content, string contentType)
        {
            if ("text/plain".Equals(contentType, StringComparison.OrdinalIgnoreCase))
            {
                Content = content;
            }
            else
            {
                Content = content.ThrowIfNullOrWhiteSpaceArgument(nameof(content));
            }

            ContentType = contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)).Trim();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static MediaContent CreateJson(string jsonString) => new MediaContent(jsonString, "application/json");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static MediaContent CreateXml(string xmlString) => new MediaContent(xmlString, "application/xml");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static MediaContent CreateText(string text) => new MediaContent(text, "text/plain");

        /// <summary>
        /// 
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transformation"></param>
        /// <returns></returns>
        public async Task<MediaContent> TransformAsync(Func<string, Task<string>> transformation)
        {
            var content = await transformation.Invoke(Content);
            return new MediaContent(content, ContentType);
        }
    }
}
