using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using Zambon.Core.Database.Domain.Extensions;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Xml;

namespace Zambon.Core.Module.Extensions
{
    /// <summary>
    /// Extension method used to merge two ApplicationModel XML files.
    /// </summary>
    public static class MergeExtension
    {
        /// <summary>
        /// Merges two objects that implements the interface IMergeable.
        /// </summary>
        /// <typeparam name="TObject">The element type of both objects.</typeparam>
        /// <param name="target">The target object, that will have the items with null values.</param>
        /// <param name="source">The source object, containing the original items values.</param>
        public static void Merge<TObject>(this TObject target, TObject source) where TObject : class, IMergeable
        {
            if (source != null)
            {
                var properties = source.GetType().GetProperties();
                for (var i = 0; i < properties.Length; i++)
                {
                    if (properties[i].SetMethod != null)
                    {
                        var sourceValue = properties[i].GetValue(source);
                        var targetValue = properties[i].GetValue(target);

                        if (properties[i].PropertyType.ImplementsInterface<IMergeable>())
                        {
                            if (sourceValue != null)
                            {
                                if (targetValue == null)
                                {
                                    targetValue = Activator.CreateInstance(properties[i].PropertyType);
                                }
                                (targetValue as IMergeable).Merge(sourceValue as IMergeable);
                                properties[i].SetValue(target, targetValue);
                            }
                        }
                        else if (properties[i].PropertyType.IsArray && properties[i].PropertyType.GetElementType() is Type elementType && elementType.ImplementsInterface<IMergeable>())
                        {
                            var arrayValue = typeof(MergeExtension).GetMethod(nameof(MergeElements)).MakeGenericMethod(properties[i].PropertyType.GetElementType()).Invoke(null, new object[] { targetValue, sourceValue });
                            properties[i].SetValue(target, arrayValue);
                        }
                        else if (targetValue == null && sourceValue != null)
                        {
                            properties[i].SetValue(target, sourceValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Merges two arrays of objects that implements the interface IMergeable.
        /// </summary>
        /// <typeparam name="TObject">The element type of the array.</typeparam>
        /// <param name="target">The target array, that will have the items with null values.</param>
        /// <param name="source">The source array, containing the original items values.</param>
        /// <returns>Returns the target array plus all any new elements from the source array.</returns>
        public static TObject[] MergeElements<TObject>(this TObject[] target, TObject[] source) where TObject : class, IMergeable
        {
            if ((source?.Length ?? 0) > 0)
            {
                if (target == null)
                {
                    target = new TObject[0];
                }

                var nonExistentItems = new List<TObject>();

                var keyProperty = typeof(TObject).GetProperties().FirstOrDefault(x => x.CustomAttributes.Any(a => a.AttributeType == typeof(MergeKeyAttribute)));
                if (keyProperty == null)
                {
                    nonExistentItems.AddRange(source);
                }
                else
                {
                    var targetKeys = target.Select(x => keyProperty.GetValue(x));
                    for (var s = 0; s < source.Length; s++)
                    {
                        var sourceKey = keyProperty.GetValue(source[s]);
                        if (targetKeys.Contains(sourceKey))
                        {
                            target[targetKeys.IndexOf(sourceKey)].Merge(source[s]);
                        }
                        else
                        {
                            var newItem = Activator.CreateInstance<TObject>();
                            newItem.Merge(source[s]);
                            nonExistentItems.Add(newItem);
                        }
                    }
                }
                
                if (nonExistentItems.Count > 0)
                {
                    target = target.Union(nonExistentItems).ToArray();
                }
            }
            return target;
        }
    }
}