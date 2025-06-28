using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios")
                  .HasKey(u => u.PK_USUARIO);
            builder.HasOne(r => r.rol)
                .WithMany(u => u.usuario)
                .HasForeignKey(u => u.PK_ROL);
        }
    }
}
