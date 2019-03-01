using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Zambon.Core.Database;
using Zambon.Core.Module;
using Zambon.Core.Module.Services;
using Zambon.Core.Module.Xml.Navigation;
using Zambon.Core.Module.Xml.Views.ListViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zambon.Core.WebModule.TagHelpers
{
    [HtmlTargetElement("ul", Attributes = ForAttributeName)]
    public class MenuTagHelper : TagHelper
    {
        private const string ForAttributeName = "menu-display";

        public override int Order => -1001;

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        protected ApplicationService App { get; }

        protected IOptions<ApplicationConfigs> AppConfigs { get; }

        protected CoreDbContext Ctx { get; }


        public MenuTagHelper(IHtmlGenerator generator, ApplicationService app, IOptions<ApplicationConfigs> appConfigs, CoreDbContext ctx)
        {
            Generator = generator;
            AppConfigs = appConfigs;
            App = app;
            Ctx = ctx;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (output == null) throw new ArgumentNullException(nameof(output));

            var menus = App.UserMenu;
            for(var i = 0; i < menus.Length; i++)
            {
                var menu = menus[i];

                var li = new TagBuilder("li");
                li.AddCssClass("nav-item");

                var a = CreateLink(menu, "nav-link");
                li.InnerHtml.AppendHtml(a.RenderStartTag());
                li.InnerHtml.AppendHtml(a.RenderBody());
                li.InnerHtml.AppendHtml(a.RenderEndTag());

                if ((menu.SubMenus?.Length ?? 0) > 0)
                {
                    li.AddCssClass("dropdown");

                    var div = CreateMenuDiv(menu);
                    li.InnerHtml.AppendHtml(div.RenderStartTag());
                    li.InnerHtml.AppendHtml(div.RenderBody());
                    li.InnerHtml.AppendHtml(div.RenderEndTag());
                }

                output.Content.AppendHtml(li.RenderStartTag());
                output.Content.AppendHtml(li.RenderBody());
                output.Content.AppendHtml(li.RenderEndTag());
            }
        }

        private TagBuilder CreateMenuDiv(Menu menu)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("dropdown-menu");
            if ((menu.SubMenus?.Length ?? 0) > 0)
                for (var i = 0; i < menu.SubMenus.Length; i++)
                {
                    var subMenu = menu.SubMenus[i];
                    var a = CreateLink(subMenu, "dropdown-item");
                    div.InnerHtml.AppendHtml(a.RenderStartTag());
                    div.InnerHtml.AppendHtml(a.RenderBody());
                    div.InnerHtml.AppendHtml(a.RenderEndTag());
                }
            return div;
        }

        private TagBuilder CreateLink(Menu menu, string _class)
        {
            TagBuilder a = null;
            switch (menu.Type)
            {
                case "Separator":
                    var separator = new TagBuilder("div");
                    separator.AddCssClass("dropdown-divider");
                    return separator;

                case "DetailView":
                    a = Generator.GenerateActionLink(
                        ViewContext,
                        "",
                        menu.ActionName,
                        menu.ControllerName,
                        //"GetDetail",
                        //"View",
                        "",
                        "",
                        "",
                        new { ViewId = menu.ViewID, ViewOrigin = "Menu" },
                        new { });

                    a.Attributes.Add("data-ajax", "true");
                    a.Attributes.Add("data-ajax-method", "POST");
                    a.Attributes.Add("data-ajax-mode", "replace");
                    a.Attributes.Add("data-ajax-update", "#bodyContent");
                    a.Attributes.Add("data-ajax-update-loading", "body");

                    a.Attributes.Add("data-ajax-begin", "AjaxBegin");
                    a.Attributes.Add("data-ajax-failure", "AjaxFailure");
                    a.Attributes.Add("data-ajax-complete", "AjaxComplete");
                    break;

                case "ListView":
                    a = Generator.GenerateActionLink(
                        ViewContext,
                        "",
                        "ListView",
                        "View",
                        "",
                        "",
                        "",
                        new { ViewId = menu.ViewID, ViewOrigin = "Menu" },
                        new { });

                    a.Attributes.Add("data-ajax", "true");
                    a.Attributes.Add("data-ajax-method", "POST");
                    a.Attributes.Add("data-ajax-mode", "replace");
                    a.Attributes.Add("data-ajax-update", "#bodyContent");
                    a.Attributes.Add("data-ajax-update-loading", "body");

                    a.Attributes.Add("data-ajax-begin", "AjaxBegin");
                    a.Attributes.Add("data-ajax-failure", "AjaxFailure");
                    a.Attributes.Add("data-ajax-complete", "AjaxComplete");
                    break;

                default:
                    a = new TagBuilder("a");
                    switch (menu.Type)
                    {
                        case "Report":
                            a.Attributes.Add("href", string.Format("{0}/ReportServer/Pages/ReportViewer.aspx?{1}/{2}", AppConfigs.Value.ReportsURLServer, AppConfigs.Value.ReportsFolderBasePath, menu.ReportPath));
                            break;
                        case "ExternalURL":
                            a.Attributes.Add("href", menu.URL);
                            break;
                    }
                    a.Attributes.Add("target", "_blank");
                    break;
            }
            a.AddCssClass(_class);

            if (!string.IsNullOrWhiteSpace(menu.Icon))
            {
                var icon = new TagBuilder("span");
                icon.AddCssClass("oi");
                icon.AddCssClass(menu.Icon);
                icon.AddCssClass("mr-1");
                a.InnerHtml.AppendHtml(icon.RenderStartTag());
                a.InnerHtml.AppendHtml(icon.RenderEndTag());
            }

            var displayName = new TagBuilder("span");
            displayName.AddCssClass("display-name");
            displayName.InnerHtml.Append(menu.DisplayName);
            a.InnerHtml.AppendHtml(displayName.RenderStartTag());
            a.InnerHtml.AppendHtml(displayName.RenderBody());
            a.InnerHtml.AppendHtml(displayName.RenderEndTag());

            if (menu.ShowBadge ?? false)
            {
                var badge = new TagBuilder("span");
                badge.AddCssClass("badge");
                badge.AddCssClass("badge-pill");
                badge.AddCssClass("ml-1");

                var badgeCount = 0;
                if (!string.IsNullOrWhiteSpace(menu.ViewID) && menu.View is ListView)
                {
                    a.Attributes.Add("menu-has-badge", menu.ViewID);
                    badgeCount = ((ListView)menu.View).GetItemsCount(Ctx, App);
                }
                else if ((menu.SubMenus?.Length ?? 0) > 0 && menu.SubMenus.FirstOrDefault(x => x.ShowBadge ?? false) is Menu menuBadge && menuBadge.View is ListView)
                {
                    a.Attributes.Add("menu-has-badge", menuBadge.ViewID);
                    badgeCount = ((ListView)menuBadge.View).GetItemsCount(Ctx, App);
                }

                badge.AddCssClass(badgeCount > 0 ? "badge-warning" : _class == "nav-link" ? "badge-light" : "badge-secondary");
                //badge.AddCssClass(badgeCount > 0 ? "badge-warning" : _class == "nav-link" ? "badge-light" : "badge-secondary");
                badge.InnerHtml.Append(badgeCount.ToString());

                a.InnerHtml.AppendHtml(badge.RenderStartTag());
                a.InnerHtml.AppendHtml(badge.RenderBody());
                a.InnerHtml.AppendHtml(badge.RenderEndTag());
            }

            if ((menu.SubMenus?.Length ?? 0) > 0)
            {
                a.AddCssClass("dropdown-toggle");
                a.Attributes.Add("id", string.Format("menu_{0}", menu.Id));
                a.Attributes.Add("data-toggle", "dropdown");
                a.Attributes.Add("aria-haspopup", "true");
                a.Attributes.Add("aria-expanded", "false");
            }

            return a;
        }
        
    }
}