using System;
using System.Data;
using System.Runtime.ExceptionServices;

namespace Seaman.Core
{
    public class StorageTransaction : IStorageTransaction
    {
        #region Constructors

        public StorageTransaction()
        {
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomainOnFirstChanceException;
        }
        public StorageTransaction(IDbTransaction transaction)
            : this()
        {
            _transaction = transaction;
        }
        public StorageTransaction(IStorageTransaction transaction)
            : this()
        {
            _parentTransaction = transaction;
        }

        #endregion

        #region Public

        public Func<Exception, Exception> TransactionAbortedExceptionFactory { get; set; }

        public Boolean DoNotThrowException
        {
            get { return _doNotThrowException; } 
            set { _doNotThrowException = value; }
        }

        public void Commit()
        {
            if (_transaction != null)
                _transaction.Commit();
            _commited = true;
            if (_parentTransaction == null)
                OnCommit();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public event EventHandler Disposed;

        public event EventHandler AfterCommit
        {
            add
            {
                if (_parentTransaction != null)
                    _parentTransaction.AfterCommit += value;
                else
                    _afterCommit = (EventHandler)Delegate.Combine(value, _afterCommit);

            }
            remove
            {
                if (_parentTransaction != null)
                    _parentTransaction.AfterCommit -= value;
                else
                    _afterCommit -= value;
            }
        }

        public event EventHandler AfterRollback
        {
            add
            {
                if (_parentTransaction != null)
                    _parentTransaction.AfterRollback += value;
                else
                    _afterRollback = (EventHandler)Delegate.Combine(value, _afterRollback);

            }
            remove
            {
                if (_parentTransaction != null)
                    _parentTransaction.AfterRollback -= value;
                else
                    _afterRollback -= value;
            }
        }


        #endregion
        

        #region Protected

        protected virtual void OnRollback()
        {
            EventHandler temp = _afterRollback;
            if (temp != null)
                temp(this, EventArgs.Empty);
        }

        protected virtual void OnCommit()
        {
            EventHandler temp = _afterCommit;
            if (temp != null)
                temp(this, EventArgs.Empty);
        }

        protected virtual void Dispose(Boolean disposing)
        {
            if (_disposed)
                return;
            _disposed = true;
            if (disposing)
            {
                try
                {
                    if (!_commited)
                    {
                        if (_parentTransaction == null)
                        {
                            OnRollback();
                        }
                        if (!_doNotThrowException)
                            throw TransactionAbortedExceptionFactory != null ?
                                TransactionAbortedExceptionFactory(_outerException) :
                                new StorageTransactionAbortedException("Storage transaction is aborted", _outerException);
                    }
                }
                finally
                {
                    var tr = _transaction as IDisposable;
                    if (tr != null)
                        tr.Dispose();
                    var disposedHandler = Disposed;
                    if (disposedHandler != null)
                    {
                        disposedHandler(this, EventArgs.Empty);
                    }
                }
            }

        }

        #endregion

        #region Private

        private Boolean _disposed;
        private Boolean _commited;
        private IStorageTransaction _parentTransaction;
        private event EventHandler _afterRollback;
        private event EventHandler _afterCommit;
        private IDbTransaction _transaction;
        private Boolean _doNotThrowException;
        private Exception _outerException;
        private void CurrentDomainOnFirstChanceException(object sender, FirstChanceExceptionEventArgs firstChanceExceptionEventArgs)
        {
            //_doNotThrowException = true;
            _outerException = firstChanceExceptionEventArgs.Exception;
            AppDomain.CurrentDomain.FirstChanceException -= CurrentDomainOnFirstChanceException;
        }
        #endregion
    }
}