using System;
using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Extensions;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.EntityTypes
{
    public class Entity<TProperty> : SerializeNodeBase, IEntity<TProperty> where TProperty : class, IProperty
    {
        #region XML Attributes

        [XmlAttribute, MergeKey]
        public string Id { get; set; }

        [XmlAttribute]
        public string DisplayName { get; set; }

        [XmlAttribute]
        public string SingularName { get; set; }

        [XmlAttribute]
        public string Icon { get; set; }

        [XmlAttribute]
        public string FromSql { get; set; }

        [XmlAttribute]
        public string FromSqlParameters { get; set; }

        [XmlAttribute]
        public string TypeClr { get; set; }

        #endregion

        #region XML Arrays

        [XmlArray, XmlArrayItem(nameof(Property))]
        public ChildItemCollection<TProperty> Properties { get; set; }

        #endregion

        #region Constructors

        public Entity()
        {
            Properties = new ChildItemCollection<TProperty>(this);
        }

        public Entity(Microsoft.EntityFrameworkCore.Metadata.IEntityType dbEntity) : this()
        {
            Id = dbEntity.ClrType.Name;
            DisplayName = dbEntity.ClrType.GetDisplayName();

            foreach (var dbProperty in dbEntity.GetProperties())
            {
                Properties.Add((TProperty)Activator.CreateInstance(typeof(TProperty), new object[] { dbProperty }));
            }
        }

        #endregion
    }
}