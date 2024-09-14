using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum20.Data;
using Forum20.Data.Models;
using Forum20.Models;
using Forum20.Models.Home;
using Forum20.Models.Post;
using Forum20.Models.Reply;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Forum20.Controllers
{
   
    public class PostController : Controller
    {
        private readonly IPost _postService;
        private readonly IForum _forumService;
        private readonly IApplicationUser _userService;
        private readonly ApplicationDbContext _context;

        private static UserManager<ApplicationUser> _userManager;
        public PostController(IPost postService,IForum forumService,UserManager<ApplicationUser> userManager,IApplicationUser userService,ApplicationDbContext context)
        {
            _postService = postService;
            _forumService = forumService;
            _userManager = userManager;
            _userService = userService;
            _context = context;
        }
       public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);
            var replies = BuildPostReplies(post.Replies);
            var model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImageUrl,
                AuthorRating = post.User.Rating,
                IsAuthorAdmin = IsAuthorAdmin(post.User),
                 Created = post.Created,
                 PostContent=post.Content,
               // PostContent = _postFormatter.Prettify(post.Content),
                Replies = replies,
                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title
            };
            return View(model);
        } 

        private bool IsAuthorAdmin(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user).Result.Contains("Admin");
        }

        [Authorize]
        public IActionResult Create(int id)
        {
            //Fotum.id
            var forum = _forumService.GetById(id);

            var model = new NewPostModel
            {
                ForumName=forum.Title,
                ForumId=forum.Id,
                ForumImageUrl=forum.ImageUrl,
                AuthorName=User.Identity.Name
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewPostModel model)
        {
           
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var user = _userManager.FindByIdAsync(userId).Result;
                var post = BuildPost(model, user);

                await _postService.Add(post);
                await _userService.UpdateUserRating(userId, typeof(Post));
                //Implementing User Mnagement
                return RedirectToAction("Index", "Post", new { id = post.Id });
            }
            else
            return View(model);
        }

        private Post BuildPost(NewPostModel model, ApplicationUser user)
        {
            
                var forum = _forumService.GetById(model.ForumId);
                return new Post
                {
                    Title = model.Title,
                    Content = model.Content,
                    Created = DateTime.Now,
                    Status="Pending",
                    User = user,
                    Forum = forum
                };
            
            
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies)
        {
            return replies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                Created = reply.Created,
                ReplyContent = reply.Content,
                IsAuthorAdmin = IsAuthorAdmin(reply.User)
            });
        }

        [Authorize]
        public async Task<IActionResult> AllPosts(int page=1)
        {
            // var model = BuildHomeIndex();
            var latestPosts =_context.Posts.AsNoTracking()
                .Include(post => post.User)
                 .Include(post => post.Replies).ThenInclude(reply => reply.User)
                 .Include(post => post.Forum)
                 .OrderBy(post => post.Id);
            //  var model = await PagingList.CreateAsync(model, 5, page);
            var model = await PagingList.CreateAsync(latestPosts, 1, page);
           
            model.Action = "AllPosts";
            return View(model);
        }
        
        private HomeIndexModel BuildHomeIndex()
        {
            var latestPosts = _postService.GetAll();

            var posts = latestPosts.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.User.UserName,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = GetForumListingForPost(post)
                // ForumImageUrl = _postService.GetForumImageUrl(post.Id),
                //  ForumId = post.Forum.Id
            });

            return new HomeIndexModel
            {
                LatestPosts = posts,
                SearchQuery = ""
            };
        }
        private ForumListingModel GetForumListingForPost(Post post)
        {
            var forum = post.Forum;

            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                ImageUrl = forum.ImageUrl
            };
        }

     //   [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            var post = _postService.GetById(id);
           // var forum = _forumService.GetById(post.Forum.Id);

            var model = new NewPostModel
            {
                Id= post.Id,
                AuthorName=post.User.UserName,
                ForumId=post.Forum.Id,
                ForumName=post.Forum.Title,
                ForumImageUrl=post.Forum.ImageUrl,
                Title = post.Title,
                Content = post.Content,
                Created = post.Created,
            };

            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult>  Edit(NewPostModel model,int id)
        {
            if (ModelState.IsValid)
            {
                await _postService.EditPostContent(model.Id, model.Content);
                return RedirectToAction("Index", "Post", new { id = model.Id });
            }
            else
            {
                var post = _postService.GetById(id);
                var model1= new NewPostModel
                {
                    ForumName = post.Forum.Title,
                    ForumImageUrl = post.Forum.ImageUrl
                };
                return View(model1);
            }
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var post = _postService.GetById(id);
           // _postService.Delete(id);
            var model = new DeletePostModel
            {
                PostId = post.Id,
                PostTitle=post.Title,
                PostAuthor = post.User.UserName,
                PostContent = post.Content,
                ForumId=post.Forum.Id
            };
           // return RedirectToAction("Index", "Forum", new { id = post.Forum.Id });
            return View(model);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(DeletePostModel model, int id)
        {
            var post = _postService.GetById(id);
            await _postService.DeleteReply(id);
           await _postService.Delete(id);

            return RedirectToAction("Topic", "Forum", new { id = post.Forum.Id });
        }

        public IActionResult Index1()
        {
            //var posts = _context.Posts.OrderBy(post => post.Id);
            // var model = BuildHomeIndex();
            /*   var latestPosts = _context.Posts.AsNoTracking()
                   .Where(post => post.Id == id)
                   .Include(post => post.User)
                    .Include(post => post.Replies).ThenInclude(reply => reply.User)
                    .Include(post => post.Forum)
                    .OrderBy(post => post.Id);
               //  var model = await PagingList.CreateAsync(model, 5, page);
               var model = await PagingList.CreateAsync(latestPosts, 1, page);

               model.Action = "Index1";
               return View(model);*/
            var model = BuildHomeIndex();
            return View(model);
        }

        public JsonResult GetPost()
        {
            var posts = _context.Posts;
            // return Json(new { data = posts }, JsonRequestBehavior.AllowGet);
            return Json(posts, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [Authorize]
        public IActionResult PostList(string id)
        {
            // var userId = _userManager.GetUserId(User);
            var user = _userService.GetById(id);
            var posts = _postService.GetPostByUserID(id);
           // var user = _userManager.Users;
            var userPosts = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.User.UserName,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = GetForumListingForPost(post)
                // ForumImageUrl = _postService.GetForumImageUrl(post.Id),
                //  ForumId = post.Forum.Id
            });
            var model = new UserPostListModel
            {
                Posts = userPosts,
               UserId=user.Id,
               UserName=user.UserName,
               ImageUrl=user.ProfileImageUrl
            };
            return View(model);
        }
    }
}