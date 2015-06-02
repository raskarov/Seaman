using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Seaman.EntityFramework
{
    /// <summary>
    /// Provides a convention for fixing the independent association (IA) foreign key column names.
    /// </summary>
    public class ForeignKeyNamingConvention : IStoreModelConvention<AssociationType>
    {
        /// <summary>
        /// Applies the specified association.
        /// </summary>
        /// <param name="association">The association.</param>
        /// <param name="model">The model.</param>
        public void Apply(AssociationType association, DbModel model)
        {
            // Identify a ForeignKey properties (including IAs)
            if (association.IsForeignKey)
            {
                // rename FK columns
                var constraint = association.Constraint;
                if (DoPropertiesHaveDefaultNames(constraint.FromProperties, constraint.ToRole.Name, constraint.ToProperties))
                {
                    NormalizeForeignKeyProperties(constraint.FromProperties);
                }
                if (DoPropertiesHaveDefaultNames(constraint.ToProperties, constraint.FromRole.Name, constraint.FromProperties))
                {
                    NormalizeForeignKeyProperties(constraint.ToProperties);
                }
            }
        }
        /// <summary>
        /// Does the properties have default names.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="otherEndProperties">The other end properties.</param>
        /// <returns></returns>
        private Boolean DoPropertiesHaveDefaultNames(ReadOnlyMetadataCollection<EdmProperty> properties, String roleName, IReadOnlyList<EdmProperty> otherEndProperties)
        {
            if (properties.Count != otherEndProperties.Count)
            {
                return false;
            }

            return !properties.Where((t, i) => !t.Name.EndsWith("_" + otherEndProperties[i].Name)).Any();
        }

        /// <summary>
        /// Normalizes the foreign key properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        private void NormalizeForeignKeyProperties(IEnumerable<EdmProperty> properties)
        {
            foreach (var edmProperty in properties)
            {
                var defaultPropertyName = edmProperty.Name;
                var ichUnderscore = defaultPropertyName.IndexOf('_');
                if (ichUnderscore <= 0)
                {
                    continue;
                }
                var navigationPropertyName = defaultPropertyName.Substring(0, ichUnderscore);
                var targetKey = defaultPropertyName.Substring(ichUnderscore + 1);

                string newPropertyName;
                if (targetKey.StartsWith(navigationPropertyName))
                {
                    newPropertyName = targetKey;
                }
                else
                {
                    newPropertyName = navigationPropertyName + targetKey;
                }
                edmProperty.Name = newPropertyName;
            }
        }
    }
}