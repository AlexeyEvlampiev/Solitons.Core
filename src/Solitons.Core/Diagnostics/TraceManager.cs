using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;

namespace Solitons.Diagnostics;

/// <summary>
/// Provides helper methods for managing trace listeners.
/// </summary>
public static class TraceManager
{
    /// <summary>
    /// Adds a text file as a trace listener.
    /// </summary>
    /// <param name="path">The file path to which the trace output will be written.</param>
    /// <param name="configure">An optional action to configure the text writer trace listener.</param>
    /// <returns>An <see cref="IDisposable"/> object representing the added trace listener. Dispose the object to unregister the listener and release associated resources.</returns>
    public static IDisposable AttachTextFileListener(string path, Action<TextWriterTraceListener>? configure = null)
    {
        Action onError = () => { };
        try
        {
            var traceOutput = File.OpenWrite(path);
            onError = traceOutput.Dispose;

            var listener = new TextWriterTraceListener(traceOutput);
            configure?.Invoke(listener);
            onError += listener.Dispose;

            Trace.Listeners.Add(listener);
            void RemoveListener() => Trace.Listeners.Remove(listener);
            onError = RemoveListener + onError;

            return Disposable.Create(() =>
            {
                RemoveListener();
                listener.Flush();
                traceOutput.Flush();

                listener.Dispose();
                traceOutput.Dispose();
            });
        }
        catch (Exception e)
        {
            onError.Invoke();
            throw;
        }
    }

    /// <summary>
    /// Adds an XML file as a trace listener.
    /// </summary>
    /// <param name="path">The file path to which the trace output will be written.</param>
    /// <param name="configure">An optional action to configure the XML writer trace listener.</param>
    /// <returns>An <see cref="IDisposable"/> object representing the added trace listener. Dispose the object to unregister the listener and release associated resources.</returns>
    public static IDisposable AttachXmlFileListener(string path, Action<XmlWriterTraceListener>? configure = null)
    {
        Action onError = () => { };
        try
        {
            var traceOutput = File.OpenWrite(path);
            onError = traceOutput.Dispose;

            var listener = new XmlWriterTraceListener(traceOutput);
            configure?.Invoke(listener);
            onError += listener.Dispose;

            Trace.Listeners.Add(listener);
            void RemoveListener() => Trace.Listeners.Remove(listener);
            onError = RemoveListener + onError;

            return Disposable.Create(() =>
            {
                RemoveListener();
                listener.Flush();
                traceOutput.Flush();

                listener.Dispose();
                traceOutput.Dispose();
            });
        }
        catch (Exception e)
        {
            onError.Invoke();
            throw;
        }
    }

    /// <summary>
    /// Adds a console trace listener.
    /// </summary>
    /// <param name="configure">An optional action to configure the console trace listener.</param>
    /// <returns>An <see cref="IDisposable"/> object representing the added trace listener. Dispose the object to unregister the listener and release associated resources.</returns>
    public static IDisposable AttachConsoleListener(Action<ConsoleTraceListener>? configure = null)
    {
        var listener = new ConsoleTraceListener();
        configure?.Invoke(listener);
        Trace.Listeners.Add(listener);
        return Disposable.Create(() =>
        {
            Trace.Listeners.Remove(listener);
            listener.Dispose();
        });
    }
}