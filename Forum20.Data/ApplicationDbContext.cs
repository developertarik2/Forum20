﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Forum20.Data.Models;

namespace Forum20.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Forum> Forums { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostReply> PostReplies { get; set; }
        /* protected override void OnModelCreating(ModelBuilder builder)
         {
             base.OnModelCreating(builder);
             // Customize the ASP.NET Identity model and override the defaults if needed.
             // For example, you can rename the ASP.NET Identity table names and more.
             // Add your customizations after calling base.OnModelCreating(builder);
         }*/
    }
}
