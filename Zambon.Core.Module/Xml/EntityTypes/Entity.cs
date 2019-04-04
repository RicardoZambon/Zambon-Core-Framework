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
using Zambon.Core.Database.Domain.Extensions;
using Zambon.Core.Database.Domain.Interfaces;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.Xml.EntityTypes
{
    /// <summary>
    /// Represents entities used in CoreDbContext. IEntity or IQuery.
    /// </summary>
    public class Entity : XmlNode, IIcon
    {
        /// <summary>
        /// The Id of the entity type, used to merge same elements across ApplicationModels.
        /// </summary>
        [XmlAttribute("Id"), MergeKey]
        public string Id { get; set; }

        /// <summary>
        /// Display name of the entity type.
        /// </summary>
        [XmlAttribute("DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Singular name of each entity type.
        /// </summary>
        [XmlAttribute("SingularName")]
        public string SingularName { get; set; }

        /// <summary>
        /// Icon of the entity type.
        /// </summary>
        [XmlAttribute("Icon")]
        public string Icon { get; set; }

        /// <summary>
        /// Default controller name.
        /// </summary>
        [XmlAttribute("DefaultController")]
        public string DefaultController { get; set; }

        /// <summary>
        /// If this entity should be returned directly from the database.
        /// </summary>
        [XmlAttribute("FromSql")]
        public string FromSql { get; set; }

        /// <summary>
        /// The CLR type of the entity type, with the full type name.
        /// </summary>
        [XmlAttribute("TypeClr")]
        public string TypeClr { get; set; }

        /// <summary>
        /// Properties from the entity type.
        /// </summary>
        [XmlIgnore]
        public Property[] Properties { get { return _Properties?.Property; } }

        /// <summary>
        /// Properties from the entity type.
        /// </summary>
        [XmlElement("Properties"), Browsable(false)]
        public Properties _Properties { get; set; }


        /// <summary>
        /// The entity type from Entity Framework.
        /// </summary>
        [XmlIgnore]
        private IEntityType EntityType { get; set; }

        /// <summary>
        /// Navigations from the entity type (ICollections, IList, etc.).
        /// </summary>
        [XmlIgnore, Browsable(false)]
        public IDictionary<string, Entity> Navigations { get; set; } = new Dictionary<string, Entity>();


        #region Overrides

        internal override void OnLoadingXml(Application app, CoreDbContext ctx)
        {
            EntityType = ctx.Model.FindEntityType(TypeClr);
            if (EntityType != null)
            {
                if (string.IsNullOrWhiteSpace(SingularName))
                    SingularName = DisplayName;

                var props = EntityType.GetProperties();
                var navs = EntityType.GetNavigations();

                var properties = new Property[0];
                Array.Resize(ref properties, props.Count() + navs.Count(x => x.PropertyInfo != null));

                var pos = 0;
                foreach (var prop in props)
                {
                    properties[pos] = new Property() { Name = prop.Name, DisplayName = prop.PropertyInfo?.GetCustomAttribute<DisplayAttribute>()?.Name ?? prop.Name };

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
                            if (navClrType.ImplementsInterface<IEntity>() || navClrType.ImplementsInterface<IQuery>())
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
                        if (navClrType.ImplementsInterface<IEntity>() || navClrType.ImplementsInterface<IQuery>())
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

        /// <summary>
        /// Returns the default property set in DefaultPropertyAttribute. string.Empty if no property were informed.
        /// </summary>
        /// <returns></returns>
        public string GetDefaultProperty()
        {
            var defaultProperties = EntityType.ClrType.GetCustomAttributes(typeof(DefaultPropertyAttribute), true);
            return defaultProperties.Length > 0 ? ((DefaultPropertyAttribute)defaultProperties[0]).Name : null;
        }

        /// <summary>
        /// Get the display name of a property.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        /// <returns>Returns the display name in ApplicationModel, if not found any, from the DisplayAttribut, if not found, will return the property.</returns>
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

        /// <summary>
        /// Get the CLR type object from the entity type.
        /// </summary>
        /// <returns>Returns the type object.</returns>
        public Type GetEntityType()
        {
            return EntityType.ClrType;
        }

        #endregion
    }
}