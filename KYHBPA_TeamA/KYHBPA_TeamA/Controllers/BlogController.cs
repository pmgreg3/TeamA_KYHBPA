using KYHBPA_TeamA.Models;
using KYHBPA_TeamA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Net;



namespace KYHBPA_TeamA.Controllers
{
    public class BlogController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private BlogRepository _blogRepository = new BlogRepository();

        public ViewResult Index(int p = 1)
        {
            // pick latest 10 posts
            var posts = _blogRepository.Posts(p - 1, 10);

            var totalPosts = _blogRepository.TotalPosts();

            var listViewModel = new BlogListViewModel()
            {
                Posts = posts,
                TotalPosts = totalPosts
            };

            ViewBag.Title = "Latest Posts";

            return View("List", listViewModel);
        }

        //// GET: Blog
        //public ActionResult Index()
        //{
        //    //Grab 10 blog posts.
        //    var blogPosts = _db.Posts.Take(10);

        //    return View(blogPosts);
        //}

        public ActionResult Manage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateBlogPostViewModel() { PostedOn = DateTime.Today.Date });
        }

        [HttpPost]
        public ActionResult Create(CreateBlogPostViewModel viewModel)
        {

            if (ModelState.IsValid)
            {

                var category = _db.Categories.FirstOrDefault(c => c.Id == viewModel.SelectedCategoryId);
                var post = new Post();

                //Incase the selected category isn't in the database yet..
                if (category != null)
                    post.Category = category;
                else
                {
                    var selectedCategoryName = viewModel.CategoryList.ToList().FirstOrDefault(item => int.Parse(item.Value) == viewModel.SelectedCategoryId).Text;
                    _db.Categories.Add(new Category() { Name = selectedCategoryName });
                    _db.SaveChanges();

                    var categoryRecord = _db.Categories.FirstOrDefault(c => c.Id == viewModel.SelectedCategoryId);
                    post.Category = categoryRecord;
                }


                post.Description = viewModel.Description;
                post.ShortDescription = viewModel.ShortDescription;
                post.Modified = viewModel.Modified;
                post.Published = viewModel.Published;
                post.PostedOn = viewModel.PostedOn;
                post.Tags = viewModel.Tags;
                post.Title = viewModel.Title;

                _db.Posts.Add(post);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(viewModel);
            }
        }


        public ActionResult Edit(int id)
        {
            var post = _db.Posts.FirstOrDefault(p => p.Id == id);

            var viewModel = new CreateBlogPostViewModel()
            {
                Id = post.Id,
                Description = post.Description,
                ShortDescription = post.ShortDescription,
                Category = post.Category,
                Modified = post.Modified,
                PostedOn = post.PostedOn,
                Published = post.Published,
                Title = post.Title,
                //Could lead to possible bug if seed order doesn't stay the same.
                SelectedCategoryId = post.Category.Id
            };

            return View(viewModel);
        }


        public ActionResult Details(int id)
        {
            var post = _db.Posts.FirstOrDefault(p => p.Id == id);

            var viewModel = new BlogPostDetailsViewModel()
            {
                Id = post.Id,
                Description = post.Description,
                ShortDescription = post.ShortDescription,
                Category = post.Category,
                Modified = post.Modified,
                PostedOn = post.PostedOn,
                Published = post.Published,
                Title = post.Title,
                Comments = post.Comments,
                //Could lead to possible bug if seed order doesn't stay the same.
                SelectedCategoryId = post.Category.Id
            };

            return View(viewModel);
        }

        public ActionResult ViewByCategory(int Id)
        {
            // pick latest 10 posts by category
            var posts = _blogRepository.Posts(0, 10).Where(p => p.Category.Id == Id).ToList();

            var totalPosts = _blogRepository.TotalPosts();

            var listViewModel = new BlogListViewModel()
            {
                Posts = posts,
                TotalPosts = totalPosts
            };

            ViewBag.Title = "Latest Posts by Category";

            return View("List", listViewModel);
        }

        [HttpGet]
        public ActionResult CreateComment(int Id)
        {
            var post = _db.Posts.FirstOrDefault(p => p.Id == Id);

            var viewModel = new CreateCommentViewModel()
            {
                Post = post,
                PostNumber = post.Id
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateComment(CreateCommentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var post = _db.Posts.FirstOrDefault(p => p.Id == viewModel.PostNumber);

                var comment = new Comment()
                {
                    Name = viewModel.Name,
                    Text = viewModel.Comment,
                    Post = post
                };

                _db.Comments.Add(comment);
                _db.SaveChanges();

                post.Comments.Add(comment);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}