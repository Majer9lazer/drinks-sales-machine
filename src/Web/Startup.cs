using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Newtonsoft.Json;
using Persistence.Data;
using Web.Extensions;
using Web.Hubs;

namespace Web
{
    public class Startup
    {
        private readonly string _contentRootPath;
        private const string ContentRootPathName = "%CONTENTROOTPATH%";
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _contentRootPath = environment.ContentRootPath;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connString = Configuration.GetConnectionString("DefaultConnection");
                if (connString.Contains(ContentRootPathName))
                {
                    connString = connString.Replace(ContentRootPathName, _contentRootPath);
                    options.UseSqlServer(connString);
                }
                else
                {
                    throw new InvalidOperationException($"There must be {ContentRootPathName} in connection string in 'AttachDbFileName' Block");
                }
            });

            services.AddApplicationServices();

            services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddRazorPages();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.MigrateApplicationDbContext();
            app.AddDefaultUsers();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();

                endpoints.MapHub<PaymentHub>("/paymentHub");
                endpoints.MapHub<AdminOperationsHub>("/adminOperationsHub");
            });
        }
    }
}
