using Zambon.Core.Module.Xml.Views.DetailViews;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    public class DetailModal : BaseModal
    {

        public DetailView DetailView { get { return View as DetailView; } }
        
    }
}