using System.Threading.Tasks;
using Moq;
using Solitons.Diagnostics;
using Solitons.Diagnostics.Common;

namespace Solitons;

public sealed class FakeLogger : AsyncLogger
{
    public FakeLogger()
    {
        Mock
            .Setup(_ => _.LogAsync(It.IsAny<LogEventArgs>()))
            .Returns(Task.CompletedTask);
    }
    protected override Task LogAsync(LogEventArgs args)
    {
        return Mock.Object.LogAsync(args);
    }

    public Mock<ICallback> Mock { get; } = new Mock<ICallback>();

    public interface ICallback
    {
        Task LogAsync(LogEventArgs args);
    }

}