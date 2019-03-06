using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.Module.Interfaces
{
    public interface IViewControllerAction
    {

        /// <summary>
        /// Define the default controller name to be used within this view, by default will use the same as set in EntityType.
        /// </summary>
        string ControllerName { get; set; }

        /// <summary>
        /// Define the action name to be used for this view.
        /// </summary>
        string ActionName { get; set; }

    }
}
