using Zambon.Core.Module.Model.Nodes.Views.Columns;
using Zambon.Core.Module.Model.Nodes.Views.SearchProperties;
using Zambon.Core.WebModule.Model.Abstractions;
using Zambon.Core.WebModule.Model.Nodes.Views.Buttons;
using Zambon.Core.WebModule.Model.Nodes.Views.GridTemplates;

namespace Zambon.Core.WebModule.Model.Nodes.Views
{
    public sealed class WebListView : WebListViewBase<SearchPropertiesParent, SearchProperty, WebButtonsParent, WebButton, ColumnsParent, Column, WebGridTemplatesParent, WebGridTemplate>
    {
    }
}