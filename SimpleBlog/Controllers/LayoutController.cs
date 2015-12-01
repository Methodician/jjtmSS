using System.Linq;
using System.Web.Mvc;
using NHibernate.Linq;
using SimpleBlog.Models;
using SimpleBlog.ViewModels;

namespace SimpleBlog.Controllers
{
    public class LayoutController : Controller
    {
        // He says he's only putting this ChildActionOnly filter in so it can't be accessed directly via Url (baseurl/sidebar) but then we're also not including a url in RouteConfig for same reason.
        // Seems like this would only be used in situations where you still wanted to give it a url of its own.
        // But alass he insists that you should still always use this ChildActionOnly filter
        [ChildActionOnly] 
        public ActionResult Sidebar()
        {
            // When you need data in a view, you should pretty much always create a view model...
            return View( new LayoutSidebar
            {
                IsLoggedIn = Auth.User != null,
                Username = Auth.User != null ? Auth.User.Username : "",
                IsAdmin = User.IsInRole("admin"),
                Tags = Database.Session.Query<Tag>().Select(tag => new
                {
                    tag.Id,
                    tag.Name,
                    tag.Slug,
                    PostCount = tag.Posts.Count
                }).Where(t => t.PostCount > 0).OrderByDescending(p => p.PostCount).Select(
                    tag => new SidebarTag(tag.Id, tag.Name, tag.Slug, tag.PostCount)).ToList()
            });
        }
    }
}