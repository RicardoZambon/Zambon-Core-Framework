using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models.Common
{
    public interface IViewResultSet<
        TSearchPropertiesParent, TSearchProperty,
        TColumnsParent, TColumn,
        TGridTemplatesParent, TGridTemplate>
        where TSearchPropertiesParent : ISearchPropertiesParent<TSearchProperty>
            where TSearchProperty : ISearchProperty
        where TColumnsParent : IColumnsParent<TColumn>
            where TColumn : IColumn
        where TGridTemplatesParent : IGridTemplatesParent<TGridTemplate>
            where TGridTemplate : IGridTemplate
    {
        string Criteria { get; set; }

        string CriteriaArguments { get; set; }

        string Sort { get; set; }

        TSearchPropertiesParent _SearchProperties { get; set; }

        TColumnsParent _Columns { get; set; }

        TGridTemplatesParent _GridTemplates { get; set; }

        
        ChildItemCollection<TSearchProperty> SearchProperties { get; }

        ChildItemCollection<TColumn> Columns { get; }

        ChildItemCollection<TGridTemplate> GridTemplates { get; }
    }
}