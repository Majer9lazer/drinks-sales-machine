using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Web.Data;
using Web.Utils;

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
                ApplicationDbSeeder.SeedDatabase(context);
                logger.LogInformation("Seeding data was successfully finished!");
            }
            catch (Exception e)
            {
                logger.LogCritical(e, $"Error while applying migrations in {applicationDbContextName}");
            }
            return app;
        }



    }
}
