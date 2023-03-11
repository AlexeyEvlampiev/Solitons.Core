using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        public static readonly IEqualityComparer<string> ContentTypeComparer = MediaContentTypeEqualityComparer.Instance;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        [DebuggerStepThrough]
        public MediaContent(string content) 
            : this(content, "text/plain")
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        public MediaContent(string content, string contentType)
        {
            ContentType = ThrowIf
                .ArgumentNullOrWhiteSpace(contentType, nameof(contentType))
                .Trim();

            Content = "text/plain".Equals(ContentType, StringComparison.OrdinalIgnoreCase) 
                ? content 
                : ThrowIf.ArgumentNullOrWhiteSpace(content, nameof(content));
            
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
        /// Creates new <see cref="MediaContent"/> structure using the given <paramref name="content"/>
        /// and the instance <see cref="ContentType"/>
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public MediaContent WithContent(string content) => new MediaContent(content, ContentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public static implicit operator string(MediaContent content) => content.Content;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Content;
    }
}
