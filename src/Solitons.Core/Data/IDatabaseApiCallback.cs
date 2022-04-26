namespace Solitons.Data
{
    public interface IDatabaseApiCallback
    {
        void OnResourceNotFound(string message);
        void OnContentTypeNotSupported(string message);

        void OnAsyncExecutionNotSupported();

        void OnInvalidRequest(string message);
        void OnInvalidResponse(string message);
    }
}
