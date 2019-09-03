using System;

namespace Zambon.Core.Module.Xml
{
    /// <summary>
    /// Indicates the attribute should be used when merging to detect same records in both files.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MergeKeyAttribute : Attribute
    {
    }
}