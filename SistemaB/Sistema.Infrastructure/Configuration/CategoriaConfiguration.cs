using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Configuration
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias")
                .HasKey(c => c.PK_CATEGORIA);
            builder.HasOne(s => s.categoriaSuperior)
                .WithMany(c => c.categoria)
                .HasForeignKey(c=> c.PK_CATEGORIA_SUP);
        }
    }
}
