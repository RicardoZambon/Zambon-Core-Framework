using System;

namespace Zambon.Core.Module.Xml
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MergeKeyAttribute : Attribute
    {
    }
}