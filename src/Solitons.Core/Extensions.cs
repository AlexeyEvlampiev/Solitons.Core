using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Data;

namespace Solitons;

public static partial class Extensions
{
    /// <summary>
    /// Creates a task that will complete after a time delay specified by the TimeSpan.
    /// </summary>
    /// <param name="self">The time interval after which the task completes.</param>
    /// <param name="cancellation">The token to monitor for cancellation requests. The default value is None.</param>
    /// <returns>A task that represents the delay. This task will complete after the time span specified by <paramref name="self"/> has elapsed.</returns>
    [DebuggerNonUserCode]
    public static Task AsDelay(this TimeSpan self, CancellationToken cancellation = default) => Task.Delay(self, cancellation);



    /// <summary>
    /// Generates an observable sequence that repeats after each TimeSpan interval.
    /// </summary>
    /// <param name="self">The interval between each occurrence in the output sequence.</param>
    /// <returns>An observable sequence that produces a value after each specified interval.</returns>
    [DebuggerNonUserCode]
    public static IObservable<long> AsTimer(this TimeSpan self) => Observable.Interval(self);

    /// <summary>
    /// Generates an observable sequence that repeats after each TimeSpan interval, using the specified scheduler.
    /// </summary>
    /// <param name="self">The interval between each occurrence in the output sequence.</param>
    /// <param name="scheduler">The scheduler to schedule the intervals on.</param>
    /// <returns>An observable sequence that produces a value after each specified interval on the given scheduler.</returns>
    [DebuggerNonUserCode]
    public static IObservable<long> AsTimer(this TimeSpan self, IScheduler scheduler) => Observable.Interval(self, scheduler);

    /// <summary>
    /// Generates an observable sequence that repeats after a specified TimeSpan due time and then after each TimeSpan interval.
    /// </summary>
    /// <param name="self">The interval between each occurrence in the output sequence.</param>
    /// <param name="dueTime">The due time before the first occurrence in the output sequence.</param>
    /// <returns>An observable sequence that produces a value after the specified due time and then after each interval.</returns>
    [DebuggerNonUserCode]
    public static IObservable<long> AsTimer(this TimeSpan self, TimeSpan dueTime) => Observable.Timer(dueTime, self);

    /// <summary>
    /// Generates an observable sequence that repeats after a specified TimeSpan due time and then after each TimeSpan interval, using the specified scheduler.
    /// </summary>
    /// <param name="self">The interval between each occurrence in the output sequence.</param>
    /// <param name="dueTime">The due time before the first occurrence in the output sequence.</param>
    /// <param name="scheduler">The scheduler to schedule the intervals on.</param>
    /// <returns>An observable sequence that produces a value after the specified due time and then after each interval on the given scheduler.</returns>
    [DebuggerNonUserCode]
    public static IObservable<long> AsTimer(this TimeSpan self, TimeSpan dueTime, IScheduler scheduler) => Observable.Timer(dueTime, self, scheduler);


    /// <summary>
    /// Returns a new <see cref="System.Guid"/> that is a copy of the specified Guid, 
    /// except that the last 32 bits are replaced with the specified Int32 value.
    /// </summary>
    /// <param name="guid">The Guid to copy.</param>
    /// <param name="value">The Int32 value to use for the last 32 bits of the returned Guid.</param>
    /// <param name="useLittleEndian">Optional parameter indicating whether to reverse the endianness of the Int32 value. Defaults to true.</param>
    /// <returns>
    /// A new <see cref="System.Guid"/> that is a copy of the <paramref name="guid"/>, 
    /// except that the last 32 bits are replaced with the specified <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// GUIDs are meant to be globally unique identifiers. Altering them in this way 
    /// could potentially violate their uniqueness, especially if the same Int32 value is used multiple times.
    /// </remarks>
    public static Guid ReplaceLast32Bits(this Guid guid, int value, bool useLittleEndian = true)
    {
        byte[] bytes = guid.ToByteArray();
        int finalValue = useLittleEndian ? BinaryPrimitives.ReverseEndianness(value) : value;
        Buffer.BlockCopy(BitConverter.GetBytes(finalValue), 0, bytes, 12, 4);
        return new Guid(bytes);
    }

    /// <summary>
    /// Returns a new <see cref="System.Guid"/> that is a copy of the specified Guid,
    /// except that the last 16 bits are replaced with the specified Int16 value.
    /// </summary>
    /// <param name="guid">The Guid to copy.</param>
    /// <param name="value">The Int16 value to use for the last 16 bits of the returned Guid.</param>
    /// <param name="useLittleEndian">Optional parameter indicating whether to reverse the endianness of the Int16 value. Defaults to true.</param>
    /// <returns>
    /// A new <see cref="System.Guid"/> that is a copy of the <paramref name="guid"/>,
    /// except that the last 16 bits are replaced with the specified <paramref name="value"/>.
    /// </returns>
    public static Guid ReplaceLast16Bits(this Guid guid, short value, bool useLittleEndian = true)
    {
        byte[] bytes = guid.ToByteArray();
        short finalValue = useLittleEndian ? BinaryPrimitives.ReverseEndianness(value) : value;
        Buffer.BlockCopy(BitConverter.GetBytes(finalValue), 0, bytes, 14, 2);
        return new Guid(bytes);
    }

    /// <summary>
    /// Returns a new <see cref="System.Guid"/> that is a copy of the specified Guid,
    /// except that the last 64 bits are replaced with the specified Int64 value.
    /// </summary>
    /// <param name="guid">The Guid to copy.</param>
    /// <param name="value">The Int64 value to use for the last 64 bits of the returned Guid.</param>
    /// <param name="useLittleEndian">Optional parameter indicating whether to reverse the endianness of the Int64 value. Defaults to true.</param>
    /// <returns>
    /// A new <see cref="System.Guid"/> that is a copy of the <paramref name="guid"/>,
    /// except that the last 64 bits are replaced with the specified <paramref name="value"/>.
    /// </returns>
    public static Guid ReplaceLast64Bits(this Guid guid, long value, bool useLittleEndian = true)
    {
        byte[] bytes = guid.ToByteArray();
        long finalValue = useLittleEndian ? BinaryPrimitives.ReverseEndianness(value) : value;
        Buffer.BlockCopy(BitConverter.GetBytes(finalValue), 0, bytes, 8, 8);
        return new Guid(bytes);
    }

    /// <summary>
    /// Flattens the hierarchy of an exception and its inner exceptions into a linear sequence.
    /// This allows you to examine all the exceptions, in the order they were thrown, that contributed to the final exception.
    /// </summary>
    /// <param name="exception">The root exception to flatten.</param>
    /// <returns>An IEnumerable of Exception that includes the root exception and all inner exceptions.</returns>
    public static IEnumerable<Exception> FlattenHierarchy(this Exception exception)
    {
        Exception? current = exception;
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        while (current != null)
        {
            yield return current;
            current = current.InnerException;
        }
    }

    /// <summary>
    /// Scales a TimeSpan duration by a specified factor raised to a specified power.
    /// </summary>
    /// <param name="originalDuration">The original TimeSpan duration to be scaled.</param>
    /// <param name="scaleFactor">The factor by which to scale the duration. Must be non-negative.</param>
    /// <param name="scaleFactorExponent">The exponent to which to raise the scale factor. Must be non-negative.</param>
    /// <returns>A new TimeSpan representing the original duration scaled by the scale factor to the power of the exponent.</returns>
    /// <exception cref="ArgumentException">Thrown when scaleFactor or scaleFactorExponent are negative.</exception>
    public static TimeSpan ScaleByFactor(this TimeSpan originalDuration, double scaleFactor, int scaleFactorExponent)
    {
        if (scaleFactor < 0d)
            throw new ArgumentException("Scale factor cannot be negative.", nameof(scaleFactor));

        if (scaleFactorExponent < 0)
            throw new ArgumentException("Exponent cannot be negative.", nameof(scaleFactorExponent));

        double scaleFactorToThePower;

        switch (scaleFactorExponent)
        {
            case 0:
                scaleFactorToThePower = 1;
                break;
            case 1:
                scaleFactorToThePower = scaleFactor;
                break;
            case 2:
                scaleFactorToThePower = scaleFactor * scaleFactor;
                break;
            default:
                scaleFactorToThePower = Math.Pow(scaleFactor, scaleFactorExponent);
                break;
        }

        double milliseconds = originalDuration.TotalMilliseconds * scaleFactorToThePower;
        return TimeSpan.FromMilliseconds(milliseconds);
    }

