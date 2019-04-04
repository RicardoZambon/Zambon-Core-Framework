using Castle.DynamicProxy;
using System;

namespace Zambon.Core.Database.ChangeTracker.Extensions
{
    /// <summary>
    /// Helper methods to use when having Castle.Proxies entities.
    /// </summary>
    public static class ProxyExtension
    {
        /// <summary>
        /// Get the object type without the proxy.
        /// </summary>
        /// <param name="entity">The entity instance.</param>
        /// <returns>Returns the original type of the entity.</returns>
        public static Type GetUnproxiedType(this object entity)
            => ProxyUtil.IsProxy(entity) ? ProxyUtil.GetUnproxiedType(entity) : entity.GetType();

        /// <summary>
        /// Get the object type without the proxy.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>Returns the original type.</returns>
        public static Type GetUnproxiedType(this Type type)
            => ProxyUtil.IsProxyType(type) ? ProxyUtil.GetUnproxiedType(type) : type;
    }
}