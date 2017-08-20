using LMS.Core.Domain;
using LMS.Core.Domain.Books;
using LMS.Core.Domain.Issues;
using LMS.Core.Domain.Members;
using LMS.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {

        }

        public LibraryContext()
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Issue> Issues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new BookConfiguration(modelBuilder.Entity<Book>());
            new MemberConfiguration(modelBuilder.Entity<Member>());
            new IssueConfiguration(modelBuilder.Entity<Issue>());
            new BookAuthorConfiguration(modelBuilder.Entity<BookAuthor>());
            new BookSubjectConfiguration(modelBuilder.Entity<BookSubject>());
            new AuthorConfiguration(modelBuilder.Entity<Author>());
            new SubjectConfiguration(modelBuilder.Entity<Subject>());
        }
    }
}
