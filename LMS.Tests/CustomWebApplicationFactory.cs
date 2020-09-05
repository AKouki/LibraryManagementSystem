using LMS.Web.Admin;
using LMS.Web.Admin.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LMS.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("Identity");
                });

                //var sp = services.BuildServiceProvider();

                //using (var scope = sp.CreateScope())
                //{
                //    var scopedServices = scope.ServiceProvider;
                //    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                //    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                //    db.Database.EnsureCreated();

                //    try
                //    {
                //        // Seed user data
                //        var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();
                //        var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                //    }
                //    catch (Exception ex)
                //    {
                //        logger.LogError(ex, "An error occurred seeding the " +
                //        "database with test messages. Error: {Message}", ex.Message);
                //    }

                //}
            });
        }
    }
}
