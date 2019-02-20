using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Zambon.Core.Database;
using Zambon.Core.Database.Entity;
using Zambon.Core.Module.Interfaces;
using Zambon.Core.Module.Operations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.EntityTypes
{
    public class Entity : XmlNode
    {

        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        [XmlAttribute("Icon")]
        public string Icon { get; set; }

        [XmlAttribute("DefaultController")]
        public string DefaultController { get; set; }

        [XmlAttribute("FromSql")]
        public string FromSql { get; set; }

        [XmlAttribute("TypeClr")]
        public string TypeClr { get; set; }

        [XmlIgnore]
        private Type EntityType { get; set; }


        [XmlElement("Properties")]
        public Properties Properties { get; set; }

        [XmlIgnore]
        public IDictionary<string, Entity> Navigations { get; set; } = new Dictionary<string, Entity>();

        #region Overrides

        internal override void OnLoading(Application app, CoreContext ctx)
        {
            var entityType = ctx.Model.GetEntityTypes(TypeClr).FirstOrDefault();
            if (entityType != null)
            {
                EntityType = entityType.ClrType;

                var props = entityType.GetRuntimeProperties();
                var navs = entityType.GetNavigations();

                var properties = new Property[0];
                Array.Resize(ref properties, props.Count() + navs.Count(x => x.PropertyInfo != null));

                var pos = 0;
                foreach (var prop in props)
                {
                    properties[pos] = new Property() { Name = prop.Value.Name, DisplayName = prop.Value.GetCustomAttribute<DisplayAttribute>()?.Name ?? prop.Value.Name };

                    if (Properties != null && Properties.Property != null && Properties.Property.Length > 0)
                    {
                        var custProperty = Properties.Property.FirstOrDefault(x => x.Name == prop.Value.Name);
                        if (custProperty != null)
                            properties[pos].DisplayName = custProperty.DisplayName;
                    }
                    pos++;
                }

                foreach (var nav in navs)
                {
                    if (nav.PropertyInfo != null)
                    {
                        properties[pos] = new Property() { Name = nav.Name, DisplayName = nav.PropertyInfo.GetCustomAttribute<DisplayAttribute>()?.Name ?? nav.Name };
                        if (Properties != null && Properties.Property != null && Properties.Property.Length > 0)
                        {
                            var custProperty = Properties.Property.FirstOrDefault(x => x.Name == nav.Name);
                            if (custProperty != null)
                                properties[pos].DisplayName = custProperty.DisplayName;
                        }
                        pos++;

                        if (!Navigations.ContainsKey(nav.Name))
                        {
                            var navClrType = nav.ClrType.GenericTypeArguments.Length > 0 ? nav.ClrType.GenericTypeArguments[0] : nav.ClrType;
                            if (navClrType.IsSubclassOf(typeof(BaseDBObject)))
                            {
                                var entity = app.FindEntityByClrType(navClrType.FullName) ?? app.FindEntityById(navClrType.Name);
                                if (entity != null)
                                    Navigations.Add(nav.Name, entity);
                            }
                        }
                    }
                }

                if (Properties == null)
                    Properties = new Properties() { Property = properties };
                else
                    Properties.Property = properties;
            }
            else
                throw new Exception($"The ClrType \"{TypeClr}\" was not found. Check the application model.");

            base.OnLoading(app, ctx);
        }

        #endregion

        #region Methods

        public Property GetProperty(string name)
        {
            if (name.IndexOf(".") >= 0)
            {
                var key = name.Substring(0, name.IndexOf("."));
                var properties = name.Substring(name.IndexOf(".") + 1, name.Length - name.IndexOf(".") - 1);
                return Navigations[key].GetProperty(properties);
            }
            return Properties?.Property?.FirstOrDefault(x => x.Name == name);
        }

        public Type GetEntityType()
        {
            return EntityType;
        }

        #endregion

    }
}