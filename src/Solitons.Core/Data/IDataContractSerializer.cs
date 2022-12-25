using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public partial interface IDataContractSerializer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    bool CanSerialize(Type type, string contentType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    bool CanSerialize(Type type, out string? contentType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeId"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    bool CanDeserialize(Guid typeId, string contentType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    bool CanDeserialize(Type type, string contentType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeId"></param>
    /// <returns></returns>
    IEnumerable<string> GetSupportedContentTypes(Guid typeId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    IEnumerable<string> GetSupportedContentTypes(Type type);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    MediaContent Serialize(object obj, string contentType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    MediaContent Serialize(object obj);


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="content"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    T Deserialize<T>(string content, string contentType);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeId"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    object Deserialize(Guid typeId, MediaContent content);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    DataTransferPackage Pack(object dto);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="package"></param>
    /// <returns></returns>
    object Unpack(DataTransferPackage package);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<Type> GetSupportedTypes();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dtoTypeId"></param>
    /// <returns></returns>
    Type GetType(Guid dtoTypeId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dtoTypeId"></param>
    /// <returns></returns>
    Type? GetTypeIfRegistered(Guid dtoTypeId);
}

public partial interface IDataContractSerializer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public string Serialize(object obj, out string contentType)
    {
        var content = Serialize(obj);
        contentType = content.ContentType;
        return content.Content;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeId"></param>
    /// <param name="content"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public object Deserialize(Guid typeId, string content, string contentType) => 
        Deserialize(typeId, new MediaContent(content, contentType));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="content"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public object Deserialize(Type type, string content, string contentType) =>
        Deserialize(type.GUID, new MediaContent(content, contentType));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    object Deserialize(Type type, MediaContent content) => Deserialize(type.GUID, content);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeId"></param>
    /// <param name="input"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    public MediaContent Transform(
        Guid typeId, 
        MediaContent input, 
        string contentType)
    {
        var dto = Deserialize(typeId, input);
        var content = Serialize(dto, contentType);
        return new MediaContent(content, contentType);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mediaContent"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public T Deserialize<T>(MediaContent mediaContent) =>
        (T)Deserialize(typeof(T), mediaContent.Content, mediaContent.ContentType);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static IDataContractSerializer Build(Action<IDataContractSerializerBuilder> config)
    {
        var builder = new DataContractSerializerBuilder(true);
        config?.Invoke(builder);
        return builder.Build();
    }
}
