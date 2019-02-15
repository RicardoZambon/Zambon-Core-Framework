using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zambon.Core.WebModule.Helper
{
    public static class CurrentTabHelper
    {

        public static string GetCurrentTab(this ITempDataDictionary tempData, string defaultTab, params string[] availableTabs)
        {
            if (tempData["CurrentTabId"] is string currentTab)
            {
                if (Array.Exists(availableTabs, x => x.ToLower() == currentTab.ToLower()))
                    return currentTab;
            }
            return defaultTab;
        }

    }
}
