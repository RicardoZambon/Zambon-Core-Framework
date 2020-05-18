using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.Module.Model.Nodes.Views.Columns;
using Zambon.Core.Module.Model.Nodes.Views.GridTemplates;
using Zambon.Core.Module.Model.Nodes.Views.SearchProperties;

namespace Zambon.Core.Module.Model.Nodes.Views
{
    public sealed class LookupView : LookupViewBase<SearchPropertiesParent, SearchProperty, ColumnsParent, Column, GridTemplatesParent, GridTemplate>
    {
    }
}