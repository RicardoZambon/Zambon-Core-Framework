using System.ComponentModel;
using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models.Common;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Abstractions
{
    public abstract class ViewResultSetBase<
        TSearchPropertiesParent, TSearchProperty,
        TColumnsParent, TColumn,
        TGridTemplatesParent, TGridTemplate> : ViewBase, IViewResultSet<TSearchPropertiesParent, TSearchProperty, TColumnsParent, TColumn, TGridTemplatesParent, TGridTemplate>
        where TSearchPropertiesParent : SearchPropertiesParentBase<TSearchProperty>
            where TSearchProperty : SearchPropertyBase
        where TColumnsParent : ColumnsParentBase<TColumn>
            where TColumn : ColumnBase
        where TGridTemplatesParent : GridTemplatesParentBase<TGridTemplate>
            where TGridTemplate : GridTemplateBase
    {
        #region XML Attributes

        [XmlAttribute]
        public string Criteria { get; set; }

        [XmlAttribute]
        public string CriteriaArguments { get; set; }

        [XmlAttribute]
        public string Sort { get; set; }

        #endregion

        #region XML Elements

        private TSearchPropertiesParent _searchProperties;
        [XmlElement(nameof(SearchProperties)), Browsable(false)]
        public TSearchPropertiesParent _SearchProperties
        {
            get => _searchProperties;
            set => SetParent(value, ref _searchProperties);
        }

        private TColumnsParent _columns;
        [XmlElement(nameof(Columns)), Browsable(false)]
        public TColumnsParent _Columns
        {
            get => _columns;
            set => SetParent(value, ref _columns);
        }

        private TGridTemplatesParent _gridTemplates;
        [XmlElement(nameof(GridTemplates)), Browsable(false)]
        public TGridTemplatesParent _GridTemplates
        {
            get => _gridTemplates;
            set => SetParent(value, ref _gridTemplates);
        }

        #endregion

        #region Properties

        [XmlIgnore]
        public ChildItemCollection<TSearchProperty> SearchProperties => _SearchProperties?.SearchPropertiesList ?? new ChildItemCollection<TSearchProperty>(null);

        [XmlIgnore]
        public ChildItemCollection<TColumn> Columns => _Columns?.ColumnsList ?? new ChildItemCollection<TColumn>(null);

        [XmlIgnore]
        public ChildItemCollection<TGridTemplate> GridTemplates => _GridTemplates?.GridTemplatesList ?? new ChildItemCollection<TGridTemplate>(null);

        #endregion
    }
}