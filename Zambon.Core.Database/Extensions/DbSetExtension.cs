using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Zambon.Core.Database.Domain.Interfaces;

namespace Zambon.Core.Database.Extensions
{
    /// <summary>
    /// Extension methods to manipulate database objects.
    /// </summary>
    public static class DbSetExtension
    {
        /// <summary>
        /// Validate the an entity.
        /// </summary>
        /// <typeparam name="T">Type of the entity.</typeparam>
        /// <param name="entity">The entity instance.</param>
        /// <param name="serviceProvider">Service provider instance.</param>
        /// <param name="items">The error list.</param>
        /// <returns>Returns true if no errors were found.</returns>
        public static bool IsValid<T>(this T entity, IServiceProvider serviceProvider, out IDictionary<object, object> items) where T : ICustomValidatableObject
        {
            items = entity.ValidateData(new ValidationContext(entity, serviceProvider, new Dictionary<object, object>()));
            return items.Any();
        }
    }
}