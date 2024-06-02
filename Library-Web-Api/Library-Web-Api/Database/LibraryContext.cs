using Library_Web_Api.Identity;
using Library_Web_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Web_Api.Database
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users {  get; set; }
        public DbSet<Loggs> Loggs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books);


            modelBuilder.Entity<User>().HasData(
                     new User
                     {
                         Id = 1,
                         FirstName = "Admin",
                         LastName = "Admin",
                         UserName = "Admin123",
                         Password = PasswordHashing.HashPassword("Admin123"),
                         Role = Roles.Admin
                     },
                     new User
                     {
                         Id = 2,
                         FirstName = "Misho",
                         LastName = "Kharazishvili",
                         UserName = "Misho123",
                         Password = PasswordHashing.HashPassword("Misho123"),
                         Role = Roles.User
                     });
        }
    }
}
