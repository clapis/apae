using Apae.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Apae.Data.Extensions
{
    public static class DataExtensions
    {
        public static IHost DbUpdate(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            DbUpdateAsync(scope.ServiceProvider).Wait();

            return host;
        }

        public static async Task DbUpdateAsync(IServiceProvider services)
        {
            // Run any pending migrations
            await DbMigrationsAsync(services);

            // Ensure all roles registered
            await EnsureRolesAsync(services);

            // Ensure admin user exists
            await EnsureAdminUserAsync(services);
        }

        private static async Task DbMigrationsAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();
        }

        private static async Task EnsureRolesAsync(IServiceProvider services)
        {
            // Ensure roles
            var manager = services.GetRequiredService<RoleManager<Role>>();

            var result = await Task.WhenAll(Roles.GetAll()
                            .Except(manager.Roles.Select(r => r.Name))
                            .Select(name => new Role { Name = name })
                            .Select(r => manager.CreateAsync(r)));

            if (result.Any(t => !t.Succeeded))
                throw new Exception($"Failed seeding roles: {result.First(t => !t.Succeeded)}");

        }

        private static async Task EnsureAdminUserAsync(IServiceProvider services)
        {
            var manager = services.GetRequiredService<UserManager<User>>();

            const string email = "admin@tangram.social";

            var admin = await manager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new User
                {
                    Email = email,
                    UserName = email,
                    FirstName = "Admin",
                    LastName = "Tangram"
                };

                var result = await manager.CreateAsync(admin, email);

                if (!result.Succeeded)
                    throw new Exception($"Failed creating admin user: {result}");
            }

            if (!await manager.IsInRoleAsync(admin, Roles.Admin))
            {
                var res = await manager.AddToRoleAsync(admin, Roles.Admin);

                if (!res.Succeeded)
                    throw new Exception($"Failed adding admin user to admin role: {res}");
            }
        }
    }
}
