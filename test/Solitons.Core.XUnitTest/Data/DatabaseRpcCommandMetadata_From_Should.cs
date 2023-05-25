using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;
using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public sealed class DatabaseRpcCommandMetadata_From_Should
{
    [Theory]
    [InlineData(typeof(FirstCommand), "10000000-0000-0000-0000-000000000000", typeof(TestRequest), "application/json", typeof(TestResponse), "application/json", "first-procedure", IsolationLevel.ReadCommitted, "00:00:30")]
    [InlineData(typeof(SecondCommand), "20000000-0000-0000-0000-000000000000", typeof(TestRequest), "application/xml", typeof(TestResponse), "application/json", "second-procedure", IsolationLevel.Serializable, "00:00:03")]
    [InlineData(typeof(ThirdCommand), "30000000-0000-0000-0000-000000000000", typeof(TestRequest), "application/xml", typeof(TestResponse), "application/json", "third-procedure", IsolationLevel.Serializable, "00:00:03")]
    public void ScanAllMetadata(
        Type targetCommandType, 
        string expectedOperationOid,
        Type expectedRequestType,
        string expectedRequestContentType,
        Type expectedResponseType,
        string expectedResponseContentType,
        string expectedProcedure,
        IsolationLevel expectedIsolationLevel, 
        string expectedTimeout)
    {
        var metadata = DatabaseRpcCommandMetadata.Get(targetCommandType);

        Assert.Equal(targetCommandType, metadata.CommandType);

        Assert.Equal(Guid.Parse(expectedOperationOid), metadata.CommandOid);
        Assert.Equal(expectedRequestType, metadata.Request.DtoType);
        Assert.Equal(expectedRequestContentType, metadata.Request.ContentType);

        Assert.Equal(expectedResponseType, metadata.Response.DtoType);
        Assert.Equal(expectedResponseContentType, metadata.Response.ContentType);

        Assert.Equal(expectedProcedure, metadata.Procedure);
        Assert.Equal(expectedIsolationLevel, metadata.IsolationLevel);
        Assert.Equal(expectedTimeout, metadata.OperationTimeout.ToString());

        if (targetCommandType == typeof(ThirdCommand))
        {
            Assert.Equal("Some demo description", metadata.Description);
        }
    }

    [Guid("10000000-0000-0000-0000-000000000000")]
    [DatabaseRpcCommand("first-procedure")]
    public sealed class FirstCommand : DatabaseRpcCommand<TestRequest, TestResponse>
    {
        public FirstCommand(IDatabaseRpcProvider client, IDataContractSerializer serializer) : base(client, serializer)
        {
        }
    }


    [Guid("20000000-0000-0000-0000-000000000000")]
    [DatabaseRpcCommand("second-procedure", IsolationLevel = IsolationLevel.Serializable, OperationTimeout = "00:00:03", RequestContentType = "application/xml")]
    public sealed class SecondCommand : DatabaseRpcCommand<TestRequest, TestResponse>
    {
        public SecondCommand(IDatabaseRpcProvider client, IDataContractSerializer serializer) : base(client, serializer)
        {
        }
    }

    [Guid("30000000-0000-0000-0000-000000000000")]
    [DatabaseRpcCommand("third-procedure", IsolationLevel = IsolationLevel.Serializable, OperationTimeout = "00:00:03", RequestContentType = "application/xml")]
    [Description("Some demo description")]
    public sealed class ThirdCommand : DatabaseRpcCommand<TestRequest, TestResponse>
    {
        public ThirdCommand(IDatabaseRpcProvider client, IDataContractSerializer serializer) : base(client, serializer)
        {
        }
    }

    public sealed class TestRequest
    {

    }

    public sealed class TestResponse
    {

    }
}