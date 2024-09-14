using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Forum20.Data;
using Forum20.Data.Models;
using Forum20.Models;
using Forum20.Models.Forum;
using Forum20.Models.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Forum20.Controllers
{
    public class ForumController : Controller
    {
        private readonly IPost _postService;
        private readonly IForum _forumService;
     //   private readonly IApplicationUser _userService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;
        public ForumController(IForum forumService,IPost postService,IConfiguration configuration,IUpload uploadService)
        {
            _forumService = forumService;
            _postService = postService;
            _configuration = configuration;
            _uploadService = uploadService;
        }
        public IActionResult Index()
        {
           var forums = _forumService.GetAll().Select(forum => new ForumListingModel {
               Id=forum.Id,
               Name=forum.Title,
               Description=forum.Description,
               NumberOfPosts = forum.Posts?.Count() ?? 0,
               NumberOfUsers = _forumService.GetActiveUsers(forum.Id).Count(),
               ImageUrl = forum.ImageUrl,
               HasRecentPost = _forumService.HasRecentPost(forum.Id)

           }) ;

            var model = new ForumIndexModel {
                ForumList = forums.OrderBy(f=> f.Name)
            };
            return View(model);
        }
        public IActionResult Topic(int id,string searchQuery)
        {
            var forum = _forumService.GetById(id);
            // var posts = _postService.GetFilteredPosts(id,searchQuery).ToList();
            var posts = new List<Post>();

          // if(!String.IsNullOrEmpty(searchQuery))
          //  {
                posts = _postService.GetFilteredPosts(forum,  searchQuery).ToList();
          //  }
           // posts = forum.Posts.ToList();
            var postListings = posts.Select(post => new PostListingModel {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorName=post.User.UserName,
                AuthorRating = post.User.Rating,
                Title = post.Title,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)
            });

            var model=new ForumTopicModel
            {
              Posts=postListings,
              Forum=BuildForumListing(forum)
            };
            return View(model);
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            // throw new NotImplementedException();
            var forum = post.Forum;
            return BuildForumListing(forum);
        }
        [HttpPost]
        public IActionResult Search(int id,string searchQuery)
        {
            return RedirectToAction("Topic", new { id, searchQuery });
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            var model = new AddForumModel();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddForum(AddForumModel model)
        {
            var imageUri = "/images/users/default.png";

            if(model.ImageUpload!=null)
            {
                var blockBlob = UploadForumImage(model.ImageUpload);
                imageUri = blockBlob.Uri.AbsoluteUri;
            }

            var forum = new Forum
            {
                Title = model.Title,
                Description=model.Description,
                Created=DateTime.Now,
                ImageUrl=imageUri
            };

            await _forumService.Create(forum);
            return RedirectToAction("Index", "Forum");
        }

        private CloudBlockBlob UploadForumImage(IFormFile imageUpload)
        {
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");  
            var container = _uploadService.GetBlobContainer(connectionString,"forum-images");

            var ContentDisposition = ContentDispositionHeaderValue.Parse(imageUpload.ContentDisposition);

            var filename = ContentDisposition.FileName.Trim('"');

            var blockBlob = container.GetBlockBlobReference(filename);
          
             blockBlob.UploadFromStreamAsync(imageUpload.OpenReadStream()).Wait();
            //  await _userService.SetProfileImage(userId, blockBlob.Uri);

            // return RedirectToAction("Detail", "Profile", new { id = userId });
            return blockBlob;
        }

        private ForumListingModel BuildForumListing(Forum forum)
        {
            // throw new NotImplementedException();
           // var forum = post.Forum;
            return new ForumListingModel
            {
                Id = forum.Id,
                ImageUrl = forum.ImageUrl,
                Name = forum.Title,
                Description = forum.Description
            };
        }
    }
}