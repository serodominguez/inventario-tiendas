using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Configuration
{
    public class ArticuloConfiguration : IEntityTypeConfiguration<Articulo>
    {
        public void Configure(EntityTypeBuilder<Articulo> builder)
        {
            builder.ToTable("Articulos")
                .HasKey(a => a.PK_ARTICULO);
            builder.HasOne(c => c.categoria)
                .WithMany(a => a.articulo)
                .HasForeignKey(a => a.PK_CATEGORIA);
        }
    }
}
