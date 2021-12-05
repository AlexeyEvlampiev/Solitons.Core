namespace Solitons.Web
{
    public interface IWebEvent
    {
    }

    public interface IWebEvent<TPayload> : IWebEvent
    {
    }
}
