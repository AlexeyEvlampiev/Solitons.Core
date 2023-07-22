using System;
using System.Diagnostics;
using System.Threading;

namespace Solitons;

public sealed class GuidGenerator
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Func<Guid> _generateNext;

    private GuidGenerator(Func<Guid> generateNext)
    {
        _generateNext = generateNext;
    }

    public static GuidGenerator Create() => new GuidGenerator(Guid.NewGuid);

    [DebuggerStepThrough]
    public static GuidGenerator CreateInt32SerialGenerator(bool useLittleEndian = true)
    {
        var seed = Guid.NewGuid();
        int serial = 0;
        return new GuidGenerator(GenerateNext);

        Guid GenerateNext()
        {
            var id = Interlocked.Increment(ref serial);
            return seed.ReplaceLast32Bits(id, useLittleEndian);
        }
    }

    [DebuggerStepThrough]
    public Guid NewGuid() => _generateNext();

    public Func<string> AsStringFactory(string guidFormat = "N")
    {
        return () => this.NewGuid().ToString(guidFormat);
    }
}