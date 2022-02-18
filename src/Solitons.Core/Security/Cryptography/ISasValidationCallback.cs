namespace Solitons.Security.Cryptography
{
    public interface ISasValidationCallback
    {
        void OnInvalidFormat(string message);
        void OnInvalidSignature();
    }
}
