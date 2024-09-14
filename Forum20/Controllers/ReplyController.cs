using System;
using System.Threading.Tasks;
using Forum20.Data;
using Forum20.Data.Models;
using Forum20.Models.Reply;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum20.Controllers
{
    [Authorize]
    public class ReplyController : Controller
    {
        private readonly IPost _postService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _userService;
        
        public ReplyController(IPost postService, UserManager<ApplicationUser> userManager,IApplicationUser userService)       
        {
            _postService = postService;
            _userManager = userManager;
            _userService = userService;
        }

        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            var post = _postService.GetById(id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var model = new PostReplyModel
            {
                PostContent = post.Content,
                PostTitle = post.Title,
                PostId = post.Id,

                ForumName =post. Forum.Title,
                ForumId =post. Forum.Id,
                ForumImageUrl =post. Forum.ImageUrl,

                AuthorName = User.Identity.Name,
                AuthorImageUrl = user.ProfileImageUrl,
                AuthorId = user.Id,
                AuthorRating = user.Rating,
                IsAuthorAdmin = User.IsInRole("Admin"),

                Created = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReply(PostReplyModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var reply = BuildReply(model, user);
            await _postService.AddReply(reply);
            await _userService.UpdateUserRating(userId, typeof(PostReply));
            return RedirectToAction("Index", "Post", new { id = model.PostId });
        }
        private PostReply BuildReply(PostReplyModel reply, ApplicationUser user)
        {
          //  var now = DateTime.Now;
            var post = _postService.GetById(reply.PostId);

            return new PostReply
            {
                Post = post,
                Content = reply.ReplyContent,
                Created = DateTime.Now,
                User = user
            };
        }

        [Authorize]
        public IActionResult Edit( int id,int postId)
        {
            var reply = _postService.GetReply(id,postId);
            var model = new PostReplyModel
            {
                Id=reply.Id,
                ReplyContent=reply.Content,
                PostId=reply.Post.Id,
                PostTitle=reply.Post.Title,
                ForumImageUrl=reply.Post.Forum.ImageUrl,
                AuthorName=reply.User.UserName,

            };
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, int postId,PostReplyModel model)
        {
            await _postService.EditReply(model.Id,model.PostId,model.ReplyContent);
            return RedirectToAction("Index", "Post", new { id = model.PostId });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id, int postId)
        {
            var post = _postService.GetById(postId);
            await _postService.DeleteReplyByUser(id, postId);
            return RedirectToAction("Index", "Post", new { id = post.Id });
        }
    }
}