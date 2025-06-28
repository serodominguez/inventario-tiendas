using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Configuration
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Roles")
                 .HasKey(p => p.PK_ROL);
            builder.Property(p => p.ROL)
                .HasMaxLength(15);
            builder.Property(p => p.ESTADO)
                .HasMaxLength(5);
        }
    }
}
