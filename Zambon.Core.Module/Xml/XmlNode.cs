using System;
using System.IO;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Xml
{
    /// <summary>
    /// Base class used for all XML nodes from Application Model.
    /// </summary>
    public abstract class XmlNode : IMergeable, ICloneable
    {
        [XmlIgnore]
        private bool IsLoaded { get; set; }

        internal virtual void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            if (IsLoaded)
                return;

            IsLoaded = true; 
            var properties = GetType().GetProperties();
            for (var p = 0; p < properties.Length; p++)
            {
                var itemValue = properties[p].GetValue(this);
                if (itemValue != null)
                {
                    if (itemValue is XmlNode itemValueXmlNode)
                        itemValueXmlNode.OnLoadingXml(app, ctx);
                    else if (itemValue is XmlNode[] itemArray)
                    {
                        if (itemArray.Length > 0)
                            for (var a = 0; a < itemArray.Length; a++)
                                itemArray[a].OnLoadingXml(app, ctx);
                    }
                }
            }
        }

        internal virtual void OnLoadingUserModel(Application app, CoreDbContext ctx)
        {
            if (IsLoaded)
                return;

            IsLoaded = true;
            var properties = GetType().GetProperties();
            for (var p = 0; p < properties.Length; p++)
            {
                var itemValue = properties[p].GetValue(this);
                if (itemValue != null)
                {
                    if (itemValue is XmlNode itemValueXmlNode)
                        itemValueXmlNode.OnLoadingUserModel(app, ctx);
                    else if (itemValue is XmlNode[] itemArray)
                    {
                        if (itemArray.Length > 0)
                            for (var a = 0; a < itemArray.Length; a++)
                                itemArray[a].OnLoadingUserModel(app, ctx);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a new instance of the same object.
        /// </summary>
        /// <returns>Returns a new instance of the same object with the same values.</returns>
        public object Clone()
        {
            using (var memoryStream = new MemoryStream())
            {
                var xmlSerializer = new XmlSerializer(GetType());
                xmlSerializer.Serialize(memoryStream, this);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return xmlSerializer.Deserialize(memoryStream);
            }
        }
    }
}