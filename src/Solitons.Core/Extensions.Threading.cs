using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons;

public static partial class Extensions
{
    /// <summary>
    /// Executes a given <see cref="Action"/> under a read lock.
    /// </summary>
    /// <param name="slim">The <see cref="ReaderWriterLockSlim"/> used to control access to a resource.</param>
    /// <param name="action">The <see cref="Action"/> to execute under a read lock.</param>
    [DebuggerStepThrough]
    public static void ExecuteWithReadLock(this ReaderWriterLockSlim slim, Action action)
    {
        slim.EnterReadLock();
        try
        {
            action.Invoke();
        }
        finally
        {
            slim.ExitReadLock();
        }
    }

    /// <summary>
    /// Executes a given <see cref="Func{TResult}"/> under a read lock and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the result produced by the function.</typeparam>
    /// <param name="slim">The <see cref="ReaderWriterLockSlim"/> used to control access to a resource.</param>
    /// <param name="func">The <see cref="Func{TResult}"/> to execute under a read lock.</param>
    /// <returns>The TResult obtained from <paramref name="func"/>.</returns>
    [DebuggerStepThrough]
    public static T ExecuteWithReadLock<T>(this ReaderWriterLockSlim slim, Func<T> func)
    {
        slim.EnterReadLock();
        try
        {
            return func.Invoke();
        }
        finally
        {
            slim.ExitReadLock();
        }
    }

    /// <summary>
    /// Executes a given <see cref="Action"/> under a write lock.
    /// </summary>
    /// <param name="slim">The <see cref="ReaderWriterLockSlim"/> used to control access to a resource.</param>
    /// <param name="action">The <see cref="Action"/> to execute under a write lock.</param>
    [DebuggerStepThrough]
    public static void ExecuteWithWriteLock(this ReaderWriterLockSlim slim, Action action)
    {
        slim.EnterWriteLock();
        try
        {
            action.Invoke();
        }
        finally
        {
            slim.ExitWriteLock();
        }
    }

    /// <summary>
    /// Executes a given <see cref="Func{TResult}"/> under a write lock and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the result produced by the function.</typeparam>
    /// <param name="slim">The <see cref="ReaderWriterLockSlim"/> used to control access to a resource.</param>
    /// <param name="func">The <see cref="Func{TResult}"/> to execute under a write lock.</param>
    /// <returns>The TResult obtained from <paramref name="func"/>.</returns>
    [DebuggerStepThrough]
    public static T ExecuteWithWriteLock<T>(this ReaderWriterLockSlim slim, Func<T> func)
    {
        slim.EnterWriteLock();
        try
        {
            return func.Invoke();
        }
        finally
        {
            slim.ExitWriteLock();
        }
    }

    /// <summary>
    /// Attempts to enter the read lock, and if it's not immediately available, waits for a specified timeout.
    /// </summary>
    /// <param name="slim">The <see cref="ReaderWriterLockSlim"/> used to control access to a resource.</param>
    /// <param name="timeout">A <see cref="TimeSpan"/> representing the amount of time to wait for the read lock.</param>
    /// <param name="action">The <see cref="Action"/> to execute under a read lock if it can be entered within the timeout period.</param>
    /// <returns>A boolean indicating whether the read lock was entered.</returns>
    [DebuggerStepThrough]
    public static bool TryExecuteWithReadLock(this ReaderWriterLockSlim slim, TimeSpan timeout, Action action)
    {
        if (slim.TryEnterReadLock(timeout))
        {
            try
            {
                action();
                return true;
            }
            finally
            {
                slim.ExitReadLock();
            }
        }

        return false;
    }


    /// <summary>
    /// Attempts to enter the write lock, and if it's not immediately available, waits for a specified timeout.
    /// </summary>
    /// <param name="slim">The <see cref="ReaderWriterLockSlim"/> used to control access to a resource.</param>
    /// <param name="timeout">A <see cref="TimeSpan"/> representing the amount of time to wait for the write lock.</param>
    /// <param name="action">The <see cref="Action"/> to execute under a write lock if it can be entered within the timeout period.</param>
    /// <returns>A boolean indicating whether the write lock was entered.</returns>
    [DebuggerStepThrough]
    public static bool TryExecuteWithWriteLock(this ReaderWriterLockSlim slim, TimeSpan timeout, Action action)
    {
        if (slim.TryEnterWriteLock(timeout))
        {
            try
            {
                action();
                return true;
            }
            finally
            {
                slim.ExitWriteLock();
            }
        }

        return false;
    }


