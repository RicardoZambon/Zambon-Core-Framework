using Zambon.Core.Module.Xml.Views.DetailViews;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Zambon.Core.Module.Xml.Views.SubViews
{
    public class DetailModal : BaseModal
    {

        public DetailView DetailView { get { return View as DetailView; } }
        
    }
}