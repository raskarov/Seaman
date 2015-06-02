using System.Data.Common;

namespace Seaman.Core
{
    /// <summary>
    /// Describes unit of work for storages
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Tries to save the changes into storage.
        /// </summary>
        void SaveChanges();
    }
}