using System;

namespace Solitons.IO;

/// <summary>
/// Represents a scope for temporarily changing the foreground and background colors of the console output.
/// </summary>
public class ConsoleColorScope : IDisposable
{
    private static readonly object SyncObj = new object();
    private readonly ConsoleColor _previousForeground;
    private readonly ConsoleColor _previousBackground;

    /// <summary>
    /// Initializes a new instance of the ConsoleColorScope class with the specified foreground and background colors.
    /// </summary>
    /// <param name="newForeground">The new foreground color to set.</param>
    /// <param name="newBackground">The new background color to set.</param>
    private ConsoleColorScope(ConsoleColor newForeground, ConsoleColor newBackground)
    {
        lock (SyncObj)
        {
            _previousForeground = Console.ForegroundColor;
            _previousBackground = Console.BackgroundColor;

            Console.ForegroundColor = newForeground;
            Console.BackgroundColor = newBackground;
        }
    }

    /// <summary>
    /// Creates a ConsoleColorScope instance with the specified foreground and background colors.
    /// </summary>
    /// <param name="newForeground">The new foreground color to set.</param>
    /// <param name="newBackground">The new background color to set.</param>
    /// <returns>An IDisposable representing the ConsoleColorScope instance.</returns>
    public static IDisposable Create(ConsoleColor newForeground, ConsoleColor newBackground)
    {
        return new ConsoleColorScope(newForeground, newBackground);
    }

    /// <summary>
    /// Creates a ConsoleColorScope instance with the specified foreground color and the current background color.
    /// </summary>
    /// <param name="newForeground">The new foreground color to set.</param>
    /// <returns>An IDisposable representing the ConsoleColorScope instance.</returns>
    public static IDisposable Create(ConsoleColor newForeground)
    {
        return new ConsoleColorScope(newForeground, Console.BackgroundColor);
    }

    public static T Invoke<T>(ConsoleColor foreground, Func<T> func)
    {
        using (Create(foreground))
        {
            return func.Invoke();
        }
    }

    /// <summary>
    /// Restores the previous foreground and background colors of the console output.
    /// </summary>
    public void Dispose()
    {
        lock (SyncObj)
        {
            Console.ForegroundColor = _previousForeground;
            Console.BackgroundColor = _previousBackground;
        }
    }
}