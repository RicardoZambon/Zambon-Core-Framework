using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Interfaces.Models.Common
{
    public interface IViewResultSet<TSearchProperty,TColumn,TGridTemplate>
        where TSearchProperty : ISearchProperty
        where TColumn : IColumn
        where TGridTemplate : IGridTemplate
    {
        string Criteria { get; set; }

        string CriteriaArguments { get; set; }

        string Sort { get; set; }

        ChildItemCollection<TSearchProperty> SearchProperties { get; set; }

        ChildItemCollection<TColumn> Columns { get; set; }

        ChildItemCollection<TGridTemplate> GridTemplates { get; set; }
    }
}