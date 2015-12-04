using NHibernate.Linq;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;
using SimpleBlog.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SimpleBlog.Controllers
{
    public class PostsController : Controller
    {
        private const int PostsPerPage = 10;

        public ActionResult Index(int page = 1)
        {
/*            throw new Exception("AN ERROR!");*/

            var baseQuery =
                Database.Session.Query<Post>().Where(p => p.DeletedAt == null).OrderByDescending(p => p.CreatedAt);

            var totalPostCount = baseQuery.Count();
            var postIds = baseQuery.Skip((page - 1) * PostsPerPage).Take(PostsPerPage).Select(t => t.Id).ToArray();
            var posts = baseQuery.Where(p => postIds.Contains(p.Id)).FetchMany(p => p.Tags).Fetch(p => p.User).ToList();

            return View(new PostsIndex
            {
                Posts = new PagedData<Post>(posts, totalPostCount, page, PostsPerPage)
            });
        }

        // Says he's implementiong it "between Show and Index because honestly it's a mix between the two"
        public ActionResult Tag(string idAndSlug, int page = 1) // Optional parameters need to come last, so page has to be at the end...
        {
            var parts = SeparateIdAndSlug(idAndSlug);
            if (parts == null)
                return HttpNotFound();

            var tag = Database.Session.Load<Tag>(parts.Item1);
            if (tag == null)
                return HttpNotFound();

            if (!tag.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
                return RedirectToActionPermanent("tag", new { id = parts.Item1, slug = tag.Slug });

            var totalPostCount = tag.Posts.Count();
            var postIds = tag.Posts
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .Where(t => t.DeletedAt == null)
                .Select(t => t.Id)
                .ToArray();

            var posts = Database.Session.Query<Post>()
                .OrderByDescending(p => p.CreatedAt)
                .Where(p => postIds.Contains(p.Id))
                .FetchMany(p => p.Tags)
                .Fetch(p => p.User)
                .ToList();

            return View(new PostsTag
            {
                Tag = tag,
                Posts = new PagedData<Post>(posts, totalPostCount, page, PostsPerPage)
            });
        }

        public ActionResult Show(string idAndSlug)
        {
            var parts = SeparateIdAndSlug(idAndSlug);
            if (parts == null)
                return HttpNotFound();

            var post = Database.Session.Load<Post>(parts.Item1);
            if (post == null || post.IsDeleted)
                return HttpNotFound();

            if (!post.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
                return RedirectToActionPermanent("Show", new { id = parts.Item1, slug = post.Slug });

            return View(new PostsShow
            {
                Post = post
            });
        }

        private System.Tuple<int, string> SeparateIdAndSlug(string idAndSlug)
        {
            var matches = Regex.Match(idAndSlug, @"^(\d+)\-(.*)?$");
            if (!matches.Success)
                return null;

            var id = int.Parse(matches.Result("$1"));
            var slug = matches.Result("$2");
            return Tuple.Create(id, slug);
        }
    }
}