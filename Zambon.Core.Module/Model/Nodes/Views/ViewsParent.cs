using Zambon.Core.Module.Model.Abstractions;
using Zambon.Core.Module.Model.Nodes.Views.Buttons;
using Zambon.Core.Module.Model.Nodes.Views.Columns;
using Zambon.Core.Module.Model.Nodes.Views.GridTemplates;
using Zambon.Core.Module.Model.Nodes.Views.SearchProperties;

namespace Zambon.Core.Module.Model.Nodes.Views
{
    public sealed class ViewsParent : ViewsParentBase
        <DetailView, ListView, LookupView,
            SearchPropertiesParent, SearchProperty,
            ButtonsParent, Button,
            ColumnsParent, Column,
            GridTemplatesParent, GridTemplate>
    {
        public ViewsParent() : base()
        {

        }
    }
}