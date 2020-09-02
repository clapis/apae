using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Apae.Data;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Apae.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Apae.Authorization;
using Microsoft.AspNetCore.DataProtection;
using System.IO;

namespace Apae
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("SqliteConnection")));

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(Configuration["DataProtectionPath"]));

            services
                .AddIdentityCore<User>(opts =>
                {
                    opts.Password.RequireDigit = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireNonAlphanumeric = false;
                })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services
                .AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme, opts =>
                {
                    opts.LoginPath = "/login";
                    opts.LogoutPath = "/logout";
                    opts.AccessDeniedPath = "/error/403";
                });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(Policies.CanDelete, p => p.RequireRole(Roles.Admin));
            });

            services
                .AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();
            app.UseRequestLocalization("pt-BR");
            app.UseStaticFiles(BuildStaticFileOptions());
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllers()
                    .RequireAuthorization();
            });

        }

        private static StaticFileOptions BuildStaticFileOptions()
        {
            var provider = new FileExtensionContentTypeProvider();

            provider.Mappings[".webmanifest"] = "application/manifest+json";

            return new StaticFileOptions
            {
                ContentTypeProvider = provider
            };
        }

    }

}
