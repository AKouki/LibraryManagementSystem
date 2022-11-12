using LMS.Core.Domain.Books;
using LMS.Core.Domain.Issues;
using LMS.Core.Domain.Members;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LMS.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }
        
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LMSDB;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
