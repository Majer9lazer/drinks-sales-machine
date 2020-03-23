using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Identity;
using Persistence.Data;
using Persistence.Entities;

namespace Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder MigrateApplicationDbContext(this IApplicationBuilder app)
        {
            var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();
            const string applicationDbContextName = nameof(ApplicationDbContext);
            try
            {
                logger.LogInformation($"Getting Service Factory for {applicationDbContextName}...");
                using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                logger.LogInformation("Migrating...");
                context.Database.Migrate();
                logger.LogInformation($"Migration of {applicationDbContextName} successfully applied.");
                logger.LogInformation("Seeding data...");
                logger.LogInformation("Seeding data was successfully finished!");
            }
            catch (Exception e)
            {
                logger.LogCritical(e, $"Error while applying migrations in {applicationDbContextName}");
            }
            return app;
        }

        public static IApplicationBuilder AddDefaultUsers(this IApplicationBuilder app)
        {
            const string adminName = "admin@mail.ru";
            const string adminPassword = "admin123";

            var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var user = userManager.FindByNameAsync(adminName).GetAwaiter().GetResult();
            if (user == null)
            {
                var adminUser = new ApplicationUser(adminName);
                var createResult = userManager.CreateAsync(adminUser, adminPassword).GetAwaiter()
                    .GetResult();

                if (!createResult.Succeeded)
                {
                    logger.LogError("Error while creating default users. Errors={Errors}", createResult.Errors);
                }
                else
                {
                    var adminRole = roleManager.FindByNameAsync("admin").GetAwaiter().GetResult();
                    if (adminRole == null)
                    {
                        _ = roleManager.CreateAsync(new IdentityRole("admin")).GetAwaiter().GetResult();
                    }

                    userManager.AddToRoleAsync(adminUser, "admin").GetAwaiter().GetResult();
                    logger.LogInformation("Added Default Users!");
                }
            }

            return app;
        }

    }
}
