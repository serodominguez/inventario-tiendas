using Microsoft.EntityFrameworkCore;
using Sistema.Entities;
using Sistema.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Infrastructure
{
    public class DbContextSistema : DbContext
    {
        public DbSet<Articulo> articulos { get; set; }
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<CategoriaSuperior> categoriaSuperiors { get; set; }
        public DbSet<DetalleInventarioT> detalleInventariosT { get; set; }
        public DbSet<ExistenciaT> existenciasT { get; set; }
        public DbSet<InventarioT> InventariosT { get; set; }
        public DbSet<Semana> Semanas { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Tienda> Tiendas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbContextSistema(DbContextOptions<DbContextSistema> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ArticuloConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaSuperiorConfiguration());
            modelBuilder.ApplyConfiguration(new DetalleInventarioTConfiguration());
            modelBuilder.ApplyConfiguration(new ExistenciaTConfiguration());
            modelBuilder.ApplyConfiguration(new InventarioTConfiguration());
            modelBuilder.ApplyConfiguration(new SemanaConfiguration());
            modelBuilder.ApplyConfiguration(new RolConfiguration());
            modelBuilder.ApplyConfiguration(new TiendaConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        }
    }
}
