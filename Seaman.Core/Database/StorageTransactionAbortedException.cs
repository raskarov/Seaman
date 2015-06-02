using System;
using System.Runtime.Serialization;

namespace Seaman.Core
{
    /// <summary>
    /// Application exception for aborted transactions 
    /// </summary>
    public class StorageTransactionAbortedException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageTransactionAbortedException"/> class.
        /// </summary>
        public StorageTransactionAbortedException()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageTransactionAbortedException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public StorageTransactionAbortedException(String message)
            : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageTransactionAbortedException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public StorageTransactionAbortedException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageTransactionAbortedException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected StorageTransactionAbortedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}