    /// <summary>
    /// Asynchronously executes a given <see cref="Func{Task}"/> under a semaphore lock.
    /// </summary>
    /// <remarks>
    /// This method is used when you want to perform an operation on a resource that requires synchronization.
    /// The semaphore is waited on before the operation is invoked and released after the operation completes.
    /// If an exception occurs within the operation, the semaphore is still correctly released.
    /// </remarks>
    /// <param name="semaphore">The <see cref="SemaphoreSlim"/> used to control access to a resource.</param>
    /// <param name="asyncAction">The <see cref="Func{Task}"/> to execute under the semaphore lock.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static async Task ExecuteWithSemaphoreAsync(this SemaphoreSlim semaphore, Func<Task> asyncAction)
    {
        await semaphore.WaitAsync();
        try
        {
            await asyncAction.Invoke();
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <summary>
    /// Asynchronously executes a given <see cref="Func{Task}"/> under a semaphore lock and returns the result.
    /// </summary>
    /// <remarks>
    /// This method is used when you want to perform an operation on a resource that requires synchronization
    /// and return a result. The semaphore is waited on before the operation is invoked and released after the operation completes.
    /// If an exception occurs within the operation, the semaphore is still correctly released.
    /// </remarks>
    /// <typeparam name="T">The type of the result produced by the function.</typeparam>
    /// <param name="semaphore">The <see cref="SemaphoreSlim"/> used to control access to a resource.</param>
    /// <param name="asyncFunc">The <see cref="Func{Task}"/>  to execute under the semaphore lock.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation, containing the TResult obtained from <paramref name="asyncFunc"/>.</returns>
    public static async Task<T> ExecuteWithSemaphoreAsync<T>(this SemaphoreSlim semaphore, Func<Task<T>> asyncFunc)
    {
        await semaphore.WaitAsync();
        try
        {
            return await asyncFunc.Invoke();
        }
        finally
        {
            semaphore.Release();
        }
    }

    /// <summary>
    /// Asynchronously attempts to enter the semaphore, and if it's not immediately available, waits for a specified timeout.
    /// </summary>
    /// <remarks>
    /// This method is used when you want to perform an operation on a resource that requires synchronization and you're okay with a potential timeout.
    /// If the method returns false, the semaphore was not entered.
    /// </remarks>
    /// <param name="semaphore">The <see cref="SemaphoreSlim"/> used to control access to a resource.</param>
    /// <param name="timeout">A <see cref="TimeSpan"/> representing the amount of time to wait for the semaphore.</param>
    /// <param name="asyncAction">The <see cref="Func{Task}"/> to execute under a semaphore lock if it can be entered within the timeout period.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation, containing a boolean indicating whether the semaphore was entered.</returns>
    public static async Task<bool> TryExecuteWithSemaphoreAsync(this SemaphoreSlim semaphore, TimeSpan timeout, Func<Task> asyncAction)
    {
        if (await semaphore.WaitAsync(timeout))
        {
            try
            {
                await asyncAction();
                return true;
            }
            finally
            {
                semaphore.Release();
            }
        }

        return false;
    }

    /// <summary>
    /// Asynchronously executes an action after waiting for a given <see cref="System.Threading.AutoResetEvent"/>.
    /// </summary>
    /// <param name="autoResetEvent">The <see cref="System.Threading.AutoResetEvent"/> to wait on.</param>
    /// <param name="action">The action to execute after the wait.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static async Task ExecuteAfterAutoResetEventAsync(this AutoResetEvent autoResetEvent, Action action)
    {
        await Task.Run(autoResetEvent.WaitOne);
        action.Invoke();
    }

    /// <summary>
    /// Asynchronously waits for the <see cref="System.Threading.CountdownEvent"/> to become set.
    /// </summary>
    /// <param name="countdownEvent">The <see cref="System.Threading.CountdownEvent"/> to wait on.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task WaitAsync(this CountdownEvent countdownEvent)
    {
        var tcs = new TaskCompletionSource<bool>();
        if (countdownEvent.IsSet)
            return Task.CompletedTask;

        ThreadPool.QueueUserWorkItem(_ =>
        {
            try
            {
                countdownEvent.Wait();
                tcs.TrySetResult(true);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        });
        return tcs.Task;
    }

    /// <summary>
    /// Asynchronously acquires an upgradeable read lock, performs an action, and then releases the lock.
    /// </summary>
    /// <param name="readerWriterLockSlim">The <see cref="ReaderWriterLockSlim"/> to perform the operation under.</param>
    /// <param name="action">The action to perform under the lock.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task ExecuteWithUpgradeableReadLockAsync(this ReaderWriterLockSlim readerWriterLockSlim, Action action)
    {
        return Task.Run(() =>
        {
            readerWriterLockSlim.EnterUpgradeableReadLock();
            try
            {
                action.Invoke();
            }
            finally
            {
                readerWriterLockSlim.ExitUpgradeableReadLock();
            }
        });
    }

    /// <summary>
    /// Asynchronously acquires a write lock, performs an action, and then releases the lock.
    /// </summary>
    /// <param name="readerWriterLockSlim">The <see cref="ReaderWriterLockSlim"/> to perform the operation under.</param>
    /// <param name="action">The action to perform under the lock.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static Task ExecuteWithWriteLockAsync(this ReaderWriterLockSlim readerWriterLockSlim, Action action)
    {
        return Task.Run(() =>
        {
            readerWriterLockSlim.EnterWriteLock();
            try
            {
                action.Invoke();
            }
            finally
            {
                readerWriterLockSlim.ExitWriteLock();
            }
        });
    }
}

