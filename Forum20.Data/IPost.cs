using Forum20.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Forum20.Data
{
  public  interface IPost
    {
        Post GetById(int Id);
        PostReply GetReply(int id,int postId);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery);
        IEnumerable<Post> GetFilteredPosts( string searchQuery);
        IEnumerable<Post> GetPostsByForum(int id);
        IEnumerable<Post> GetLatestPosts(int n);
        IEnumerable<Post> GetPostByUserID(string userId);

        IEnumerable<Post> PendingPosts();
        Task Add(Post post);
        Task Delete(int id);
        Task EditPostContent(int id, string NewContent);
        Task AddReply(PostReply reply);

        Task EditReply( int id, int postId,string newReply);

        Task DeleteReplyByUser(int id, int postId);
        Task DeleteReply(int id);

        IEnumerable<PostReply> GetByPostId(int id);

        //  string Query();
      //  Post GetPostAll(int page);


    }
}
