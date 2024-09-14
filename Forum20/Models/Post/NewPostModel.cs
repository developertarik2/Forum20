using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum20.Models.Post
{
    public class NewPostModel
    {
        public int Id { get; set; }
        public string ForumName { get; set; }
        public int ForumId { get; set; }
        public string AuthorName { get; set; }
        public string ForumImageUrl { get; set; }

        [Required]       
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Content { get;  set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Title { get;  set; }

        public DateTime Created { get; set; }
    }
}
