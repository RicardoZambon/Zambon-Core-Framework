using System;
using System.Linq;
using Zambon.Core.Module.Interfaces;

namespace Zambon.Core.Module.ExtensionMethods
{
    /// <summary>
    /// Extensions used to return icons names.
    /// </summary>
    public static class IconsExtension
    {
        /// <summary>
        /// Will return the complete CSS class to use with the icon.
        /// </summary>
        /// <returns>Return the CSS class as string.</returns>
        public static string GetIconCssClass(this IIcon obj)
        {
            if (!string.IsNullOrWhiteSpace(obj.Icon))
            {
                var classes = obj.Icon.ToLower().Split(" ");

                if (classes.Any(x => x.StartsWith("oi-")))
                {
                    if (classes.Contains("oi"))
                        return obj.Icon;
                    return $"oi {obj.Icon}";
                }
                else if (classes.Any(x => x.StartsWith("fa-")))
                {
                    if (classes.Contains("fas") || classes.Contains("far") || classes.Contains("fal"))
                        return obj.Icon;
                    return $"fas {obj.Icon}";
                }
            }
            return $"fas fa-{obj.Icon}";
        }
    }
}