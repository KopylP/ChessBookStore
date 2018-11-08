using ChessBookStore.web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessBookStore.web.Data
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) {}
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Discont> Disconts { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Discont>()
            //    .HasMany(c => c.Books)
            //    .WithOne(e => e.Discont);
            //modelBuilder.Entity<Category>()
            //    .HasMany(c => c.Books)
            //    .WithOne(e => e.Category);
            modelBuilder.Entity<Author>()
                .HasMany(c => c.Books)
                .WithOne(c => c.Author);
            //Console.WriteLine("Hello");
            modelBuilder.Entity<Book>()
                .HasOne(c => c.Author)
                .WithMany(c => c.Books);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<ChessBookStore.web.Models.Address> Address { get; set; }
    }
}
