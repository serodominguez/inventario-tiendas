using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Configuration
{
    public class ExistenciaTConfiguration : IEntityTypeConfiguration<ExistenciaT>
    {
        public void Configure(EntityTypeBuilder<ExistenciaT> builder)
        {
            builder.ToTable("ExistenciasT")
                .HasKey(e => e.PK_EXISTENCIAST);
            builder.HasOne(e => e.inventariosT)
               .WithMany(i => i.existenciasT)
               .HasForeignKey(e => e.PK_INVENTARIOT);
            builder.HasOne(e => e.articulos)
               .WithMany(a => a.existenciasT)
               .HasForeignKey(e => e.PK_ARTICULO);
        }
    }
}
