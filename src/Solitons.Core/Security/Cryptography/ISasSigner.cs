namespace Solitons.Security.Cryptography
{
    public interface ISasSigner
    {
        string Sign(string data);
    }
}
