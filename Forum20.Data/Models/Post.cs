using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Forum20.Data.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Content { get; set; }

        public string Status { get; set; }

        public DateTime Created { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Forum Forum { get; set; }

        public virtual IEnumerable<PostReply> Replies { get; set; }

        
    }
}
