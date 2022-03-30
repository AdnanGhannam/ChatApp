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
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(300);



            // Relationships
            builder.HasOne(e => e.Sender)
                .WithMany(e => e.Messages)
                .HasForeignKey(e => e.SenderId);
        }
    }
}
