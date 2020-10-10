using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Apae.Data.Auditing;
using Apae.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Apae.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {

        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }


        private readonly IHttpContextAccessor _context;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor context)
            : base(options)
        {
            _context = context;
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Benefit>(model =>
            {
                model.HasOne(e => e.CreatedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                model.HasOne(e => e.LastModifiedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Beneficiary>(model =>
            {
                model.HasOne(e => e.CreatedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                model.HasOne(e => e.LastModifiedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                model.HasIndex(p => p.NIS)
                    .IsUnique();
            });

            builder.Entity<FamilyMember>(model =>
            {
                model.HasOne(e => e.CreatedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

                model.HasOne(e => e.LastModifiedBy)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

            });
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateAuditableFields();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditableFields();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void UpdateAuditableFields()
        {
            var entries = ChangeTracker.Entries<IAuditable>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedById = _context.HttpContext.User.GetUserId();
                }

                entry.Entity.LastModifiedOn = DateTime.UtcNow;
                entry.Entity.LastModifiedById = _context.HttpContext.User.GetUserId();
            }
        }
    }
}
