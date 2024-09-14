using Forum20.Models.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum20.Models.Post
{
    public class UserPostListModel
    {
        public IEnumerable<PostListingModel> Posts { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string ImageUrl { get; set; }
    }
}
