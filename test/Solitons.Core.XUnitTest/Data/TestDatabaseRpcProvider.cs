using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Data.Common;

namespace Solitons.Data
{
    sealed class TestDatabaseRpcProvider : DatabaseRpcProvider
    {
        private readonly Callback _callback;
        private readonly IDataContractSerializer _serializer;

        public delegate Task<string> Callback(DbCommandAttribute annotation, string payload, CancellationToken cancellation);


        public static IDatabaseRpcProvider Create(Callback callback, IDataContractSerializer serializer) => new TestDatabaseRpcProvider(callback, serializer);
        private TestDatabaseRpcProvider(Callback callback, IDataContractSerializer serializer)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        [DebuggerStepThrough]
        protected override Task<string> InvokeAsync(DbCommandAttribute annotation, string payload,
            CancellationToken cancellation) =>
            _callback.Invoke(annotation, payload, cancellation);

        [DebuggerStepThrough]
        protected override bool CanSerialize(Type type, string contentType) =>
            _serializer.CanSerialize(type, contentType);


        [DebuggerStepThrough]
        protected override string Serialize(object request, string contentType) =>
            _serializer.Serialize(request, contentType);

        [DebuggerStepThrough]
        protected override object Deserialize(string content, string contentType, Type type)
            => _serializer.Deserialize(type, contentType, content);
    }
}
