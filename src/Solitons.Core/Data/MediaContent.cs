namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed record MediaContent
    {
        private MediaContent(string content, string contentType)
        {
            Content = content.ThrowIfNullOrWhiteSpaceArgument(nameof(content));
            ContentType = contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)).Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static MediaContent Create(string content, string contentType) => new MediaContent(content, contentType);

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
        public string Content { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; }
    }
}
