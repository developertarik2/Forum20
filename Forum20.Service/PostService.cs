using Forum20.Data;
using Forum20.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Forum20.Service
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext _context;
        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(Post post)
        {
            _context.Add(post);
            //throw new NotImplementedException();
            await _context.SaveChangesAsync();
        }

        public async Task AddReply(PostReply reply)
        {
            _context.PostReplies.Add(reply);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            var post = GetById(id);
            _context. Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReply(int id)
        {
            var replies = GetByPostId(id);
            //_context.Remove(replies);
            _context.RemoveRange(replies);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<PostReply> GetByPostId(int id)
        {
            return _context.PostReplies.Where(replies => replies.Post.Id == id);
        }

        public async Task EditPostContent(int id, string NewContent)
        {
            var post = GetById(id);
            post.Content = NewContent;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts
                 .Include(post => post.User)
                 .Include(post => post.Replies).ThenInclude(reply => reply.User)
                 .Include(post => post.Forum);
                
        }

        public Post GetById(int id)
        {
            // throw new NotImplementedException();
            return _context.Posts.Where(post => post.Id == id)             
                 .Include(post => post.User)
                 .Include(post => post.Replies).ThenInclude(reply => reply.User)
                 .Include(post => post.Forum)
                 .FirstOrDefault();
        }

        public IEnumerable<Post> GetFilteredPosts(Forum forum,  string searchQuery)
        {
            // throw new NotImplementedException();
           // var forum = _context.Forums.Find(id);
            return string.IsNullOrEmpty(searchQuery)
                ? forum.Posts
                : forum.Posts.Where(post =>
                post.Title.Contains(searchQuery)
                || post.Content.Contains(searchQuery));
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
              var normalaized = searchQuery;
              return GetAll().Where(post =>
              post.Title.ToLower().Contains(normalaized)
              || post.Content.ToLower().Contains(normalaized));
         /*   var query = searchQuery.ToLower();

            return _context.Posts
                .Include(post => post.Forum)
                .Include(post => post.User)
                .Include(post => post.Replies)
                .Where(post =>
                    post.Title.ToLower().Contains(query)
                 || post.Content.ToLower().Contains(query));*/
        }

        public IEnumerable<Post> GetLatestPosts(int n)
        {
           return GetAll().OrderByDescending(post => post.Created).Take(n);
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
            //throw new NotImplementedException();
            return _context.Forums.Where(forum => forum.Id ==id).First().Posts;
        }

        public PostReply GetReply(int id ,int postId)
        {
            //var posts= 
            return _context.PostReplies.
                Where(replies => replies.Id == id && replies.Post.Id ==postId).
                 Include(replies => replies.User)
                 .Include(replies=>replies.Post)
                 .Include(replies=>replies.Post.Forum)
                .First();
        }

        public async Task EditReply( int id, int postId,string newReply)
        {
            var reply = GetReply(id,postId);
            reply.Content = newReply;
            _context.PostReplies.Update(reply);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReplyByUser(int id, int postId)
        {
            var reply = GetReply(id, postId);
            _context.Remove(reply);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Post> GetPostByUserID(string userId)
        {
            return _context.Posts.Where(post => post.User.Id == userId)
                .Include(post => post.User)
                .Include(post => post.Replies).ThenInclude(reply => reply.User)
                .Include(post => post.Forum);
               
        }

        public IEnumerable<Post> PendingPosts()
        {
            return _context.Posts.Where(post => post.Status == "Pending")
               .Include(post => post.User)             
               .Include(post => post.Forum);
        }
        /* public Post GetPostAll(int page)
{
    var latestPosts = _context.Posts.AsNoTracking()
         .OrderBy(post => post.Id);
    //  var model = await PagingList.CreateAsync(model, 5, page);
    return  PagingList.CreateAsync(latestPosts, 1, page);
   // return model;
}*/
    }
}
