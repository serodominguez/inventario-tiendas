using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Configuration
{
    public class CategoriaSuperiorConfiguration : IEntityTypeConfiguration<CategoriaSuperior>
    {
        public void Configure(EntityTypeBuilder<CategoriaSuperior> builder)
        {
            builder.ToTable("CategoriaSuperior")
                 .HasKey(s => s.PK_CATEGORIA_SUP);
        }
    }
}
