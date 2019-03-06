using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Module.Xml.Views.SubViews;

namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interface used from Views that implement SubViews.
    /// </summary>
    public interface IViewSubViews
    {

        /// <summary>
        /// The SubViews element from XML.
        /// </summary>
        SubViews SubViews { get; set; }

        /// <summary>
        /// Retrieves a SubView using the SubView Id.
        /// </summary>
        /// <param name="Id">The Id of the SubView.</param>
        /// <returns>If found, return the SubView instance; Otherwise, return null.</returns>
        BaseSubView GetSubView(string Id);

    }
}
