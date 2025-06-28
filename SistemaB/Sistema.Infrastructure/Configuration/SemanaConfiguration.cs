using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure.Configuration
{
    public class SemanaConfiguration : IEntityTypeConfiguration<Semana>
    {
        public void Configure(EntityTypeBuilder<Semana> builder)
        {
            builder.ToTable("Semanas")
                .HasKey(s => s.PK_SEMANA);
        }
    }
}
