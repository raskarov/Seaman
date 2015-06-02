using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using Seaman.Core;

namespace Seaman.EntityFramework
{
    /// <summary>
    /// Base class for storage contexts
    /// </summary>
    public abstract class DbContextBase : DbContext, IStorageTransactionSource, IUnitOfWork
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBase"/> class.
        /// </summary>
        /// <param name="objectContext">An existing ObjectContext to wrap with the new context.</param>
        /// <param name="dbContextOwnsObjectContext">If set to <c>true</c> the ObjectContext is disposed when the DbContext is disposed, otherwise the caller must dispose the connection.</param>
        protected DbContextBase(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBase"/> class.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="model">The model that will back this context.</param>
        /// <param name="contextOwnsConnection">If set to <c>true</c> the connection is disposed when the context is disposed, otherwise the caller must dispose the connection.</param>
        protected DbContextBase(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBase"/> class.
        /// </summary>
        /// <param name="existingConnection">An existing connection to use for the new context.</param>
        /// <param name="contextOwnsConnection">If set to <c>true</c> the connection is disposed when the context is disposed, otherwise the caller must dispose the connection.</param>
        protected DbContextBase(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBase"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        /// <param name="model">The model that will back this context.</param>
        protected DbContextBase(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBase"/> class.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        protected DbContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBase"/> class.
        /// </summary>
        /// <param name="model">The model that will back this context.</param>
        protected DbContextBase(DbCompiledModel model)
            : base(model)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBase"/> class.
        /// </summary>
        protected DbContextBase()
        {
        }

        #endregion


        
        /// <summary>
        /// Called when new transaction was created
        /// </summary>
        /// <param name="transaction">The new transaction.</param>
        protected virtual void OnTransactionCreated(StorageTransaction transaction)
        {

        }
        /// <summary>
        /// Creates new transaction
        /// </summary>
        /// <returns></returns>
        public virtual IStorageTransaction RequireTransaction()
        {
            var prev = Transactions.LastOrDefault();
            var tr = prev == null
                ? new StorageTransaction(this.Database.BeginTransaction().UnderlyingTransaction)
                : new StorageTransaction(prev);
            tr.Disposed += TransactionDisposed;
            Transactions.Add(tr);
            OnTransactionCreated(tr);
            return tr;
        }

        /// <summary>
        /// Sets the on validate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback">The callback.</param>
        public void SetOnValidate<T>(Func<DbEntityEntry, IDictionary<object, object>, DbEntityValidationResult> callback)
        {
            _validationSubscriptions.Add(new ValidationSubscription { EntityType = typeof(T), Callback = callback });
        }
        /// <summary>
        /// Resets the on validate.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public void ResetOnValidate(Func<DbEntityEntry, IDictionary<Object, Object>, DbEntityValidationResult> callback)
        {
            _validationSubscriptions.RemoveAll(it => it.Callback == callback);
        }

        /// <summary>
        /// Reverts not saved changes
        /// </summary>
        public virtual void UndoChanges()
        {
            foreach (DbEntityEntry entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        #region Protected

        /// <summary>
        /// Disposes the context. The underlying <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" /> is also disposed if it was created
        /// is by this context or ownership was passed to this context when this context was created.
        /// The connection to the database (<see cref="T:System.Data.Common.DbConnection" /> object) is also disposed if it was created
        /// is by this context or ownership was passed to this context when this context was created.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _validationSubscriptions.Clear();
            base.Dispose(disposing);
        }


        /// <summary>
        /// List of current transactions
        /// </summary>
        protected readonly List<IStorageTransaction> Transactions = new List<IStorageTransaction>();
        #endregion


        #region Private

        private class ValidationSubscription
        {
            public Type EntityType { get; set; }
            public Func<DbEntityEntry, IDictionary<object, object>, DbEntityValidationResult> Callback { get; set; }
        }

        private readonly List<ValidationSubscription> _validationSubscriptions = new List<ValidationSubscription>();

        /// <summary>
        /// Handles TransactionDisposed method for context
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TransactionDisposed(Object sender, EventArgs e)
        {
            var i = Transactions.IndexOf(sender as IStorageTransaction);
            if (i >= 0)
                Transactions.RemoveRange(i, Transactions.Count - i);
        }
        #endregion

        void IUnitOfWork.SaveChanges()
        {
            SaveChanges();
        }
    }
}