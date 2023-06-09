﻿using System;
using System.Data;

namespace StackExchange.Profiling.Data
{
    /// <summary>
    /// A general implementation of <see cref="IDbTransaction"/> that is used to
    /// wrap profiling information around calls to it.
    /// </summary>
    public class SimpleProfiledTransaction : IDbTransaction
    {
        private readonly SimpleProfiledConnection _connection;

        /// <summary>
        /// Creates a new wrapped <see cref="IDbTransaction"/>
        /// </summary>
        /// <param name="transaction">The transaction to wrap.</param>
        /// <param name="connection">The already-wrapped connection.</param>
        /// <exception cref="ArgumentNullException">Throws when the <paramref name="transaction"/> or <paramref name="connection"/> is <c>null</c>.</exception>
        public SimpleProfiledTransaction(IDbTransaction transaction, SimpleProfiledConnection connection)
        {
            WrappedTransaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Gets the internal wrapped transaction.
        /// </summary>
        public IDbTransaction WrappedTransaction { get; }

        /// <inheritdoc cref="IDbTransaction.Connection"/>
        public IDbConnection Connection => _connection;

        /// <inheritdoc cref="IDbTransaction.IsolationLevel"/>
        public IsolationLevel IsolationLevel => WrappedTransaction.IsolationLevel;

        /// <inheritdoc cref="IDbTransaction.Commit()"/>
        public void Commit() => WrappedTransaction.Commit();

        /// <inheritdoc cref="IDbTransaction.Rollback()"/>
        public void Rollback() => WrappedTransaction.Rollback();

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="IDbTransaction"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="IDbTransaction"/>.
        /// </summary>
        /// <param name="disposing">false if being called from a <c>finalizer</c></param>
        private void Dispose(bool disposing)
        {
            if (disposing) WrappedTransaction?.Dispose();
        }
    }
}
