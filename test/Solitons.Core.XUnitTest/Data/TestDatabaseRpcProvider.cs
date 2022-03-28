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

        [DebuggerNonUserCode]
        protected override IRpcCommand BuildRpcCommand(DbCommandAttribute annotation) 
            => new TestRpcCommand(annotation, _serializer, _callback);

        sealed class TestRpcCommand : RpcCommand
        {
            private readonly IDataContractSerializer _serializer;
            private readonly Callback _callback;

            public TestRpcCommand(
                DbCommandAttribute annotation, 
                IDataContractSerializer serializer,
                Callback callback) : base(annotation, serializer)
            {
                _serializer = serializer;
                _callback = callback;
            }

            protected override async Task<object> InvokeAsync(object request, CancellationToken cancellation)
            {
                var payload = SerializeRequest(request);
                payload = await _callback.Invoke(Annotation, payload, cancellation);
                return DeserializeResponse(payload);
            }

        }

        public static IDatabaseRpcProvider Create(Callback callback, IDataContractSerializer serializer) => new TestDatabaseRpcProvider(callback, serializer);
        private TestDatabaseRpcProvider(Callback callback, IDataContractSerializer serializer)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        

        [DebuggerStepThrough]
        protected override bool CanSerialize(Type type, string contentType) =>
            _serializer.CanSerialize(type, contentType);
    }
}
