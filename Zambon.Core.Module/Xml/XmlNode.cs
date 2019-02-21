using Zambon.Core.Database;
using Zambon.Core.Module.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.Module.Xml
{
    public class XmlNode : IMergeable, ICloneable
    {

        internal virtual void OnLoading(Application app, CoreDbContext ctx)
        {
            var properties = GetType().GetProperties();
            for(var p = 0; p < properties.Length; p++)
            {
                var itemValue = properties[p].GetValue(this);
                if (itemValue != null)
                {
                    if (itemValue is XmlNode)
                        (itemValue as XmlNode).OnLoading(app, ctx);
                    else if (itemValue is Array)
                    {
                        var itemArray = (object[])itemValue;

                        if (itemArray.Length > 0 && (itemArray[0] is XmlNode))
                            for (var a = 0; a < itemArray.Length; a++)
                                (itemArray[a] as XmlNode).OnLoading(app, ctx);
                    }
                }
            }
        }

        public object Clone()
        {
            var newObject = Activator.CreateInstance(GetType());

            var properties = GetType().GetProperties();
            for (var p = 0; p < properties.Length; p++)
            {
                if (properties[p].SetMethod != null)
                    properties[p].SetValue(newObject, properties[p].GetValue(this));
            }

            return newObject;
        }


    }
}