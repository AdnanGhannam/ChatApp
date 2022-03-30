using ChatVia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.DbConfigurations
{
    public class AppUserConfigurations : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(e => e.Bio)
                .HasMaxLength(200);

            builder.Property(e => e.Location)
                .HasMaxLength(100);

            builder.Property(e => e.Status)
                .HasMaxLength(150);


            // Relationships
            builder.HasMany(e => e.Chats)
                .WithMany(e => e.Members)
                .UsingEntity("ChatsMembers");

            builder.HasMany(e => e.MutedChats)
                .WithMany(e => e.MutedBy)
                .UsingEntity("UsersMutedChat");
            builder
                .HasMany(e => e.BlockedUsers)
                .WithMany(u => u.BlockedBy)
                .UsingEntity(b => b.ToTable("BlockedUsers"));

            builder.HasMany(e => e.Contacts)
                .WithMany(e => e.SavedBy)
                .UsingEntity(b => b.ToTable("UsersContacts"));
        }
    }
}
