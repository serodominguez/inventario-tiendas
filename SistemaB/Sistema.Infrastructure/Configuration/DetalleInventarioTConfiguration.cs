using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Configuration
{
    public class DetalleInventarioTConfiguration : IEntityTypeConfiguration<DetalleInventarioT>
    {
        public void Configure(EntityTypeBuilder<DetalleInventarioT> builder)
        {
            builder.ToTable("DetalleInventariosT")
                .HasKey(d => d.PK_DETALLET);
            builder.HasOne(d => d.inventariosT)
               .WithMany(i => i.detallesInventario)
               .HasForeignKey(d => d.PK_INVENTARIOT);
        }
    }
}