    /// <summary>
    /// Gets the extension of the specified file.
    /// </summary>
    /// <param name="file">The <see cref="FileInfo"/> representing the file.</param>
    /// <returns>The extension of the file.</returns>
    [DebuggerNonUserCode]
    public static string GetExtension(this FileInfo file) => Path.GetExtension(file.Name);

    /// <summary>
    /// Gets the file name without the extension from the specified file.
    /// </summary>
    /// <param name="file">The <see cref="FileInfo"/> representing the file.</param>
    /// <returns>The file name without the extension.</returns>
    [DebuggerNonUserCode]
    public static string GetFileNameWithoutExtension(this FileInfo file) => Path.GetFileNameWithoutExtension(file.Name);

    /// <summary>
    /// Gets the relative path of the specified file or directory with respect to the specified base directory.
    /// </summary>
    /// <param name="target">The target <see cref="FileSystemInfo"/> representing the file or directory.</param>
    /// <param name="relativeTo">The <see cref="DirectoryInfo"/> representing the base directory.</param>
    /// <param name="delimiter">The delimiter character to use for the path (default is '/').</param>
    /// <returns>The relative path of the target file or directory with respect to the base directory.</returns>
    public static string GetRelativePath(this FileSystemInfo target, DirectoryInfo relativeTo, char delimiter = '/')
    {
        var path = Path
            .GetRelativePath(relativeTo.FullName, target.FullName);
        return Regex.Replace(path, @"[/\\]", delimiter.ToString());
    }

    /// <summary>
    /// Reads all bytes from the current position of the <see cref="BinaryReader"/> and returns them as a byte array.
    /// </summary>
    /// <param name="reader">The <see cref="BinaryReader"/> object to read bytes from.</param>
    /// <returns>A byte array containing all the bytes read from the <paramref name="reader"/>.</returns>
    public static byte[] ReadAllBytes(this BinaryReader reader)
    {
        const int bufferSize = 4096;
        using var ms = new MemoryStream();
        byte[] buffer = new byte[bufferSize];
        int count;
        while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
            ms.Write(buffer, 0, count);
        return ms.ToArray();
    }

    /// <summary>
    /// Executes the specified <paramref name="callback"/> with the <see cref="IDbCommand"/> object created from the <see cref="IDbConnection"/>.
    /// </summary>
    /// <param name="self">The <see cref="IDbConnection"/> object to create a <see cref="IDbCommand"/> object from.</param>
    /// <param name="callback">The <see cref="Action{T}"/> to execute with the created <see cref="IDbCommand"/> object.</param>
    /// <remarks>The <see cref="IDbCommand"/> object is disposed automatically after the <paramref name="callback"/> has completed.</remarks>
    public static void Do(this IDbConnection self, Action<IDbCommand> callback)
    {
        using var command = self.CreateCommand();
        callback.Invoke(command);
    }

    /// <summary>
    /// Asynchronously executes the specified <paramref name="callback"/> with the <see cref="DbCommand"/> object created from the <see cref="DbConnection"/>.
    /// </summary>
    /// <param name="self">The <see cref="DbConnection"/> object to create a <see cref="DbCommand"/> object from.</param>
    /// <param name="callback">The <see cref="Func{T, TResult}"/> to execute with the created <see cref="DbCommand"/> object.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous execution of the <paramref name="callback"/>.</returns>
    /// <remarks>The <see cref="DbCommand"/> object is disposed automatically after the <paramref name="callback"/> has completed.</remarks>
    [DebuggerStepThrough]
    public static async Task DoAsync(this DbConnection self, Func<DbCommand, Task> callback)
    {
        await using var command = self.CreateCommand();
        await callback.Invoke(command);
    }



    /// <summary>
    /// Executes the specified <paramref name="callback"/> with the <see cref="IDbCommand"/> object created from the <see cref="IDbConnection"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result object returned by the <paramref name="callback"/>.</typeparam>
    /// <param name="self">The <see cref="IDbConnection"/> object to create a <see cref="IDbCommand"/> object from.</param>
    /// <param name="callback">The <see cref="Func{T, TResult}"/> to execute with the created <see cref="IDbCommand"/> object.</param>
    /// <returns>The result of executing the <paramref name="callback"/> with the created <see cref="IDbCommand"/> object.</returns>
    /// <remarks>The <see cref="IDbCommand"/> object is disposed automatically after the <paramref name="callback"/> has completed.</remarks>
    public static T Do<T>(this IDbConnection self, Func<IDbCommand, T> callback)
    {
        using var command = self.CreateCommand();
        return callback.Invoke(command);
    }

    /// <summary>
    /// Asynchronously executes the specified <paramref name="callback"/> with the <see cref="DbCommand"/> object created from the <see cref="DbConnection"/>, and returns the result of the execution.
    /// </summary>
    /// <typeparam name="T">The type of the result returned by the <paramref name="callback"/>.</typeparam>
    /// <param name="self">The <see cref="DbConnection"/> object to create a <see cref="DbCommand"/> object from.</param>
    /// <param name="callback">The <see cref="Func{T, TResult}"/> to execute with the created <see cref="DbCommand"/> object.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous execution of the <paramref name="callback"/> and containing the result of the execution.</returns>
    /// <remarks>The <see cref="DbCommand"/> object is disposed automatically after the <paramref name="callback"/> has completed.</remarks>
    [DebuggerStepThrough]
    public static async Task<T> DoAsync<T>(this DbConnection self, Func<DbCommand, Task<T>> callback)
    {
        await using var command = self.CreateCommand();
        return await callback.Invoke(command);
    }


    /// <summary>
    /// Executes a non-query command (e.g., INSERT, UPDATE, DELETE) asynchronously on a database.
    /// </summary>
    /// <param name="self">The DbConnection to execute the command on.</param>
    /// <param name="commandText">The text of the command to be executed.</param>
    /// <param name="cancellation">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [DebuggerNonUserCode]
    public static async Task<int> ExecuteNonQueryAsync(
        this DbConnection self, 
        string commandText, 
        CancellationToken cancellation = default)
    {
        await using var command = self.CreateCommand();
        command.CommandText = commandText;
        return await command.ExecuteNonQueryAsync(cancellation);
    }

    



    /// <summary>
    /// Executes a command that returns a single scalar value, converts the result to a specified type <typeparamref name="T"/>, 
    /// and returns that value asynchronously.
    /// </summary>
    /// <param name="self">The DbConnection to execute the command on.</param>
    /// <param name="commandText">The text of the command to be executed.</param>
    /// <param name="cancellation">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <typeparam name="T">The type to convert the result to.</typeparam>
    /// <returns>A task that represents the asynchronous operation. The task result contains the first column of the first row in the result set, or a default value if the result set is empty.</returns>
    [DebuggerNonUserCode]
    public static async Task<T?> ExecuteScalarAsync<T>(
        this DbConnection self,
        string commandText,
        CancellationToken cancellation = default)
    {
        await using var command = self.CreateCommand();
        command.CommandText = commandText;
        var response = await command.ExecuteScalarAsync(cancellation) ?? DBNull.Value;
        return response is not DBNull ? (T)response! : default(T?);
    }

