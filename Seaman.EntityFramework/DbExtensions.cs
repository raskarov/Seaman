using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Seaman.EntityFramework
{
    /// <summary>
    /// Helper functions for ef
    /// </summary>
    public static class DbExtensions
    {
        /// <summary>
        /// Creates the entity and adds it to the storage (within current transaction).
        /// </summary>
        /// <typeparam name="T">Type of the entity to create</typeparam>
        /// <returns>Newly created entity</returns>
        public static T CreateAndAdd<T>(this IDbSet<T> set)
            where T : class
        {
            var entity = set.Create();
            set.Add(entity);
            return entity;
        }
        /// <summary>
        /// Creates the entity and adds it to the storage (within current transaction).
        /// </summary>
        /// <typeparam name="T">Type of the entity for storage set</typeparam>
        /// <typeparam name="TD">Type of the entity to create</typeparam>
        /// <returns>Newly created entity</returns>
        public static TD CreateAndAdd<T, TD>(this IDbSet<T> set)
            where T : class
            where TD : class, T
        {
            var entity = set.Create<TD>();
            set.Add(entity);
            return entity;
        }
        /// <summary>
        /// Creates the entity and adds it to the storage (within current transaction).
        /// </summary>
        /// <typeparam name="T">Type of the entity to create</typeparam>
        /// <returns>Newly created entity</returns>
        public static T CreateAndAdd<T>(this DbContext context)
            where T : class
        {
            var set = context.Set<T>();
            return set.CreateAndAdd();
        }

        /// <summary>
        /// Get local entity or get from db if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set"></param>
        /// <param name="queryExpression"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public static T FindLocalOrRemote<T>(this IDbSet<T> set, Expression<Func<T, Boolean>> queryExpression, Func<IQueryable<T>,IQueryable<T>> includes = null) 
            where T : class
        {
            var entity = set.Local.FirstOrDefault(queryExpression.Compile());
            if (entity != null)
                return entity;

            var q = set.Where(queryExpression);
            if (includes != null)
                q = includes(q);
            return  q.FirstOrDefault(queryExpression);
        }

        #region date as utc

        /// <summary>
        /// Contains mapping between types and their properties of <see cref="DateTime"/> type for each context
        /// </summary>
        private static readonly Dictionary<object, Dictionary<Type, List<PropertyInfo>>> DateProperties = new Dictionary<Object, Dictionary<Type, List<PropertyInfo>>>();
        /// <summary>
        /// Registers handler that should read all date values as UTC
        /// </summary>
        /// <param name="context">The context.</param>
        public static void ReadAllDateTimeValuesAsUtc(this DbContext context)
        {
            ((IObjectContextAdapter)context).ObjectContext.ObjectMaterialized += (sender, e) => ReadDateTimeValuesAsUtc(sender, e.Entity, null);
        }
        /// <summary>
        /// Registers handler that should read all date values in UTC for properties with Utc suffix in the name
        /// </summary>
        /// <param name="context">The context.</param>
        public static void ReadSuffixedDateTimeValuesAsUtc(this DbContext context)
        {
            ((IObjectContextAdapter)context).ObjectContext.ObjectMaterialized += (sender, e) => ReadDateTimeValuesAsUtc(sender, e.Entity, p => p.Name.EndsWith("Utc", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Converts datetime values for provided entity to UTC
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="filter">The filter for property names.</param>
        private static void ReadDateTimeValuesAsUtc(Object sender, Object entity, Func<PropertyInfo, Boolean> filter)
        {
            var type = entity.GetType();

            //Extract all DateTime properties of the object type
            List<PropertyInfo> props;
            Dictionary<Type, List<PropertyInfo>> dateProps;
            if (!DateProperties.TryGetValue(sender, out dateProps))
            {
                dateProps = new Dictionary<Type, List<PropertyInfo>>();
                DateProperties.Add(sender, dateProps);
            }
            if (!dateProps.TryGetValue(type, out props))
            {
                var q = type.GetTypeInfo().GetProperties().Where(property => property.PropertyType == typeof(DateTime) ||
                                                                             property.PropertyType == typeof(DateTime?));
                if (filter != null)
                    q = q.Where(filter);
                props = q.ToList();
                dateProps[type] = props;
            }

            //Set all DaetTimeKinds to Utc
            props.ForEach(property => SpecifyUtcKind(property, entity));
        }

        /// <summary>
        /// Specifies the <see cref="DateTimeKind"/> values for values provided
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        private static void SpecifyUtcKind(PropertyInfo property, Object value)
        {
            //Get the datetime value
            var datetime = property.GetValue(value, null);

            //set DateTimeKind to Utc
            if (property.PropertyType == typeof(DateTime))
            {
                datetime = DateTime.SpecifyKind((DateTime)datetime, DateTimeKind.Utc);
            }
            else if (property.PropertyType == typeof(DateTime?))
            {
                var nullable = (DateTime?)datetime;
                if (!nullable.HasValue) return;
                datetime = (DateTime?)DateTime.SpecifyKind(nullable.Value, DateTimeKind.Utc);
            }
            else
            {
                return;
            }

            //And set the Utc DateTime value
            property.SetValue(value, datetime, null);
        }

        #endregion
    }
}