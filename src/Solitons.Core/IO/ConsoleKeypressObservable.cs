using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.IO;

public sealed class ConsoleKeypressObservable : ObservableBase<char>
{
    private readonly HashSet<char> _options;
    private readonly Func<char, char> _transform;

    private ConsoleKeypressObservable(HashSet<char> options, Func<char, char> transform)
    {
        _options = options;
        _transform = transform;
    }

    public static Task<bool> GetYesNoAsync(CancellationToken cancellation = default)
    {
        return FromOptions(char.ToUpper, 'Y', 'N')
            .Select(_ => _ == 'Y')
            .ToTask(cancellation);
    }


    public static Task<bool> GetYesNoAsync(string question, CancellationToken cancellation = default)
    {
        Console.WriteLine(question);
        return FromOptions(char.ToUpper, 'Y', 'N')
            .Select(_ => _ == 'Y')
            .ToTask(cancellation);
    }

    public static IObservable<char> FromOptions(params char[] options) => 
        new ConsoleKeypressObservable(options.ToHashSet(), c => c);

    public static IObservable<char> FromOptions(Func<char, char> transform, params char[] options) => 
        new ConsoleKeypressObservable(options.ToHashSet(), transform)
        .Select(transform);

    protected override IDisposable SubscribeCore(IObserver<char> observer)
    {
        for(;;)
        {
            var key = Console.ReadKey(true);
            var c = _transform.Invoke(key.KeyChar);
            if (_options.Contains(c))
            {
                Console.Write(c);
                observer.OnNext(c);
                observer.OnCompleted();
                break;
            }
        }
        
        return Disposable.Empty;
    }
}