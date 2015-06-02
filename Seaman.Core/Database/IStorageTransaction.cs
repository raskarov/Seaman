using System;

namespace Seaman.Core
{
    /// <summary>
    /// Describes storage transaction
    /// </summary>
    public interface IStorageTransaction : IDisposable
    {
        /// <summary>
        /// Commits current transaction.
        /// </summary>
        void Commit();
        /// <summary>
        /// Occurs after transaction commited.
        /// </summary>
        event EventHandler AfterCommit;
        /// <summary>
        /// Occurs after transaction rolled back.
        /// </summary>
        event EventHandler AfterRollback;
    }
}