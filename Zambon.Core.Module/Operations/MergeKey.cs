using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.Module.Operations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MergeKey : Attribute
    {
    }
}