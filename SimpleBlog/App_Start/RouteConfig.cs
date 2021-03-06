﻿using SimpleBlog.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimpleBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var namespaces = new[] { typeof(PostsController).Namespace };

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Read the notes on Post below...
            routes.MapRoute("TagForRealThisTime", "tag/{idAndSlug}", new { controller = "Posts", action = "Tag" },
                namespaces);
            routes.MapRoute("Tag", "tag/{id}-{slug}", new { Controller = "Posts", action = "Tag" }, namespaces);

            // We're making another route for this because ASP.NET, MVC won't put together the post and slug combo for us. It wants everything separated by slashes... So, we're going to coerce it and help it out.
            routes.MapRoute("PostForRealThisTime", "post/{idAndSlug}",
                new { controller = "Posts", action = "Show" }, namespaces);
            routes.MapRoute("Post", "post/{id}-{slug}", new { Controller = "Posts", action = "Show" }, namespaces);

            routes.MapRoute("Login", "login", new { controller = "Auth", action = "Login" }, namespaces);
            routes.MapRoute("Logout", "logout", new { controller = "Auth", action = "Logout" }, namespaces);

            routes.MapRoute("Home", "", new { controller = "Posts", action = "Index" }, namespaces);

            // Order does matter. This has to come after the Home route so the Home route will always match before the Sidebar route (whatever that means)
            routes.MapRoute("Sidebar", "", new {controller = "Layout", action = "Sidebar"}, namespaces);

            routes.MapRoute("Error500", "errors/500", new {controller = "Errors", action = "Error"}, namespaces);
            routes.MapRoute("Error404", "errors/404", new {controller = "Errors", action = "NotFound"}, namespaces);

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
