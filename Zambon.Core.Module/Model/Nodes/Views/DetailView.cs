using System.Xml.Serialization;
using Zambon.Core.Module.Interfaces.Models;
using Zambon.Core.Module.Model.Nodes.Views.Buttons;
using Zambon.Core.Module.Model.Serialization;

namespace Zambon.Core.Module.Model.Nodes.Views
{
    public class DetailView<TButton> : ViewBase, IDetailView<TButton> where TButton : class, IButton<TButton>
    {
        #region XML Arrays

        [XmlArray, XmlArrayItem(nameof(Button))]
        public ChildItemCollection<TButton> Buttons { get; set; }

        #endregion

        #region Constructors

        public DetailView()
        {
            Buttons = new ChildItemCollection<TButton>(this);
        }

        #endregion
    }
}