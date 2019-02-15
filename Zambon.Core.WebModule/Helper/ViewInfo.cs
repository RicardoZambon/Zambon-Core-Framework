using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.WebModule.Helper
{
    public class ViewInfo
    {

        public string ViewId { get; set; }

        public string ViewOrigin { get; set; }

        //For ModalViews

        public string ParentViewId { get; set; }

        public string ModalId { get; set; }     
        
        public string ModalTitle { get; set; }

    }
}