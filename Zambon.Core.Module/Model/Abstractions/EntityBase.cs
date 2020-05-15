using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Atrributes;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class EntityBase<TPropertiesParent, TProperty> : SerializeNodeBase, IEntity<TPropertiesParent, TProperty>
        where TPropertiesParent : IPropertiesParent<TProperty>
            where TProperty : IProperty
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

        #region XML Elements

        [XmlElement(nameof(Properties)), Browsable(false)]
        public TPropertiesParent _Properties { get; set; }

        #endregion

        #region Properties

        [XmlIgnore]
        public ChildItemCollection<TProperty> Properties => _Properties?.PropertiesList ?? new ChildItemCollection<TProperty>(null);

        #endregion
    }
}