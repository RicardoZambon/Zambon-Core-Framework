using System;

namespace Zambon.Core.Module.Atrributes
{
    /// <summary>
    /// Indicates the attribute should be used when merging models to detect same records in both files.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MergeAttribute : Attribute
    {
    }
}