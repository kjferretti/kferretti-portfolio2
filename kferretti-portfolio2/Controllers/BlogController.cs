using kferretti_portfolio2.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace kferretti_portfolio2.Controllers
{
    [RequireHttps]
    public class BlogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TopicHelper helper = new TopicHelper();

        public ActionResult RoleChange()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            db.Users.Remove(user);
            db.Users.Add(user);
            db.SaveChanges();

            return View(user);
        }

        // GET: Blog/Index
        public ActionResult Index(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(db.BlogPosts.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Search(int? page, string query)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.Query = query;
            var qposts = db.BlogPosts.AsQueryable();
            if (!String.IsNullOrWhiteSpace(query))
            {
                qposts = qposts.Where(p => p.Title.Contains(query) || p.Body.Contains(query) || p.Comments.Any(c => c.Body.Contains(query) || c.Author.DisplayName.Contains(query)));
            }

            return View("Index", qposts.OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
        }

        // GET: Blog/Single
        public ActionResult Single(string slug)
        {
            if (String.IsNullOrWhiteSpace(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost post = db.BlogPosts.FirstOrDefault(p => p.slug == slug);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.Count = post.Comments.Count;
            if (ViewBag.Count == 1)
            {
                ViewBag.Comment = "comment";
            }
            else
            {
                ViewBag.Comment = "comments";
            }

            RelativeTimeHelper relativeTime = new RelativeTimeHelper();

            if (post.Comments != null && post.Comments.Any())
            {
                foreach (var comment in post.Comments)
                {
                    comment.TimeSincePosted = relativeTime.TimeAgo(comment.Created);
                    db.Entry(comment).Property("TimeSincePosted").IsModified = true;
                    db.SaveChanges();
                }
            }


            return View(post);
        }

        // GET: Blog/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Topics = new MultiSelectList(db.Topics, "Id", "Description");

            return View();
        }

        // POST: Blog/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Post, Topics")] BlogTopicsViewModel model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                string slug = StringUtilities.URLFriendly(model.Post.Title);
                if (String.IsNullOrWhiteSpace(slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    ViewBag.Topics = new MultiSelectList(db.Topics, "Id", "Description");
                    return View(model);
                }
                if (db.BlogPosts.Any(p => p.slug == slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    ViewBag.Topics = new MultiSelectList(db.Topics, "Id", "Description");
                    return View(model);
                }
                model.Post.Created = DateTimeOffset.Now;
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var filename = Path.GetFileName(image.FileName);
                    var customName = string.Format(Guid.NewGuid() + filename);
                    image.SaveAs(Path.Combine(Server.MapPath("~/assets/images/uploads/"), customName));
                    model.Post.ImageURL = "~/assets/images/uploads/" + customName;
                }
                else
                {
                    ViewBag.Message = "Please select a valid format image";
                    model.Post.ImageURL = "~/assets/images/post-1.jpg";
                }


                model.Post.slug = slug;
                db.BlogPosts.Add(model.Post);
                db.SaveChanges();

                if (model.Topics != null && model.Topics.Any())
                {
                    foreach (var topic in model.Topics)
                    {
                        int topicId = Convert.ToInt32(topic);
                        helper.AddTopicToBlog(topicId, model.Post.Id);
                    }
                }

                return RedirectToAction("Single", new { slug = model.Post.slug });
            }
            ViewBag.Topics = new MultiSelectList(db.Topics, "Id", "Description");
            return View(model);
        }

        // GET: Blog/Edit
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost post = db.BlogPosts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            BlogTopicsViewModel model = new BlogTopicsViewModel();
            model.Post = post;
            string[] selectedItemIds = new string[post.Topics.Count];
            int i = 0;
            foreach (var topic in post.Topics)
            {
                selectedItemIds[i] = topic.Id.ToString();
                i++;
            }


            ViewBag.Topics = new MultiSelectList(db.Topics, "Id", "Description", selectedItemIds);
            return View(model);
        }

        // POST: Blog/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Post, Topics")] BlogTopicsViewModel model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var blogPost = db.BlogPosts.AsNoTracking().First(p => p.Id == model.Post.Id);
                if (blogPost.Title != model.Post.Title)
                {
                    string slug = StringUtilities.URLFriendly(model.Post.Title);
                    if (String.IsNullOrWhiteSpace(slug))
                    {
                        ModelState.AddModelError("Title", "Invalid title");
                        ViewBag.Topics = new MultiSelectList(db.Topics, "Id", "Description");
                        return View(model);
                    }
                    if (db.BlogPosts.Any(p => p.slug == slug))
                    {
                        ModelState.AddModelError("Title", "The title must be unique");
                        ViewBag.Topics = new MultiSelectList(db.Topics, "Id", "Description");
                        return View(model);
                    }
                }

                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var filePath = Server.MapPath(model.Post.ImageURL);
                    var filePathDefault = Server.MapPath("~/assets/images/post-1.jpg");
                    if (System.IO.File.Exists(filePath) && filePath != filePathDefault)
                    {
                        System.IO.File.Delete(filePath);
                    }
                    var filename = Path.GetFileName(image.FileName);
                    var customName = string.Format(Guid.NewGuid() + filename);
                    image.SaveAs(Path.Combine(Server.MapPath("~/assets/images/uploads/"), customName));
                    model.Post.ImageURL = "~/assets/images/uploads/" + customName;
                }
                else
                {
                    ViewBag.Message = "Please select a valid format image";
                }
                model.Post.Updated = DateTimeOffset.Now;
                db.Entry(model.Post).State = EntityState.Modified;
                db.SaveChanges();
                if (blogPost.Topics != null && blogPost.Topics.Any())
                {
                    foreach (var topic in blogPost.Topics)
                    {
                        helper.RemoveTopicFromBlog(topic.Id, blogPost.Id);
                    }
                }
                if (model.Topics != null && model.Topics.Any())
                {

                    foreach (var topic in model.Topics)
                    {
                        int topicId = Convert.ToInt32(topic);
                        helper.AddTopicToBlog(topicId, model.Post.Id);
                    }
                }
                return RedirectToAction("Single", new { slug = model.Post.slug });
            }

            BlogPost post = db.BlogPosts.Find(model.Post.Id);
            string[] selectedItemIds = new string[post.Topics.Count];
            int i = 0;
            foreach (var topic in post.Topics)
            {
                selectedItemIds[i] = topic.Id.ToString();
                i++;
            }
            ViewBag.Topics = new MultiSelectList(db.Topics, "Id", "Description", selectedItemIds);
            return View(model);
        }

        // GET: Blog/Delete
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost post = db.BlogPosts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Blog/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost post = db.BlogPosts.Find(id);
            var filePath = Server.MapPath(post.ImageURL);
            var filePathDefault = Server.MapPath("~/assets/images/post-1.jpg");
            if (System.IO.File.Exists(filePath) && filePath != filePathDefault)
            {
                System.IO.File.Delete(filePath);
            }
            db.BlogPosts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Comments

        // POST: Blog/CreateComment
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "Id, Body")] Comment comment, int? postId)
        {
            if (postId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost post = db.BlogPosts.Find(postId);
            if (post == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                comment.Created = DateTimeOffset.Now;
                comment.AuthorId = userId;
                comment.BlogPostId = postId ?? default(int);
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            return RedirectToAction("Single", new { slug = post.slug });
        }

        // GET: Blog/EditComment
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult EditComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Blog/EditComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult EditComment([Bind(Include = "Id, Body, Created, AuthorId, UpdateReason, BlogPostId")] Comment comment)
        {
            BlogPost post = db.BlogPosts.Find(comment.BlogPostId);
            if (ModelState.IsValid)
            {
                comment.Updated = DateTimeOffset.Now;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Single", new { slug = post.slug });
        }

        // POST: Blog/DeleteComment
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            BlogPost post = db.BlogPosts.Find(comment.BlogPostId);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Single", new { slug = post.slug });
        }

        // Topics

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}