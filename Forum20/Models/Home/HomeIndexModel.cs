﻿using Forum20.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum20.Models.Home
{
    public class HomeIndexModel
    {
        public string SearchQuery { get; set; }
        public IEnumerable<PostListingModel> LatestPosts { get; set; }
    }
}
