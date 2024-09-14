using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum20.Models.Post
{
    public class DeletePostModel
    {
        public int PostId { get; set; }
        public string PostAuthor { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }

        public int ForumId { get; set; }
    }
}
