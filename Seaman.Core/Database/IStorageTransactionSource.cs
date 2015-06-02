namespace Seaman.Core
{
    /// <summary>
    /// Desribes types that require transactions for processing
    /// </summary>
    public interface IStorageTransactionSource
    {
        /// <summary>
        /// Creates new transaction 
        /// </summary>
        IStorageTransaction RequireTransaction();
    }
}