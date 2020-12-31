using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectTimeTracking.Models;

namespace ProjectTimeTracking.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<ProjectStatus> ProjectStatus { get; set; }
        public DbSet<Resource> Resource { get; set; }
        public DbSet<ProjectTracker> ProjectTracker{ get; set; }
    }
}