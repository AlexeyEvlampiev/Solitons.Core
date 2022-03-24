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
    string Serialize(object obj, string contentType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    string Serialize(object obj, out string contentType);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetType"></param>
    /// <param name="contentType"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    object Deserialize(Type targetType, string contentType, string content);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="contentType"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    T Deserialize<T>(string contentType, string content);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeId"></param>
    /// <param name="contentType"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    object Deserialize(Guid typeId, string contentType, string content);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="writer"></param>
    void Pack(object dto, IDataTransferPackageWriter writer);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    string Pack(object dto);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="package"></param>
    /// <returns></returns>
    object Unpack(string package);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<Type> GetSupportedTypes();
}

public partial interface IDataContractSerializer
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [DebuggerNonUserCode]
    public static IDataContractSerializerBuilder CreateBuilder() => new DataContractSerializerBuilder(true);
}
