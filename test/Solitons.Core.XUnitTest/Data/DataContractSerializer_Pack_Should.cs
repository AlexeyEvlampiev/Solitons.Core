using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public sealed class DataContractSerializer_Pack_Should
{
    [Fact]
    public void PackPlainDto()
    {
        var dto = new MyDto() { Text = "This is a test" };
        var serializer = IDataContractSerializer
            .Build(builder => builder
                .IgnoreMissingCustomGuidAnnotation(true)
                .Add(typeof(MyDto), IMediaTypeSerializer.BasicJsonSerializer));

        var expectedTransactionTypeId = Guid.Parse("4b957593-43b3-4c48-be57-fd8b079699b9");
            

        var package = serializer.Pack(dto);

        package.TimeToLive = TimeSpan.FromMinutes(10);

        var clone = (MyDto)serializer.Unpack(package);
        Assert.Equal(dto.Text, clone.Text);

    }

    [Fact]
    public void PackExplicitCommandArgs()
    {
        var dto = new ExplicitTransactionArgs() { Value = 321 };
        var serializer = IDataContractSerializer
            .Build(builder => builder
                .IgnoreMissingCustomGuidAnnotation(true)
                .Add(typeof(ExplicitTransactionArgs), IMediaTypeSerializer.BasicJsonSerializer));
        var expectedCommandId = ((IRemoteTriggerArgs)dto).IntentId;
        var package = serializer.Pack(dto);
        var clone = (ExplicitTransactionArgs)serializer.Unpack(package);
        Assert.Equal(dto.Value, clone.Value);
    }

    [Fact]
    public void PackImplicitCommandArgs()
    {
        var dto = new ImplicitTransactionArgs() { Value = 54321 };
        var serializer = IDataContractSerializer
            .Build(builder => builder
                .IgnoreMissingCustomGuidAnnotation(true)
                .Add(typeof(ImplicitTransactionArgs), IMediaTypeSerializer.BasicJsonSerializer));
        var package = serializer.Pack(dto);
        var clone = (ImplicitTransactionArgs)serializer.Unpack(package);
        Assert.Equal(dto.Value, clone.Value);
    }

    public sealed class MyDto :
        BasicJsonDataTransferObject,
        IRemoteTriggerArgs
    {
        public string Text { get; set; } = String.Empty;
    }

    [Guid("b6fd4e7d-140a-44e1-b692-27ba49e92f6f")]
    public sealed class ExplicitTransactionArgs : BasicJsonDataTransferObject, IRemoteTriggerArgs
    {
        Guid IRemoteTriggerArgs.IntentId => Guid.Parse("07041ff2319d48ada77e62cb4eb086b7");

        public int Value { get; set; }
    }

    public sealed class ImplicitTransactionArgs : BasicXmlDataTransferObject, IRemoteTriggerArgs
    {
        public int Value { get; set; }
    }

}