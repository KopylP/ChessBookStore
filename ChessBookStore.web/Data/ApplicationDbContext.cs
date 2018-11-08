using System;
using System.Collections.Generic;
using System.Text;
using ChessBookStore.web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChessBookStore.web.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Address> Addresses { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Address>().HasOne(c => c.User).WithMany(c => c.Addresses);
            //modelBuilder.Entity<User>().HasMany(c => c.Addresses).WithOne(c => c.User);
            base.OnModelCreating(modelBuilder);
        }
    }
}
