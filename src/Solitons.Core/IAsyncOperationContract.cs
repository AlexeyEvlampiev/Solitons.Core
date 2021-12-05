using System.Reactive;

namespace Solitons
{
    public interface IAsyncOperationContract<TPayload>
    {
    }

    public interface IAsyncOperationContract : IAsyncOperationContract<Unit>
    {
    }
}