    /// <summary>
    /// Executes a SQL command asynchronously that returns a single value, and returns the first column of the first row in the result set cast to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to cast the result to.</typeparam>
    /// <param name="self">The DbConnection object this method extends.</param>
    /// <param name="commandText">The SQL command to execute.</param>
    /// <param name="parameters">A dictionary of parameter names and values to include in the command.</param>
    /// <param name="cancellation">An optional CancellationToken that can be used to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the first column of the first row in the result set cast to the specified type, or null if the result set is empty.</returns>
    /// <remarks>
    /// This method uses parameterized queries to help protect against SQL injection attacks.
    /// Each key-value pair in the parameters dictionary is added as a parameter to the command.
    /// The key is the parameter name and the value is the parameter value.
    /// </remarks>
    [DebuggerNonUserCode]
    public static async Task<T?> ExecuteScalarAsync<T>(
        this DbConnection self,
        string commandText,
        Dictionary<string, object> parameters,
        CancellationToken cancellation = default)
    {
        await using var command = self.CreateCommand();
        command.CommandText = commandText;
        foreach (var param in parameters)
        {
            var dbParameter = command.CreateParameter();
            dbParameter.ParameterName = param.Key;
            dbParameter.Value = param.Value;
            command.Parameters.Add(dbParameter);
        }
        var response = await command.ExecuteScalarAsync(cancellation) ?? DBNull.Value;
        return response is not DBNull ? (T)response! : default(T?);
    }


