
using System;
using System.Data;
using System.Data.Common;

namespace Solitons.Data;

/// <summary>
/// Represents a managed database transaction. This class cannot be inherited.
/// </summary>
/// <remarks>
/// This class encapsulates a DbTransaction that is managed externally, 
/// therefore direct calls to commit or rollback the transaction are not supported.
/// </remarks>
sealed class ManagedDbTransaction : DbTransaction
{
    private readonly DbTransaction _innerTransaction;

    /// <summary>
    /// Initializes a new instance of the ManagedDbTransaction class.
    /// </summary>
    /// <param name="innerTransaction">The DbTransaction to be managed.</param>
    /// <exception cref="ArgumentNullException">Thrown when innerTransaction is null.</exception>
    public ManagedDbTransaction(DbTransaction innerTransaction)
    {
        _innerTransaction = ThrowIf.ArgumentNull(innerTransaction);
        DbConnection = ThrowIf.NullReference(innerTransaction.Connection);
    }

    /// <summary>
    /// Attempts to commits the transaction.
    /// </summary>
    /// <exception cref="NotSupportedException">Thrown always as commit operation is not supported. Transaction is managed externally.</exception>
    public override void Commit()
    {
        throw new NotSupportedException("Commit operation is not supported. Transaction is managed externally.");
    }

    /// <summary>
    /// Attempts to roll back the transaction.
    /// </summary>
    /// <exception cref="NotSupportedException">Thrown always as rollback operation is not supported. Transaction is managed externally.</exception>
    public override void Rollback()
    {
        throw new NotSupportedException("Rollback operation is not supported. Transaction is managed externally.");
    }

    /// <summary>
    /// Gets the DbConnection object associated with the transaction, or null if the transaction is no longer valid.
    /// </summary>
    protected override DbConnection DbConnection { get; }

    /// <summary>
    /// Specifies the IsolationLevel for this transaction.
    /// </summary>
    /// <returns>The IsolationLevel for this transaction.</returns>
    public override IsolationLevel IsolationLevel => _innerTransaction.IsolationLevel;
}