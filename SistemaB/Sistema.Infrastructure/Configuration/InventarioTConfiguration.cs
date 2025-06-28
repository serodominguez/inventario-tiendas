using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Configuration
{
    public class InventarioTConfiguration : IEntityTypeConfiguration<InventarioT>
    {
        public void Configure(EntityTypeBuilder<InventarioT> builder)
        {
            builder.ToTable("InventariosT")
                .HasKey(i => i.PK_INVENTARIOT);
            builder.HasOne(i => i.tiendas)
                .WithMany(t => t.inventariosT)
                .HasForeignKey(t => t.PK_TIENDA);
        }
    }
}