    /// <summary>
    /// Executes a SQL command asynchronously and returns the first column of the first row in the result set returned by the query.
    /// Additional columns or rows are ignored.
    /// </summary>
    /// <param name="connection">The DbConnection object this method extends.</param>
    /// <param name="commandText">The SQL command to execute.</param>
    /// <param name="cancellationToken">An optional CancellationToken that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the first column of the first row in the result set, or a null reference if the result set is empty.</returns>
    /// <remarks>
    /// This method is typically used when a command returns a single value. If the command results in a result set with multiple rows or columns, only the first column of the first row is returned.
    /// The command is executed using the Connection, Transaction, and CommandTimeout property values of the command. If you wish to use different property values, you should use the other overload of the ExecuteScalarAsync method.
    /// </remarks>
    [DebuggerNonUserCode]
    public static async Task<object> ExecuteScalarAsync(
        this DbConnection connection, 
        string commandText, 
        CancellationToken cancellationToken = default)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = commandText;
        return await command.ExecuteScalarAsync(cancellationToken) ?? DBNull.Value;
    }

    /// <summary>
    /// Executes a SQL command asynchronously that returns a single value, and returns the first column of the first row in the result set.
    /// </summary>
    /// <param name="connection">The DbConnection object this method extends.</param>
    /// <param name="commandText">The SQL command to execute.</param>
    /// <param name="parameters">A dictionary of parameter names and values to include in the command.</param>
    /// <param name="cancellationToken">An optional CancellationToken that can be used to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the first column of the first row in the result set, or DBNull.Value if the result set is empty.</returns>
    /// <remarks>
    /// This method uses parameterized queries to help protect against SQL injection attacks.
    /// Each key-value pair in the parameters dictionary is added as a parameter to the command.
    /// The key is the parameter name and the value is the parameter value.
    /// </remarks>
    [DebuggerNonUserCode]
    public static async Task<object> ExecuteScalarAsync(
        this DbConnection connection,
        string commandText,
        Dictionary<string, object> parameters,
        CancellationToken cancellationToken = default)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = commandText;
        foreach (var param in parameters)
        {
            var dbParameter = command.CreateParameter();
            dbParameter.ParameterName = param.Key;
            dbParameter.Value = param.Value;
            command.Parameters.Add(dbParameter);
        }
        return await command.ExecuteScalarAsync(cancellationToken) ?? DBNull.Value;
    }



    /// <summary>
    /// Executes a SQL command asynchronously that does not return any data (e.g., INSERT, UPDATE, DELETE), and returns the number of rows affected.
    /// </summary>
    /// <param name="connection">The DbConnection object this method extends.</param>
    /// <param name="commandText">The SQL command to execute.</param>
    /// <param name="parameters">A dictionary of parameter names and values to include in the command.</param>
    /// <param name="cancellationToken">An optional CancellationToken that can be used to cancel the operation.</param>
    /// <returns>A Task representing the asynchronous operation, with a result of the number of rows affected by the command.</returns>
    /// <remarks>
    /// This method uses parameterized queries to help protect against SQL injection attacks.
    /// Each key-value pair in the parameters dictionary is added as a parameter to the command.
    /// The key is the parameter name and the value is the parameter value.
    /// </remarks>
    [DebuggerStepThrough]
    public static async Task<int> ExecuteNonQueryAsync(
        this DbConnection connection, 
        string commandText, 
        Dictionary<string, object> parameters, 
        CancellationToken cancellationToken = default)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = commandText;
        foreach (var param in parameters)
        {
            var dbParameter = command.CreateParameter();
            dbParameter.ParameterName = param.Key;
            dbParameter.Value = param.Value;
            command.Parameters.Add(dbParameter);
        }
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }



    /// <summary>
    /// Executes a SQL command asynchronously and returns the results as an IAsyncEnumerable of IDataRecord.
    /// </summary>
    /// <param name="self">The DbConnection object this method extends.</param>
    /// <param name="commandText">The SQL command to execute.</param>
    /// <param name="cancellation">An optional CancellationToken that can be used to cancel the operation.</param>
    /// <returns>An IAsyncEnumerable of IDataRecord objects representing the rows in the result set.</returns>
    /// <remarks>
    /// The consumer of this method needs to understand the lifetime of the IDataRecord objects that are returned.
    /// Each IDataRecord object is only valid until the next one is retrieved or until the IAsyncEnumerator is disposed.
    /// Therefore, if you need to keep data from an IDataRecord object around for longer, you should copy it out of the IDataRecord into your own objects or variables.
    /// Also, this method only supports commands that return a single result set. If your command returns multiple result sets, you will need to handle this differently.
    /// </remarks>
    [DebuggerNonUserCode]
    public static async IAsyncEnumerable<IDataRecord> ExecuteReaderAsync(
        this DbConnection self,
        string commandText,
        [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        await using var command = self.CreateCommand();
        command.CommandText = commandText;
        await using var reader = await command.ExecuteReaderAsync(cancellation);
        
        while (await reader.ReadAsync(cancellation))
        {
            yield return reader;
        }
    }

    /// <summary>
    /// Executes a SQL command asynchronously and returns the results as an IAsyncEnumerable of type T.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <param name="self">The DbConnection object this method extends.</param>
    /// <param name="commandText">The SQL command to execute.</param>
    /// <param name="factory">A function that creates an object of type T from an IDataRecord.</param>
    /// <param name="cancellation">An optional CancellationToken that can be used to cancel the operation.</param>
    /// <returns>An IAsyncEnumerable of objects of type T representing the rows in the result set.</returns>
    /// <remarks>
    /// This method uses the provided factory function to create an object of type T from each IDataRecord in the result set.
    /// The consumer of this method needs to understand the lifetime of the IDataRecord objects that are passed to the factory function.
    /// Each IDataRecord object is only valid until the next one is retrieved or until the IAsyncEnumerator is disposed.
    /// Therefore, the factory function should not keep a reference to the IDataRecord object and should instead copy out any data it needs.
    /// </remarks>
    [DebuggerNonUserCode]
    public static async IAsyncEnumerable<T> ExecuteReaderAsync<T>(
        this DbConnection self,
        string commandText,
        Func<IDataRecord, T> factory,
        [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        await using var command = self.CreateCommand();
        command.CommandText = commandText;
        await using var reader = await command.ExecuteReaderAsync(cancellation);

        while (await reader.ReadAsync(cancellation))
        {
            yield return factory(reader);
        }
    }


    /// <summary>
    /// Executes a SQL command asynchronously and returns the results as an IAsyncEnumerable of type T.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <param name="self">The DbConnection object this method extends.</param>
    /// <param name="commandText">The SQL command to execute.</param>
    /// <param name="parameters">A dictionary of parameter names and values to include in the command.</param>
    /// <param name="factory">A function that creates an object of type T from an IDataRecord.</param>
    /// <param name="cancellation">An optional CancellationToken that can be used to cancel the operation.</param>
    /// <returns>An IAsyncEnumerable of objects of type T representing the rows in the result set.</returns>
    /// <remarks>
    /// This method uses parameterized queries to help protect against SQL injection attacks.
    /// Each key-value pair in the parameters dictionary is added as a parameter to the command.
    /// The key is the parameter name and the value is the parameter value.
    /// This method uses the provided factory function to create an object of type T from each IDataRecord in the result set.
    /// The consumer of this method needs to understand the lifetime of the IDataRecord objects that are passed to the factory function.
    /// Each IDataRecord object is only valid until the next one is retrieved or until the IAsyncEnumerator is disposed.
    /// Therefore, the factory function should not keep a reference to the IDataRecord object and should instead copy out any data it needs.
    /// </remarks>
    [DebuggerNonUserCode]
    public static async IAsyncEnumerable<T> ExecuteReaderAsync<T>(
        this DbConnection self,
        string commandText,
        Dictionary<string, object> parameters,
        Func<IDataRecord, T> factory,
        [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        await using var command = self.CreateCommand();
        command.CommandText = commandText;
        foreach (var param in parameters)
        {
            var dbParameter = command.CreateParameter();
            dbParameter.ParameterName = param.Key;
            dbParameter.Value = param.Value;
            command.Parameters.Add(dbParameter);
        }
        await using var reader = await command.ExecuteReaderAsync(cancellation);

        while (await reader.ReadAsync(cancellation))
        {
            yield return factory(reader);
        }
    }



    /// <summary>
    /// Executes a SQL command asynchronously and returns the results as an IAsyncEnumerable of IDataRecord.
    /// </summary>
    /// <param name="self">The DbConnection object this method extends.</param>
    /// <param name="commandText">The SQL command to execute.</param>
    /// <param name="parameters">A dictionary of parameter names and values to include in the command.</param>
    /// <param name="cancellation">An optional CancellationToken that can be used to cancel the operation.</param>
    /// <returns>An IAsyncEnumerable of IDataRecord objects representing the rows in the result set.</returns>
    /// <remarks>
    /// This method uses parameterized queries to help protect against SQL injection attacks.
    /// Each key-value pair in the parameters dictionary is added as a parameter to the command.
    /// The key is the parameter name and the value is the parameter value.
    /// The consumer of this method needs to understand the lifetime of the IDataRecord objects that are returned.
    /// Each IDataRecord object is only valid until the next one is retrieved or until the IAsyncEnumerator is disposed.
    /// Therefore, if you need to keep data from an IDataRecord object around for longer, you should copy it out of the IDataRecord into your own objects or variables.
    /// </remarks>
    [DebuggerNonUserCode]
    public static async IAsyncEnumerable<IDataRecord> ExecuteReaderAsync(
        this DbConnection self,
        string commandText,
        Dictionary<string, object> parameters,
        [EnumeratorCancellation] CancellationToken cancellation = default)
    {
        await using var command = self.CreateCommand();
        command.CommandText = commandText;
        foreach (var param in parameters)
        {
            var dbParameter = command.CreateParameter();
            dbParameter.ParameterName = param.Key;
            dbParameter.Value = param.Value;
            command.Parameters.Add(dbParameter);
        }
        await using var reader = await command.ExecuteReaderAsync(cancellation);

        while (await reader.ReadAsync(cancellation))
        {
            yield return reader;
        }
    }


    /// <summary>
    /// Executes the specified <paramref name="callback"/> with the <see cref="IDbCommand"/> object created from the <see cref="IDbConnection"/> and the given <paramref name="commandText"/>, and returns the result of the <paramref name="callback"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result to return.</typeparam>
    /// <param name="self">The <see cref="IDbConnection"/> object to create a <see cref="IDbCommand"/> object from.</param>
    /// <param name="commandText">The text of the command to execute.</param>
    /// <param name="callback">The <see cref="Func{T, TResult}"/> to execute with the created <see cref="IDbCommand"/> object.</param>
    /// <returns>The result of the <paramref name="callback"/> execution.</returns>
    /// <remarks>The <see cref="IDbCommand"/> object is disposed automatically after the <paramref name="callback"/> has completed.</remarks>
    public static T Do<T>(this IDbConnection self, string commandText, Func<IDbCommand, T> callback)
    {
        using var command = self.CreateCommand();
        command.CommandText = commandText;
        return callback.Invoke(command);
    }


    /// <summary>
    /// Inverts the <paramref name="self"/> <see cref="SortOrder"/> value.
    /// </summary>
    /// <param name="self">The <see cref="SortOrder"/> value to invert.</param>
    /// <returns>The inverted <see cref="SortOrder"/> value.</returns>
    /// <remarks>The <see cref="SortOrder"/> value is inverted by using an XOR operation with a byte value of 1.</remarks>
    public static SortOrder Invert(this SortOrder self) => (SortOrder)((byte)self ^ 1);

    /// <summary>
    /// Appends text to the end of the current <see cref="string"/> using the specified <paramref name="config"/> to configure the <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="self">The <see cref="string"/> to append text to.</param>
    /// <param name="config">The <see cref="Action{T}"/> to configure the <see cref="StringBuilder"/> used for the append operation.</param>
    /// <returns>A new <see cref="string"/> that is the result of appending the text to the end of the original <paramref name="self"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> or <paramref name="config"/> is null.</exception>
    [DebuggerNonUserCode]
    public static string Append(this string self, Action<StringBuilder> config)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        if (config == null) throw new ArgumentNullException(nameof(config));
        var builder = new StringBuilder(self);
        config.Invoke(builder);
        return builder.ToString();
    }

    /// <summary>
    /// Converts the specified <see cref="Guid"/> to a Base64-encoded string.
    /// </summary>
    /// <param name="self">The <see cref="Guid"/> to convert.</param>
    /// <returns>A Base64-encoded string representing the <paramref name="self"/> <see cref="Guid"/>.</returns>
    /// <remarks>
    /// The resulting string can be converted back to a <see cref="Guid"/> using <see cref="System.Convert.FromBase64String"/>.
    /// </remarks>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToBase64(this Guid self)
    {
        var bytes = self.ToByteArray();
        return System.Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Determines whether the numeric value of the current <see cref="HttpStatusCode"/> object is within the specified range of values.
    /// </summary>
    /// <param name="self">The <see cref="HttpStatusCode"/> object to check.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <returns>true if the numeric value of the <paramref name="self"/> is between <paramref name="min"/> and <paramref name="max"/>; otherwise, false.</returns>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBetween(this HttpStatusCode self, int min, int max)
    {
        var code = (int)self;
        return code >= min && code <= max;
    }

    /// <summary>
    /// Returns the specified default value if the <see cref="Nullable{Guid}"/> object is null or has a value of <see cref="Guid.Empty"/>,
    /// otherwise returns the value of the <see cref="Nullable{Guid}"/> object.
    /// </summary>
    /// <param name="self">The <see cref="Nullable{Guid}"/> object to check.</param>
    /// <param name="defaultValue">The default value to return if the <see cref="Nullable{Guid}"/> object is null or has a value of <see cref="Guid.Empty"/>.</param>
    /// <returns>The value of the <see cref="Nullable{Guid}"/> object if it is not null and not equal to <see cref="Guid.Empty"/>, otherwise the specified <paramref name="defaultValue"/>.</returns>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid DefaultIfNullOrEmpty(this Guid? self, Guid defaultValue)
    {
        return self.GetValueOrDefault(Guid.Empty) == Guid.Empty
            ? defaultValue
            : self!.Value;
    }

    /// <summary>
    /// Determines whether the specified <see cref="Guid"/> value is null or empty.
    /// </summary>
    /// <param name="self">The nullable <see cref="Guid"/> value to check.</param>
    /// <returns><see langword="true"/> if the value is null or empty; otherwise, <see langword="false"/>.</returns>
    /// <remarks>An empty <see cref="Guid"/> value is considered to be equal to <see cref="Guid.Empty"/>.</remarks>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty(this Guid? self) => self == null || self == Guid.Empty;


    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> if the specified <paramref name="self"/> stream cannot be read.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="self"/> stream, which must inherit from <see cref="Stream"/>.</typeparam>
    /// <param name="self">The <paramref name="self"/> stream to check if it can be read.</param>
    /// <param name="message">An optional custom message to include in the exception.</param>
    /// <param name="paramName">The name of the <paramref name="self"/> parameter.</param>
    /// <returns>The <paramref name="self"/> stream, if it can be read.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the <paramref name="self"/> stream cannot be read.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIfCanNotRead<T>(this T self, string? message = null, [CallerArgumentExpression("self")] string paramName = "") where T : Stream
    {
        return self.CanRead
            ? self
            : throw new InvalidOperationException($"{paramName}.{nameof(self.CanRead)} is false.");
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> if the current <typeparamref name="T"/> <see cref="Stream"/> cannot be written to.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Stream"/> to check.</typeparam>
    /// <param name="self">The <see cref="Stream"/> to check for write capability.</param>
    /// <param name="message">The error message to include in the exception if the <paramref name="self"/> stream cannot be written to. Optional.</param>
    /// <param name="paramName">The name of the parameter being checked. Optional.</param>
    /// <returns>The current <typeparamref name="T"/> <see cref="Stream"/> if it can be written to.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the current <typeparamref name="T"/> <see cref="Stream"/> cannot be written to.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIfCanNotWrite<T>(this T self, string? message = null, [CallerArgumentExpression("self")] string paramName = "") where T : Stream
    {
        return self.CanWrite
            ? self
            : throw new InvalidOperationException(message
                .DefaultIfNullOrWhiteSpace($"{paramName}.{nameof(self.CanWrite)} is false.")); ;
    }



    /// <summary>
    /// Converts a byte array to a <see cref="MemoryStream"/>.
    /// </summary>
    /// <param name="bytes">The byte array to convert to a <see cref="MemoryStream"/>.</param>
    /// <returns>A new <see cref="MemoryStream"/> instance containing the same bytes as the input <paramref name="bytes"/> array.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="bytes"/> array is null.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemoryStream ToMemoryStream(this byte[] bytes)
    {
        if (bytes == null) throw new ArgumentNullException(nameof(bytes));
        return new MemoryStream(bytes);
    }


    /// <summary>
    /// Sets the console foreground color to the specified color and invokes the provided action.
    /// </summary>
    /// <param name="self">The color to set the console foreground to.</param>
    /// <param name="callback">The action to invoke.</param>
    [DebuggerStepThrough]
    public static void AsForegroundColor(this ConsoleColor self, Action callback)
    {
        try
        {
            Console.ForegroundColor = self;
            callback?.Invoke();
        }
        finally
        {
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Sets the foreground color of the console to the specified color,
    /// executes the provided callback, and then resets the console color to its original value.
    /// </summary>
    /// <typeparam name="T">The return type of the callback function.</typeparam>
    /// <param name="self">The console color to use as the foreground color.</param>
    /// <param name="callback">The callback function to execute.</param>
    /// <returns>The result of executing the callback function.</returns>
    [DebuggerStepThrough]
    public static T AsForegroundColor<T>(this ConsoleColor self, Func<T> callback)
    {
        try
        {
            Console.ForegroundColor = self;
            return callback.Invoke();
        }
        finally
        {
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Gets the target object of the specified <see cref="WeakReference{T}"/> if it is alive, or creates a new object using the specified <paramref name="factory"/> and sets it as the target object of the <see cref="WeakReference{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the target object.</typeparam>
    /// <param name="self">The <see cref="WeakReference{T}"/> object to get or create the target object from.</param>
    /// <param name="factory">The <see cref="Func{T}"/> to create a new target object if the original target object is no longer alive.</param>
    /// <returns>The target object of the <see cref="WeakReference{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either <paramref name="self"/> or <paramref name="factory"/> is <c>null</c>.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetOrCreate<T>(this WeakReference<T> self, Func<T> factory) where T : class
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        if (factory == null) throw new ArgumentNullException(nameof(factory));
        if (self.TryGetTarget(out var result)) return result;
        result = factory.Invoke();
        self.SetTarget(result);
        return result;
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> if the <see cref="Uri"/> object is not of the expected <see cref="UriKind"/>.
    /// </summary>
    /// <param name="self">The <see cref="Uri"/> object to validate.</param>
    /// <param name="expectedKind">The expected <see cref="UriKind"/> of the <paramref name="self"/> object.</param>
    /// <param name="message">The error message to include in the <see cref="InvalidOperationException"/> if thrown. If null, a default error message is used.</param>
    /// <param name="paramName">The name of the parameter that is being validated. This is automatically inferred by the compiler.</param>
    /// <returns>The original <see cref="Uri"/> object if it is of the expected <see cref="UriKind"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the <paramref name="self"/> object is not of the expected <see cref="UriKind"/>.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Uri ThrowIfNot(this Uri self, UriKind expectedKind, string? message = null, [CallerArgumentExpression("self")] string paramName = "")
    {
        if (Uri.IsWellFormedUriString(self.ToString(), expectedKind)) return self;
        throw new InvalidOperationException(message
            .DefaultIfNullOrWhiteSpace($"The value of '{paramName}' is not a well formed '{expectedKind}' Uri."));
    }

    /// <summary>
    /// Determines whether the specified <see cref="HttpStatusCode"/> represents a success status code (2xx).
    /// </summary>
    /// <param name="statusCode">The <see cref="HttpStatusCode"/> value to check.</param>
    /// <returns><c>true</c> if the <paramref name="statusCode"/> represents a success status code; otherwise, <c>false</c>.</returns>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSuccessStatusCode(this HttpStatusCode statusCode)
    {
        var x = (int)statusCode / 100;
        return x == 2;
    }

    /// <summary>
    /// Determines whether the specified <see cref="HttpStatusCode"/> value represents a redirect status code.
    /// </summary>
    /// <param name="statusCode">The <see cref="HttpStatusCode"/> value to check.</param>
    /// <returns><c>true</c> if the <paramref name="statusCode"/> represents a redirect status code; otherwise, <c>false</c>.</returns>
    /// <remarks>A redirect status code is defined as 302 (Found), 307 (Temporary Redirect), 303 (See Other), or 308 (Permanent Redirect).</remarks>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsRedirectStatusCode(this HttpStatusCode statusCode)
    {
        var value = (int)statusCode;
        return value == 302 ||
               value == 307 ||
               value == 303 ||
               value == 308;
    }

    /// <summary>
    /// Throws a <see cref="NullReferenceException"/> with the specified <paramref name="message"/> if the specified object is null.
    /// </summary>
    /// <typeparam name="T">The type of the object to check for null.</typeparam>
    /// <param name="self">The object to check for null.</param>
    /// <param name="message">The exception message to use if the object is null. If not specified, a default message is used.</param>
    /// <param name="paramName">The name of the parameter being checked. This parameter is automatically provided by the compiler and does not need to be specified explicitly.</param>
    /// <returns>The non-null object if it is not null.</returns>
    /// <exception cref="NullReferenceException">Thrown when the object is null.</exception>
    /// <remarks>The <paramref name="paramName"/> parameter is automatically provided by the compiler and represents the name of the parameter being checked.</remarks>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNull]
    public static T ThrowIfNull<T>(this T self, string? message = null, [CallerArgumentExpression("self")] string paramName = "") where T : class?
    {
        if (self is null)
        {
            throw new NullReferenceException(message.DefaultIfNullOrWhiteSpace($"{paramName} is null."));
        }
        return self;
    }



    /// <summary>
    /// Throws an exception if the specified <paramref name="predicate"/> is true for any item in the sequence.
    /// </summary>
    /// <typeparam name="T">The type of the items in the sequence.</typeparam>
    /// <param name="self">The sequence of items to check.</param>
    /// <param name="predicate">A function that returns true if an item should cause an exception to be thrown.</param>
    /// <param name="factory">A function that creates the exception to be thrown.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that returns the same items as <paramref name="self"/>, but throws an exception if any item matches the <paramref name="predicate"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> or <paramref name="factory"/> is null.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNull]
    public static IEnumerable<T> ThrowIf<T>(this IEnumerable<T> self, Func<T, bool> predicate, Func<Exception> factory)
    {
        return self.Select((item) =>
        {
            if (predicate.Invoke(item))
            {
                throw factory.Invoke();
            }
            return item;
        });
    }

    /// <summary>
    /// Zips the <see cref="Capture"/> objects of two named groups from the specified <paramref name="self"/> <see cref="Match"/> object
    /// and returns an <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/> where the key and value are
    /// <see cref="Capture"/> objects from the named groups.
    /// </summary>
    /// <param name="self">The <see cref="Match"/> object to extract <see cref="Capture"/> objects from.</param>
    /// <param name="keyGroupName">The name of the named group containing the key <see cref="Capture"/> objects.</param>
    /// <param name="valueGroupName">The name of the named group containing the value <see cref="Capture"/> objects.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/> where the key and value are <see cref="Capture"/> objects from the named groups.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="self"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="self"/>.Success is false, <paramref name="keyGroupName"/> is null or whitespace, or <paramref name="valueGroupName"/> is null or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the number of <see cref="Capture"/> objects in <paramref name="keyGroupName"/> is not equal to the number of <see cref="Capture"/> objects in <paramref name="valueGroupName"/>.</exception>
    /// <remarks>
    /// The <see cref="Capture"/> objects are extracted from the named groups using the <see cref="Match.Groups"/> property of the <paramref name="self"/> <see cref="Match"/> object.
    /// The returned <see cref="KeyValuePair{TKey, TValue}"/> objects are created using the <see cref="Enumerable.Zip{TFirst, TSecond, TResult}(IEnumerable{TFirst}, IEnumerable{TSecond}, Func{TFirst, TSecond, TResult})"/> method.
    /// </remarks>
    public static IEnumerable<KeyValuePair<Capture, Capture>> ZipCaptures(
        this Match self,
        string keyGroupName,
        string valueGroupName)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        if (self.Success == false)
        {
            throw new ArgumentException($"{nameof(self)}.{nameof(self.Success)} is false", nameof(self));
        }

        keyGroupName.ThrowIfNullOrWhiteSpace(() =>
            new ArgumentException($"Key group name is required.", nameof(keyGroupName)));
        valueGroupName.ThrowIfNullOrWhiteSpace(() =>
            new ArgumentException($"Value group name is required.", nameof(keyGroupName)));
        var keys = self.Groups[keyGroupName].Captures.ToList();
        var values = self.Groups[valueGroupName].Captures.ToList();
        if (keys.Count != values.Count)
        {
            throw new InvalidOperationException($"Detected unbound captures.");
        }

        return keys
            .Zip(values, KeyValuePair.Create);
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> with the specified <paramref name="message"/> if the current <see cref="Guid"/> instance is equal to <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="self">The current <see cref="Guid"/> instance.</param>
    /// <param name="message">The optional error message to include in the exception if thrown.</param>
    /// <param name="paramName">The optional name of the parameter that is checked for being empty.</param>
    /// <returns>The original <see cref="Guid"/> instance if it is not empty.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the <paramref name="self"/> parameter is equal to <see cref="Guid.Empty"/>.</exception>
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid ThrowIfEmpty(this Guid self, string? message = null, [CallerArgumentExpression("self")] string paramName = "")
    {
        if (self == Guid.Empty)
        {
            throw new InvalidOperationException(message
                .DefaultIfNullOrWhiteSpace($"{paramName} is empty."));
        }
        return self;
    }

    /// <summary>
    /// Throws an exception created by the specified <paramref name="createException"/> function if the specified <see cref="Guid"/> object is null or empty.
    /// </summary>
    /// <param name="self">The nullable <see cref="Guid"/> object to check for null or empty.</param>
    /// <param name="createException">The <see cref="Func{TResult}"/> that creates the <see cref="Exception"/> to throw.</param>
    /// <returns>The non-null and non-empty <see cref="Guid"/> value.</returns>
    /// <exception cref="Exception">Thrown when the <paramref name="self"/> is null or empty, with the error message generated by the <paramref name="createException"/> function.</exception>
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid ThrowIfNullOrEmpty(this Guid? self, Func<Exception> createException)
    {
        if (self == null || self == Guid.Empty)
        {
            var error = createException.Invoke();
            throw error;
        }

        return self.Value;
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> if the specified nullable <see cref="Guid"/> is null or equal to <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="self">The nullable <see cref="Guid"/> to check.</param>
    /// <param name="message">The optional custom error message to include in the exception if the check fails.</param>
    /// <param name="paramName">The name of the parameter being checked, used in the exception message.</param>
    /// <returns>The value of the specified <see cref="Guid"/> if it is not null or equal to <see cref="Guid.Empty"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the specified <paramref name="self"/> is null or equal to <see cref="Guid.Empty"/>.</exception>
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid ThrowIfNullOrEmpty(this Guid? self, string? message = null, [CallerArgumentExpression("self")] string paramName = "")
    {
        if (self == null || self == Guid.Empty)
        {
            throw new InvalidOperationException(message
                .DefaultIfNullOrWhiteSpace($"{paramName} is null or empty."));
        }

        return self.Value;
    }

    /// <summary>
    /// Returns the specified <paramref name="defaultValue"/> if the <see cref="Guid"/> value is empty; otherwise, returns the original <see cref="Guid"/> value.
    /// </summary>
    /// <param name="self">The <see cref="Guid"/> value to check for emptiness.</param>
    /// <param name="defaultValue">The <see cref="Guid"/> value to return if <paramref name="self"/> is empty.</param>
    /// <returns>The original <see cref="Guid"/> value if it is not empty; otherwise, the specified <paramref name="defaultValue"/>.</returns>
    public static Guid DefaultIfEmpty(this Guid self, Guid defaultValue)
    {
        return self == Guid.Empty
            ? defaultValue
            : self;
    }


    /// <summary>
    /// Asynchronously waits for the specified time interval, with the option to cancel the wait operation.
    /// </summary>
    /// <param name="self">The <see cref="TimeSpan"/> interval to wait for.</param>
    /// <param name="cancellation">The <see cref="CancellationToken"/> used to cancel the wait operation.</param>
    /// <param name="throwOnCancellation">A boolean indicating whether to throw an <see cref="OperationCanceledException"/> if the wait operation is canceled.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous wait operation.</returns>
    /// <remarks>If the <paramref name="cancellation"/> token is canceled before the delay has completed, and <paramref name="throwOnCancellation"/> is true, an <see cref="OperationCanceledException"/> will be thrown. Otherwise, the delay operation will be cancelled and control will return to the calling method.</remarks>
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task WaitAsync(this TimeSpan self, CancellationToken cancellation, bool throwOnCancellation = false)
    {
        try
        {
            await Task.Delay(self, cancellation);
        }
        catch (OperationCanceledException)
        {
            if (throwOnCancellation) throw;
        }
    }


    /// <summary>
    /// Converts a byte array to its equivalent string representation that is encoded with base-64 digits.
    /// </summary>
    /// <param name="self">The byte array to convert.</param>
    /// <returns>A string that consists of the base 64-encoded characters representing the input byte array.</returns>
    /// <exception cref="NullReferenceException">Thrown when the input byte array is null.</exception>
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToBase64String(this byte[] self) =>
        System.Convert.ToBase64String(self ?? throw new NullReferenceException());

    /// <summary>
    /// Converts the specified byte array to a UTF-8 encoded string.
    /// </summary>
    /// <param name="self">The byte array to convert to a string.</param>
    /// <returns>A UTF-8 encoded string representation of the byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="self"/> is null.</exception>
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToUtf8String(this byte[] self)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        return self.ToString(Encoding.UTF8);
    }

    /// <summary>
    /// Converts the specified byte array to a string using the specified <paramref name="encoding"/>.
    /// </summary>
    /// <param name="self">The byte array to convert to a string.</param>
    /// <param name="encoding">The encoding to use for the conversion.</param>
    /// <returns>A string representing the byte array.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="self"/> is <see langword="null"/> or <paramref name="encoding"/> is <see langword="null"/>.</exception>
    [DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString(this byte[] self, Encoding encoding)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        if (encoding == null) throw new ArgumentNullException(nameof(encoding));
        return encoding.GetString(self);
    }

    /// <summary>
    /// Encrypts the specified byte array using the specified <see cref="ICryptoTransform"/> object.
    /// </summary>
    /// <param name="self">The byte array to encrypt.</param>
    /// <param name="cryptoTransform">The <see cref="ICryptoTransform"/> object to use for encryption.</param>
    /// <returns>A new byte array containing the encrypted data.</returns>
    /// <remarks>The <see cref="ICryptoTransform"/> object is used to perform the encryption operation. The returned byte array contains the encrypted data.</remarks>
    public static byte[] Encrypt(this byte[] self, ICryptoTransform cryptoTransform)
    {
        using MemoryStream msEncrypt = new MemoryStream();
        using CryptoStream csEncrypt = new CryptoStream(msEncrypt, cryptoTransform, CryptoStreamMode.Write);
        csEncrypt.Write(self, 0, self.Length);
        csEncrypt.FlushFinalBlock();
        return msEncrypt.ToArray();
    }


    /// <summary>
    /// Determines whether the specified <see cref="Type"/> is a subclass of the specified generic type definition.
    /// </summary>
    /// <param name="self">The <see cref="Type"/> to check.</param>
    /// <param name="genericTypeDefinition">The generic type definition to check against.</param>
    /// <returns>true if the <paramref name="self"/> is a subclass of the <paramref name="genericTypeDefinition"/>; otherwise, false.</returns>
    /// <remarks>The method only returns true if the specified <paramref name="genericTypeDefinition"/> is a generic type definition.</remarks>
    public static bool IsSubclassOfGenericType(this Type self, Type genericTypeDefinition)
    {
        if (self.IsClass == false ||
            genericTypeDefinition.IsGenericTypeDefinition == false)
        {
            return false;
        }

        for (var type = self;
             type is { IsClass: true } && type != typeof(object);
             type = type.BaseType)
        {
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == genericTypeDefinition)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Throws an exception created by the specified <paramref name="createException"/> delegate if the value of <paramref name="self"/> is null.
    /// </summary>
    /// <typeparam name="T">The value type of the nullable value.</typeparam>
    /// <param name="self">The nullable value to check for null.</param>
    /// <param name="createException">The delegate that creates an exception to throw if <paramref name="self"/> is null.</param>
    /// <returns>The value of <paramref name="self"/> if it is not null.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="createException"/> is null.</exception>
    /// <exception cref="Exception">Thrown when <paramref name="self"/> is null and <paramref name="createException"/> returns an exception, or when <paramref name="createException"/> returns null.</exception>
    /// <remarks>
    /// Use this method to ensure that a nullable value is not null before using it.
    /// If <paramref name="self"/> is not null, its value is returned without any modification.
    /// </remarks>
    [DebuggerNonUserCode]
    [return: NotNull]
    public static T ThrowIfNull<T>(this T? self, Func<Exception> createException) where T : struct
    {
        if (createException == null) throw new ArgumentNullException(nameof(createException));
        if (self.HasValue == false)
        {
            var error = createException.Invoke() ?? throw new NullReferenceException($"{nameof(createException)}() returned null.");
            throw error;
        }
        return self.Value;
    }

    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> if the nullable value is null, optionally using the specified <paramref name="message"/> as the exception message.
    /// </summary>
    /// <typeparam name="T">The type of the nullable value.</typeparam>
    /// <param name="self">The nullable value to check.</param>
    /// <param name="message">The optional message to use for the <see cref="InvalidOperationException"/>.</param>
    /// <param name="paramName">The name of the parameter that is checked.</param>
    /// <returns>The non-nullable value of the <paramref name="self"/> if it has a value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the <paramref name="self"/> is null.</exception>
    /// <remarks>The <paramref name="paramName"/> parameter is automatically inferred from the calling argument expression.</remarks>
    [DebuggerNonUserCode]
    [return: NotNull]
    public static T ThrowIfNull<T>(this T? self, string? message = null, [CallerArgumentExpression("self")] string paramName = "") where T : struct
    {
        if (self.HasValue == false)
        {
            throw new InvalidOperationException(message
                .DefaultIfNullOrWhiteSpace($"{paramName} is null"));
        }
        return self.Value;
    }


    /// <summary>
    /// Converts an instance of <typeparamref name="TSource"/> to an instance of <typeparamref name="TResult"/> by applying the specified transform function.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object to convert.</typeparam>
    /// <typeparam name="TResult">The type of the result object after conversion.</typeparam>
    /// <param name="self">The instance of <typeparamref name="TSource"/> to convert.</param>
    /// <param name="transform">The transform function to apply.</param>
    /// <returns>The instance of <typeparamref name="TResult"/> resulting from the conversion.</returns>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Convert<TSource, TResult>(this TSource self, Func<TSource, TResult> transform) => 
        transform.Invoke(self);


    /// <summary>
    /// Converts an instance of <typeparamref name="TSource"/> to an instance of <typeparamref name="TResult"/> by applying the specified transform function.
    /// If an exception of type <typeparamref name="TException"/> is thrown during the conversion, the specified error handler is executed.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object to convert.</typeparam>
    /// <typeparam name="TResult">The type of the result object after conversion.</typeparam>
    /// <typeparam name="TException">The type of exception to handle.</typeparam>
    /// <param name="self">The instance of <typeparamref name="TSource"/> to convert.</param>
    /// <param name="transform">The transform function to apply.</param>
    /// <param name="onError">The error handler to execute if an exception of type <typeparamref name="TException"/> is thrown.</param>
    /// <returns>The instance of <typeparamref name="TResult"/> resulting from the conversion.</returns>
    /// <exception>Thrown when an exception of type <typeparamref name="TException"/> occurs during the conversion.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Convert<TSource, TResult, TException>(
        this TSource self, 
        Func<TSource, TResult> transform, 
        Action<TException> onError)
    {
        try
        {
            return transform.Invoke(self);
        }
        catch (Exception ex) when(ex is TException targetEx)
        {
            onError.Invoke(targetEx);
            throw;
        }
    }

    /// <summary>
    /// Converts an instance of <typeparamref name="TSource"/> to an instance of <typeparamref name="TResult"/> by applying the specified transform function.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object to convert.</typeparam>
    /// <typeparam name="TResult">The type of the result object after conversion.</typeparam>
    /// <param name="self">The instance of <typeparamref name="TSource"/> to convert.</param>
    /// <param name="transform">The transform function to apply.</param>
    /// <param name="onError">The action to invoke if an exception is thrown while applying the <paramref name="transform"/>.</param>
    /// <returns>The instance of <typeparamref name="TResult"/> resulting from the conversion.</returns>
    /// <exception cref="Exception">If an exception occurs while applying the <paramref name="transform"/>, it will be re-thrown after invoking the <paramref name="onError"/> action.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Convert<TSource, TResult>(
        this TSource self,
        Func<TSource, TResult> transform,
        Action<Exception> onError)
    {
        try
        {
            return transform.Invoke(self);
        }
        catch (Exception ex)
        {
            onError.Invoke(ex);
            throw;
        }
    }

    /// <summary>
    /// Converts an instance of <typeparamref name="TSource"/> to an instance of <typeparamref name="TResult"/> by applying the specified transform function, with the ability to provide a fallback function if the transformation fails with a specified exception type <typeparamref name="TException"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object to convert.</typeparam>
    /// <typeparam name="TResult">The type of the result object after conversion.</typeparam>
    /// <typeparam name="TException">The type of the exception that is caught and handled by the fallback function.</typeparam>
    /// <param name="self">The instance of <typeparamref name="TSource"/> to convert.</param>
    /// <param name="transform">The transform function to apply.</param>
    /// <param name="fallback">The fallback function to apply if the transform function fails with an exception of type <typeparamref name="TException"/>.</param>
    /// <returns>The instance of <typeparamref name="TResult"/> resulting from the conversion.</returns>
    /// <remarks>If an exception of type <typeparamref name="TException"/> is caught while executing the transform function, the fallback function is executed instead and its result is returned.</remarks>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Convert<TSource, TResult, TException>(
        this TSource self,
        Func<TSource, TResult> transform,
        Func<TException, TResult> fallback)
    {
        try
        {
            return transform.Invoke(self);
        }
        catch (Exception ex) when (ex is TException targetEx)
        {
            return fallback.Invoke(targetEx);
        }
    }

    /// <summary>
    /// Converts an instance of <typeparamref name="TSource"/> to an instance of <typeparamref name="TResult"/> by applying the specified transform function, or fallback to the specified fallback function if an exception occurs.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object to convert.</typeparam>
    /// <typeparam name="TResult">The type of the result object after conversion.</typeparam>
    /// <param name="self">The instance of <typeparamref name="TSource"/> to convert.</param>
    /// <param name="transform">The transform function to apply.</param>
    /// <param name="fallback">The fallback function to apply if an exception occurs during the transformation.</param>
    /// <returns>The instance of <typeparamref name="TResult"/> resulting from the conversion.</returns>
    /// <remarks>If an exception is thrown during the transformation, the <paramref name="fallback"/> function will be called with the exception as a parameter, and its return value will be used as the result of the conversion.</remarks>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Convert<TSource, TResult>(
        this TSource self,
        Func<TSource, TResult> transform,
        Func<Exception, TResult> fallback)
    {
        try
        {
            return transform.Invoke(self);
        }
        catch (Exception ex) 
        {
            return fallback.Invoke(ex);
        }
    }

    /// <summary>
    /// Creates a CancellationToken that is linked to a set of provided CancellationTokens.
    /// </summary>
    /// <param name="token">The original CancellationToken to which the others will be linked.</param>
    /// <param name="others">An array of CancellationTokens to link to the original token.</param>
    /// <returns>A new CancellationToken linked to the original and other tokens. Cancelling any of the input tokens will result in the returned token being cancelled.</returns>
    /// <remarks>
    /// This extension method allows multiple CancellationTokens to be handled as if they were a single token. 
    /// The CancellationTokenSource created internally is disposed of when the original token is cancelled. 
    /// </remarks>
    [DebuggerNonUserCode]
    public static CancellationToken Join(this CancellationToken token, params CancellationToken[] others)
    {
        var allTokens = new List<CancellationToken> { token };
        allTokens.AddRange(others);
        var source = CancellationTokenSource.CreateLinkedTokenSource(allTokens.ToArray());

        token.Register([DebuggerNonUserCode] () =>
        {
            if (source != null)
            {
                source.Dispose();
                source = null;
            }
        });

        return source.Token;
    }



    /// <summary>
    /// Converts an instance of <typeparamref name="TSource"/> by applying the specified transform <see cref="Action{T}"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the object to convert.</typeparam>
    /// <param name="self">The instance of <typeparamref name="TSource"/> to convert.</param>
    /// <param name="transform">The transform <see cref="Action{T}"/> to apply.</param>
    /// <returns>The converted instance of <typeparamref name="TSource"/>.</returns>
    /// <remarks>The original instance of <typeparamref name="TSource"/> is modified in place by the <paramref name="transform"/> function.</remarks>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Convert<TSource>(this TSource self, Action<TSource> transform)
    {
        transform.Invoke(self);
        return self;
    }

    /// <summary>
    /// Determines and returns the greater value between the current instance and the provided parameter.
    /// </summary>
    /// <param name="self">The current instance of System.TimeSpan.</param>
    /// <param name="threshold">The System.TimeSpan value to compare with the current instance.</param>
    /// <returns>
    /// The greater value between the current instance and the provided System.TimeSpan value.
    /// </returns>
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TimeSpan Min(this TimeSpan self, TimeSpan threshold)
    {
        return self > threshold ? self : threshold;
    }

    /// <summary>
    /// Determines and returns the smaller value between the current instance and the provided parameter.
    /// </summary>
    /// <param name="self">The current instance of System.TimeSpan.</param>
    /// <param name="threshold">The System.TimeSpan value to compare with the current instance.</param>
    /// <returns>
    /// The smaller value between the current instance and the provided System.TimeSpan value.
    /// </returns>
    public static TimeSpan Max(this TimeSpan self, TimeSpan threshold)
    {
        return self < threshold ? self : threshold;
    }

    /// <summary>
    /// Determines and returns the greater value between the current instance and the provided parameter.
    /// </summary>
    /// <param name="self">The current instance of System.DateTime.</param>
    /// <param name="threshold">The System.DateTime value to compare with the current instance.</param>
    /// <returns>
    /// The greater value between the current instance and the provided System.DateTime value.
    /// </returns>
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime Min(this DateTime self, DateTime threshold)
    {
        return self > threshold ? self : threshold;
    }

    /// <summary>
    /// Determines and returns the smaller value between the current instance and the provided parameter.
    /// </summary>
    /// <param name="self">The current instance of System.DateTime.</param>
    /// <param name="threshold">The System.DateTime value to compare with the current instance.</param>
    /// <returns>
    /// The smaller value between the current instance and the provided System.DateTime value.
    /// </returns>
    public static DateTime Max(this DateTime self, DateTime threshold)
    {
        return self < threshold ? self : threshold;
    }

    /// <summary>
    /// Determines and returns the greater value between the current instance and the provided parameter.
    /// </summary>
    /// <param name="self">The current instance of System.DateTimeOffset.</param>
    /// <param name="threshold">The System.DateTimeOffset value to compare with the current instance.</param>
    /// <returns>
    /// The greater value between the current instance and the provided System.DateTimeOffset value.
    /// </returns>
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset Min(this DateTimeOffset self, DateTimeOffset threshold)
    {
        return self > threshold ? self : threshold;
    }

    /// <summary>
    /// Determines and returns the smaller value between the current instance and the provided parameter.
    /// </summary>
    /// <param name="self">The current instance of System.DateTimeOffset.</param>
    /// <param name="threshold">The System.DateTimeOffset value to compare with the current instance.</param>
    /// <returns>
    /// The smaller value between the current instance and the provided System.DateTimeOffset value.
    /// </returns>
    public static DateTimeOffset Max(this DateTimeOffset self, DateTimeOffset threshold)
    {
        return self < threshold ? self : threshold;
    }
}