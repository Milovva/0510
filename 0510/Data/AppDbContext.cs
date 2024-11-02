using _0510.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0510.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Comments)
                .WithOne(c => c.Book)
                .HasForeignKey(c => c.BookId);

            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "1984",
                    Author = "George Orwell",
                    Genre = "Dystopian",
                    Price = 9.99m
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Genre = "Fiction",
                    Price = 7.99m
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Genre = "Classic",
                    Price = 10.99m
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Moby Dick",
                    Author = "Herman Melville",
                    Genre = "Adventure",
                    Price = 8.99m
                });
        }
    }
}
