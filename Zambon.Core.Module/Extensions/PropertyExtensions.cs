using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Zambon.Core.Module.Extensions
{
    public static class PropertyExtensions
    {
        public static void GetDisplay(this PropertyInfo property, out string displayName, out string prompt, out string description)
        {
            displayName = null;
            prompt = null;
            description = null;

            if (property.GetCustomAttribute<DisplayAttribute>() is DisplayAttribute displayAttr)
            {
                displayName = displayAttr.Name;
                prompt = displayAttr.Prompt;
                description = displayAttr.Description;
            }

            if (displayName == null && property.GetCustomAttribute<DisplayNameAttribute>() is DisplayNameAttribute displayNameAttr)
            {
                displayName = displayNameAttr.DisplayName;
            }

            if (displayName == null)
            {
                displayName = Regex.Replace(property.Name, "(\\B[A-Z])", " $1");
            }
        }
    }
}