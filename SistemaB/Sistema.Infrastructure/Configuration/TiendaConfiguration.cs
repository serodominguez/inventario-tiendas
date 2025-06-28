using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Configuration
{
    public class TiendaConfiguration : IEntityTypeConfiguration<Tienda>
    {
        public void Configure(EntityTypeBuilder<Tienda> builder)
        {
            builder.ToTable("Tiendas")
                .HasKey(t => t.PK_TIENDA);
        }
    }
}
