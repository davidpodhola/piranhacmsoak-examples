using Piranha;
using Piranha.Models;
using Piranha.Web;
using Piranha.WebPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApplication6
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void _RenderLevelStart(UIHelper ui, StringBuilder str, string cssclass)
        {
        }
        protected void _RenderLevelEnd(UIHelper ui, StringBuilder str, string cssclass)
        {
        }

        protected void _RenderItemStart(Piranha.Web.UIHelper ui, StringBuilder str, Piranha.Models.Sitemap page, bool active, bool activechild)
        {
        }
        protected void _RenderItemEnd(Piranha.Web.UIHelper ui, StringBuilder str, Piranha.Models.Sitemap page, bool active, bool activechild)
        {
        }

        private string Url(string virtualpath)
        {
            var request = HttpContext.Current.Request;
            return virtualpath.Replace("~/", request.ApplicationPath + (request.ApplicationPath != "/" ? "/" : ""));
        }
        
        private string GenerateUrl(ISitemap page)
        {
            if (page != null)
            {
                if (!String.IsNullOrEmpty(page.Redirect))
                {
                    if (page.Redirect.Contains("://"))
                        return page.Redirect;
                    else if (page.Redirect.StartsWith("~/"))
                        return Url(page.Redirect);
                }
                if (page.IsStartpage)
                    return Url("~/");
                return Url("~/" + (!Config.PrefixlessPermalinks ?
                    Piranha.Application.Current.Handlers.GetUrlPrefix("PERMALINK").ToLower() + "/" : "") + page.Permalink.ToLower());
            }
            return "";
        }


        protected void _RenderItemLink(UIHelper ui, StringBuilder str, Piranha.Models.Sitemap page)
        {
            str.AppendLine(String.Format("<a class=\"element\" href=\"{0}\">{1}</a>", GenerateUrl(page),
                        !String.IsNullOrEmpty(page.NavigationTitle) ? page.NavigationTitle : page.Title));
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Hooks.Menu.RenderLevelStart += new Piranha.Delegates.MenuLevelHook(_RenderLevelStart);
            Hooks.Menu.RenderLevelEnd += new Piranha.Delegates.MenuLevelHook(_RenderLevelEnd);
            Hooks.Menu.RenderItemStart += new Piranha.Delegates.MenuItemHook(_RenderItemStart);
            Hooks.Menu.RenderItemEnd += new Piranha.Delegates.MenuItemHook(_RenderItemEnd);
            Hooks.Menu.RenderItemLink += new Piranha.Delegates.MenuItemLinkHook(_RenderItemLink);
        }
    }
}