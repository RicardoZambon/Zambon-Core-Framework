using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Zambon.Core.Module.Operations
{
    public static class Merge
    {

        public static TObject MergeObject<TObject>(TObject target, TObject source) where TObject : IMergeable
        {
            var properties = target.GetType().GetProperties();
            for (var i = 0; i < properties.Length; i++)
            {
                if (properties[i].SetMethod != null)
                {
                    var targetValue = properties[i].GetValue(target);
                    var sourceValue = properties[i].GetValue(source);

                    if (targetValue is IMergeable)
                    {
                        if (sourceValue != null)
                        {
                            targetValue = typeof(Merge).GetMethod("MergeObject").MakeGenericMethod(targetValue.GetType()).Invoke(null, new object[] { targetValue, sourceValue });
                            properties[i].SetValue(target, targetValue);
                        }
                    }
                    else if (targetValue is Array)
                    {
                        targetValue = typeof(Merge).GetMethod("MergeArray").MakeGenericMethod(targetValue.GetType().GetElementType()).Invoke(null, new object[] { targetValue, sourceValue });
                        properties[i].SetValue(target, targetValue);
                    }
                    else
                    {
                        if (targetValue == null || (!targetValue.Equals(sourceValue) && sourceValue != null))
                            properties[i].SetValue(target, sourceValue);
                    }
                }
            }
            return target;
        }

        public static TObject[] MergeArray<TObject>(TObject[] target, TObject[] source) where TObject : class
        {
            if (source != null && source.Length > 0)
            {
                if (target == null || target.Length == 0)
                    return source;
                else if (!(target[0] is IMergeable))
                {
                    //Add all elements from source into target
                    var pos = target.Length;
                    Array.Resize(ref target, target.Length + source.Length);
                    for (var s = 0; s < source.Length; s++)
                    {
                        target[pos] = source[s];
                        pos++;
                    }
                }
                else
                {
                    var key = string.Empty;
                    var originalLength = target.Length;

                    var newItems = new List<TObject>();
                    for (var s = 0; s < source.Length; s++)
                        newItems.Add(source[s]);

                    for (var t = 0; t < originalLength; t++)
                    {
                        var targetItem = target[t];

                        if (key == string.Empty)
                            key = targetItem.GetType().GetProperties().FirstOrDefault(x => x.CustomAttributes.Where(a => a.AttributeType == typeof(MergeKey)).Count() > 0).Name;

                        var targetKeyValue = targetItem.GetType().GetProperty(key).GetValue(targetItem);

                        //Compare elements that exists in both arrays
                        for (var s = 0; s < newItems.Count; s++)
                        {
                            var sourceItem = newItems[s];
                            var sourceKeyValue = sourceItem.GetType().GetProperty(key).GetValue(sourceItem);

                            if (targetKeyValue.Equals(sourceKeyValue))
                            {
                                s--;
                                newItems.Remove(sourceItem);
                                typeof(Merge).GetMethod("MergeObject").MakeGenericMethod(targetItem.GetType()).Invoke(null, new object[] { targetItem, sourceItem });
                            }
                        }

                        //Add new elements to original array
                        if (t + 1 == originalLength && newItems.Count > 0)
                        {
                            var pos = originalLength;
                            Array.Resize(ref target, originalLength + newItems.Count);
                            foreach (var item in newItems)
                            {
                                target[pos] = item;
                                pos++;
                            }
                        }
                    } 
                }
            }
            return target;
        }

    }
}
