using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seaman.Core;
using Seaman.EntityFramework.Entity;
using Seaman.EntityFramework.EntityConfiguration;
using Seaman.EntityFramework.Migrations;

namespace Seaman.EntityFramework
{
    public class SeamanDbContext : DbContextBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeamanDbContext"/> class.
        /// </summary>
        public SeamanDbContext() :
            this("DefaultConnection")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeamanDbContext"/> class.
        /// </summary>
        /// <param name="connectionName">Name of the connction</param>
        public SeamanDbContext(String connectionName)
            : base(connectionName)
        {

        }

        public static SeamanDbContext Create()
        {
            return new SeamanDbContext();
        }

        public static void SetMigrateToLatestVersionDatabaseInitializer()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SeamanDbContext, Configuration>());
        }

        public IDbSet<User> Users { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<Cane> Canes { get; set; }
        public IDbSet<Canister> Canisters { get; set; }
        public IDbSet<CollectionMethod> CollectionMethods { get; set; }
        public IDbSet<Comment> Comments { get; set; }
        public IDbSet<Location> Locations { get; set; }
        public IDbSet<Physician> Physicians { get; set; }
        public IDbSet<Sample> Samples { get; set; }
        public IDbSet<Tank> Tanks { get; set; }
        public IDbSet<Position> Positions { get; set; } 


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //do not pluralize table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //foreign key field without underscore between table and remote key
            modelBuilder.Conventions.Add<ForeignKeyNamingConvention>();

            modelBuilder.Configurations.Add<User>(new UserConfig());
            modelBuilder.Configurations.Add<Role>(new RoleConfig());
            modelBuilder.Configurations.Add<Sample>(new SampleConfiguration());
            modelBuilder.Configurations.Add<Location>(new LocationConfig());
        }

        protected override void OnTransactionCreated(StorageTransaction transaction)
        {
            base.OnTransactionCreated(transaction);
            transaction.TransactionAbortedExceptionFactory =
                innerException => new SeamanException("Database transaction aborted", innerException);
        }
    }

    public static class SetExtensions
    {
        public static T Get<T>(this IDbSet<T> q, Int32 key, Func<IQueryable<T>, IQueryable<T>> includes,
            String throwIfNotFound = null)
            where T : class, IEntity
        {
            return q.Get(key, throwIfNotFound, includes);
        }

        public static T Get<T>(this IDbSet<T> q, Int32 key, String throwIfNotFound, Func<IQueryable<T>, IQueryable<T>> includes = null)
            where T : class, IEntity
        {
            var result = q.FindLocalOrRemote(it => it.Id == key, includes);
            if (result == null)
            {
                if (throwIfNotFound != null)
                    throw new SeamanNotFoundException(throwIfNotFound);
                return null;
            }
            return result;
        }
    }
}
