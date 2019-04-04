using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Zambon.Core.Module.ExtensionMethods;

namespace Zambon.Core.WebModule.Helper
{
    public static class DictionaryExtension
    {

        public static IList<SelectListItem> GetEnumListItems(this Type enumType, string defaultMessage = "", object defaultValue = null)
        {
            var list = new List<SelectListItem>();

            if (defaultMessage != string.Empty)
                list.Add(new SelectListItem { Text = defaultMessage, Value = (defaultValue?.ToString() ?? string.Empty) });

            var enums = Enum.GetValues(enumType).GetEnumerator();
            while(enums.MoveNext())
                list.Add(new SelectListItem { Text = ((Enum)enums.Current).GetEnumDisplayName(), Value = ((Enum)enums.Current).ToString() });

            return list;
        }

        public static IList<SelectListItem> GetListItems(this IDictionary<int, string> dbList, string defaultMessage = "", string defaultValue = "0", int? currentValue = null)
        {
            var list = new List<SelectListItem>();

            if (defaultMessage != string.Empty)
                list.Add(new SelectListItem { Text = defaultMessage, Value = defaultValue });

            foreach (var item in dbList)
                list.Add(new SelectListItem { Text = item.Value, Value = item.Key.ToString(), Selected = currentValue != null && item.Key == currentValue });

            return list;
        }

    }
}