using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// Represents a readonly record structure for handling text media content, providing mechanisms for 
/// working with various media content types and conversions.
/// </summary>
public readonly record struct TextMediaContent
{
    private  static readonly HashSet<string> BinaryContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "application/octet-stream",
        "application/zip",
        "application/gzip",
        "application/pdf",
        "application/vnd.ms-excel",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-powerpoint",
        "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        "image/jpeg",
        "image/png",
        "image/gif",
        "image/bmp",
        "image/webp",
        "image/svg+xml",
        "image/tiff",
        "image/vnd.microsoft.icon",
        "audio/mpeg",
        "audio/ogg",
        "audio/*",
        "video/mp4",
        "video/mpeg",
        "video/ogg",
        "video/*"
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="TextMediaContent"/> structure using a content string with default content type 'text/plain'.
    /// </summary>
    /// <param name="content">The content string.</param>
    [DebuggerStepThrough]
    public TextMediaContent(string content) 
        : this(content, "text/plain")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextMediaContent"/> structure with the specified content and content type.
    /// </summary>
    /// <param name="content">The media content as a string.</param>
    /// <param name="contentType">The media content type.</param>
    public TextMediaContent(string content, string contentType)
    {
        ContentType = ThrowIf
            .ArgumentNullOrWhiteSpace(contentType, nameof(contentType))
            .Trim();

        Content = "text/plain".Equals(ContentType, StringComparison.OrdinalIgnoreCase) 
            ? content 
            : ThrowIf.ArgumentNullOrWhiteSpace(content, nameof(content));
            
    }


    /// <summary>
    /// Creates a new <see cref="TextMediaContent"/> object with JSON content.
    /// </summary>
    /// <param name="jsonString">The JSON string.</param>
    /// <returns>A new <see cref="TextMediaContent"/> object with "application/json" as content type.</returns>
    public static TextMediaContent CreateJson(string jsonString) => new TextMediaContent(jsonString, "application/json");

    /// <summary>
    /// Creates a new <see cref="TextMediaContent"/> object with XML content.
    /// </summary>
    /// <param name="xmlString">The XML string.</param>
    /// <returns>A new <see cref="TextMediaContent"/> object with "application/xml" as content type.</returns>
    public static TextMediaContent CreateXml(string xmlString) => new TextMediaContent(xmlString, "application/xml");

    /// <summary>
    /// Creates a new <see cref="TextMediaContent"/> object with text content.
    /// </summary>
    /// <param name="text">The text string.</param>
    /// <returns>A new <see cref="TextMediaContent"/> object with "text/plain" as content type.</returns>
    public static TextMediaContent CreateText(string text) => new TextMediaContent(text, "text/plain");

    /// <summary>
    /// Gets the media content as a string.
    /// </summary>
    public string Content { get; }

    /// <summary>
    /// Gets the media content type.
    /// </summary>
    public string ContentType { get; }

    /// <summary>
    /// Creates new <see cref="TextMediaContent"/> structure using the given <paramref name="content"/>
    /// and the instance <see cref="ContentType"/>
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    [DebuggerNonUserCode]
    public TextMediaContent WithContent(string content) => new TextMediaContent(content, ContentType);

    /// <summary>
    /// Implicitly converts a <see cref="TextMediaContent"/> object to a string.
    /// </summary>
    /// <param name="content">The <see cref="TextMediaContent"/> object.</param>
    /// <returns>The content as a string.</returns>
    public static implicit operator string(TextMediaContent content) => content.Content;

    /// <summary>
    /// Converts the current TextMediaContent to a string.
    /// </summary>
    /// <returns>The content string of the current TextMediaContent.</returns>
    public override string ToString() => Content;

    /// <summary>
    /// Converts the value of the current TextMediaContent object to an HttpContent.
    /// </summary>
    /// <returns>A new instance of HttpContent that represents the current TextMediaContent.</returns>
    [DebuggerNonUserCode]
    public HttpContent ToHttpContent() => new StringContent(Content, Encoding.UTF8, ContentType);

    /// <summary>
    /// Gets the media content type as a MediaTypeHeaderValue object.
    /// </summary>
    /// <returns>The MediaTypeHeaderValue object representing the media content type, or null if the content type is invalid.</returns>
    public MediaTypeHeaderValue? GetMediaTypeHeaderValue()
    {
        if (MediaTypeHeaderValue.TryParse(this.ContentType, out var value))
        {
            return value;
        }

        return null;
    }

    /// <summary>
    /// Creates a new instance of TextMediaContent from an HttpContent.
    /// </summary>
    /// <param name="httpContent">The HttpContent to be converted.</param>
    /// <returns>A new instance of TextMediaContent that represents the HttpContent.</returns>
    [DebuggerNonUserCode]
    public static async Task<TextMediaContent> FromHttpContent(HttpContent httpContent)
    {
        if (httpContent == null)
            throw new ArgumentNullException(nameof(httpContent));

        var contentType = (httpContent.Headers.ContentType?.MediaType)
            .DefaultIfNullOrWhiteSpace("text/plain");

        try
        {
            if (BinaryContentTypes.Contains(contentType))
            {
                var bytes = await httpContent.ReadAsByteArrayAsync();
                var base64String = Convert.ToBase64String(bytes);
                return new TextMediaContent(base64String, contentType);
            }
            else
            {
                var encoding = Encoding.UTF8;
                if (httpContent.Headers.ContentType?.CharSet != null)
                {
                    try
                    {
                        encoding = Encoding.GetEncoding(httpContent.Headers.ContentType.CharSet);
                    }
                    catch (ArgumentException ex)
                    {
                        // If the charset is not recognized, fall back to UTF-8
                        Debug.WriteLine(ex.Message);
                    }
                }

                var bytes = await httpContent.ReadAsByteArrayAsync();
                var content = encoding.GetString(bytes);
                return new TextMediaContent(content, contentType);
            }
        }
        catch (IOException ex)
        {
            throw new ArgumentException("An error occurred while reading the HttpContent", ex);
        }
    }

    /// <summary>
    /// Asynchronously loads a JSON file from the specified file path and creates a new <see cref="TextMediaContent"/> object with the file's content.
    /// </summary>
    /// <param name="filePath">The path to the JSON file.</param>
    /// <param name="cancellation">The optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new <see cref="TextMediaContent"/> object with the JSON content and "application/json" as content type.</returns>
    public static async Task<TextMediaContent> LoadJsonFromFileAsync(string filePath, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        using StreamReader reader = File.OpenText(filePath);
        string content = await reader.ReadToEndAsync();
        return new TextMediaContent(content, "application/json");
    }

    /// <summary>
    /// Asynchronously loads a text file from the specified file path and creates a new <see cref="TextMediaContent"/> object with the file's content.
    /// </summary>
    /// <param name="filePath">The path to the text file.</param>
    /// <param name="cancellation">The optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new <see cref="TextMediaContent"/> object with the text content and "text/plain" as content type.</returns>
    public static async Task<TextMediaContent> LoadTextFromFileAsync(string filePath, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        using StreamReader reader = File.OpenText(filePath);
        string content = await reader.ReadToEndAsync();
        return new TextMediaContent(content, "text/plain");
    }

    /// <summary>
    /// Asynchronously loads an XML file from the specified file path and creates a new <see cref="TextMediaContent"/> object with the file's content.
    /// </summary>
    /// <param name="filePath">The path to the XML file.</param>
    /// <param name="cancellation">The optional cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a new <see cref="TextMediaContent"/> object with the XML content and "application/xml" as content type.</returns>
    public static async Task<TextMediaContent> LoadXmlFromFileAsync(string filePath, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        using StreamReader reader = File.OpenText(filePath);
        string content = await reader.ReadToEndAsync();
        return new TextMediaContent(content, "application/xml");
    }



    /// <summary>
    /// Implicitly converts a <see cref="TextMediaContent"/> object to an <see cref="HttpContent"/> object.
    /// </summary>
    /// <param name="content">The <see cref="TextMediaContent"/> object.</param>
    /// <returns>An <see cref="HttpContent"/> object representing the <see cref="TextMediaContent"/>.</returns>
    public static implicit operator HttpContent(TextMediaContent content) => content.ToHttpContent();

    /// <summary>
    /// Implicitly converts a <see cref="TextMediaContent"/> object to a <see cref="MediaTypeHeaderValue"/> object.
    /// </summary>
    /// <param name="content">The <see cref="TextMediaContent"/> object.</param>
    /// <returns>The <see cref="MediaTypeHeaderValue"/> object representing the media content type, or null if the content type is invalid.</returns>
    public static implicit operator MediaTypeHeaderValue?(TextMediaContent content)
    {
        if (MediaTypeHeaderValue.TryParse(content.ContentType, out var value))
        {
            return value;
        }

        return null;
    }
}