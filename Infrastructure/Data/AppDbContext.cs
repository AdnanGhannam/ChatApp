using ChatVia.Domain.Base;
using ChatVia.Domain.Entities;
using ChatVia.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public AppDbContext(DbContextOptions<AppDbContext> options,
            IDomainEventDispatcher dispatcher)
            : base(options) 
        {
            _dispatcher = dispatcher;
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            ConfigureIdentityEntitiesName(builder);
        }


        private void ConfigureIdentityEntitiesName(ModelBuilder builder)
        {

            builder.Entity<AppUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }

        public override int SaveChanges()
        {
            _preSaveChanges().GetAwaiter().GetResult();
            var res = base.SaveChanges();
            return res;
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await _preSaveChanges();
            var res = await base.SaveChangesAsync(cancellationToken);
            return res;
        }

        private async Task _preSaveChanges()
        {
            await _dispatchDomainEvents();
        }

        private async Task _dispatchDomainEvents()
        {
            var domainEventEntities = ChangeTracker.Entries<EntityBase>()
                .Select(po => po.Entity)
                .Where(po => po.DomainEvents.Any())
                .ToArray();

            foreach (var entity in domainEventEntities)
            {
                IDomainEvent dev;
                while (entity.DomainEvents.TryTake(out dev))
                    await _dispatcher.Dispatch(dev);
            }

            var domainEventAppUser = ChangeTracker.Entries<AppUser>()
                .Select(po => po.Entity)
                .Where(po => po.DomainEvents.Any())
                .ToArray();

            foreach (var entity in domainEventAppUser)
            {
                IDomainEvent dev;
                while (entity.DomainEvents.TryTake(out dev))
                    await _dispatcher.Dispatch(dev);
            }
        }
    }
}
