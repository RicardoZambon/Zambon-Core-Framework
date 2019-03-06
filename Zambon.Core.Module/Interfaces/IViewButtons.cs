using System;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Module.Xml.Views.Buttons;

namespace Zambon.Core.Module.Interfaces
{
    /// <summary>
    /// Interface used by Views that implement Buttons.
    /// </summary>
    public interface IViewButtons : IViewControllerAction
    {

        /// <summary>
        /// List all buttons.
        /// </summary>
        Button[] Buttons { get; }

        /// <summary>
        /// The Buttons element from XML.
        /// </summary>
        Buttons _Buttons { get; set; }

    }
}
