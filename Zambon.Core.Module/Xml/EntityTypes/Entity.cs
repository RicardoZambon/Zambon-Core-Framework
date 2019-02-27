using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Zambon.Core.Database;
using Zambon.Core.Database.Entity;

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
        public Property[] Properties { get { return _Properties?.Property; } }

        [XmlElement("Properties"), Browsable(false)]
        public Properties _Properties { get; set; }


        [XmlIgnore]
        private IEntityType EntityType { get; set; }

        [XmlIgnore, Browsable(false)]
        public IDictionary<string, Entity> Navigations { get; set; } = new Dictionary<string, Entity>();


        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            EntityType = ctx.Model.FindEntityType(TypeClr);
            if (EntityType != null)
            {
                var props = EntityType.GetProperties();
                var navs = EntityType.GetNavigations();

                var properties = new Property[0];
                Array.Resize(ref properties, props.Count() + navs.Count(x => x.PropertyInfo != null));

                var pos = 0;
                foreach (var prop in props)
                {
                    properties[pos] = new Property() { Name = prop.Name, DisplayName = prop.PropertyInfo.GetCustomAttribute<DisplayAttribute>()?.Name ?? prop.Name };

                    if (Properties != null && Properties.Length > 0)
                    {
                        var custProperty = Properties.FirstOrDefault(x => x.Name == prop.Name);
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
                        if (Properties != null && Properties.Length > 0)
                        {
                            var custProperty = Properties.FirstOrDefault(x => x.Name == nav.Name);
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
                    _Properties = new Properties() { Property = properties };
                else
                    _Properties.Property = properties;
            }
            else
                throw new Exception($"The ClrType \"{TypeClr}\" for the Entity \"{Id}\" was not found. Please, check your application model.");

            base.OnLoadingXml(app, ctx);
        }

        internal override void OnLoadingUserModel(Application app, CoreDbContext ctx)
        {
            EntityType = ctx.Model.FindEntityType(TypeClr);
            if (EntityType != null)
            {
                var navs = EntityType.GetNavigations();
                foreach (var nav in navs)
                    if (nav.PropertyInfo != null && !Navigations.ContainsKey(nav.Name))
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

            base.OnLoadingUserModel(app, ctx);
        }

        #endregion

        #region Methods

        public string GetDefaultProperty()
        {
            var defaultProperties = EntityType.ClrType.GetCustomAttributes(typeof(DefaultPropertyAttribute), true);
            return defaultProperties.Length > 0 ? ((DefaultPropertyAttribute)defaultProperties[0]).Name : null;
        }

        public string GetPropertyDisplayName(string property)
        {
            if (property.IndexOf(".") >= 0)
            {
                var key = property.Substring(0, property.IndexOf("."));
                var properties = property.Substring(property.IndexOf(".") + 1, property.Length - property.IndexOf(".") - 1);
                return Navigations[key].GetPropertyDisplayName(properties);
            }
            return Array.Find(Properties, x => x.Name == property)?.DisplayName;
        }

        public Type GetEntityType()
        {
            return EntityType.ClrType;
        }

        #endregion

    }
}