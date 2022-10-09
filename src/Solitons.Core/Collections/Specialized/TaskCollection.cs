using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Solitons.Collections.Specialized;

/// <summary>
/// 
/// </summary>
public sealed class TaskCollection : 
    ObservableBase<Unit>, 
    IEnumerable<Task>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly List<Task> _tasks = new();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly object _locker = new();


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="task"></param>
    /// <returns></returns>
    [DebuggerNonUserCode]
    public Task<T> Enlist<T>(Task<T> task)
    {
        lock (_locker)
        {
            _tasks.Add(task);
        }

        return task;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    [DebuggerNonUserCode]
    public Task Enlist(Task task)
    {
        lock (_locker)
        {
            _tasks.Add(task);
        }

        return task;
    }

    [DebuggerNonUserCode]
    IEnumerator<Task> IEnumerable<Task>.GetEnumerator()
    {
        lock (_locker)
        {
            return _tasks
                .ToList()
                .GetEnumerator();
        }
    }

    [DebuggerNonUserCode]
    IEnumerator IEnumerable.GetEnumerator()
    {
        lock (_locker)
        {
            return _tasks
                .ToList()
                .GetEnumerator();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="observer"></param>
    /// <returns></returns>
    protected override IDisposable SubscribeCore(IObserver<Unit> observer)
    {
        var tasks = new List<Task>(this);
        if (tasks.Count == 0)
        {
            observer.OnCompleted();
            return Disposable.Empty;
        }

        return tasks
            .ToObservable()
            .SelectMany(task => task.ToObservable())
            .Subscribe(observer);
    }
}