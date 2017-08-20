using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Data
{
    class LibraryContextFactory : IDbContextFactory<LibraryContext>
    {
        public LibraryContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LMSDB;Trusted_Connection=True;MultipleActiveResultSets=true", option => option.MigrationsAssembly("LMS.Web.Admin"));

            return new LibraryContext(optionsBuilder.Options);
        }
    }
}
