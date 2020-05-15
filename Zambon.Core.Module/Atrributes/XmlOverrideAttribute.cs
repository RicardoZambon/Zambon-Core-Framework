using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.Module.Atrributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class XmlOverrideAttribute : Attribute
    {
    }
}
