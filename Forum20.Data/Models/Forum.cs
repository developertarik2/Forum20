using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Forum20.Data.Models
{
   public class Forum
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; }
    }
}
