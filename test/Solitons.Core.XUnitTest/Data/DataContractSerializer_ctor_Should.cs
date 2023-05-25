using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public sealed class DataContractSerializer_ctor_Should
{
    [Fact]
    public void RespectGuidAnnotationConfig()
    {
        var serializer = IDataContractSerializer.Build(builder => builder
            .Add(typeof(DtoWithGuidAnnotation))
            .IgnoreMissingCustomGuidAnnotation(true)
            .Add(typeof(DtoWithoutGuidAnnotation)));

        Assert.True(serializer.CanSerialize(typeof(DtoWithGuidAnnotation), out var contentType));
        Assert.EndsWith("application/json", contentType);

        Assert.True(serializer.CanSerialize(typeof(DtoWithoutGuidAnnotation), out contentType));
        Assert.EndsWith("application/json", contentType);

        Assert.Throws<InvalidOperationException>(() => IDataContractSerializer.Build(builder => builder
            .Add(typeof(DtoWithGuidAnnotation))
            .Add(typeof(DtoWithoutGuidAnnotation))));
    }

    [Guid("3cc9972c-63fc-46ff-bbb6-30196c98629a")]
    public sealed class DtoWithGuidAnnotation : BasicJsonDataTransferObject
    {
            
    }


    public sealed class DtoWithoutGuidAnnotation : BasicJsonDataTransferObject
    {

    }
}