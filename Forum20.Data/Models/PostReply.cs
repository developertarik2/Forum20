using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Forum20.Data.Models
{
    public class PostReply
    {
        public int Id { get; set; }

       
        [Required]
        public string Content { get; set; }

        public DateTime Created { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Post Post { get; set; }
    }
}
