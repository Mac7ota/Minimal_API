using Catagory.Models;
using CatalogoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Categoria>().HasKey(c => c.CategoriaId);
            modelBuilder.Entity<Categoria>().Property(c => c.Nome).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Categoria>().Property(c => c.Descricao).IsRequired().HasMaxLength(300);
            modelBuilder.Entity<Categoria>().Property(c => c.ImagemUrl).IsRequired().HasMaxLength(500);

            modelBuilder.Entity<Produto>().HasKey(p => p.ProdutoId);
            modelBuilder.Entity<Produto>().Property(p => p.Nome).IsRequired().HasMaxLength(300);
            modelBuilder.Entity<Produto>().Property(p => p.Descricao).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<Produto>().Property(p => p.Preco).HasColumnType("decimal(5,2)");
            modelBuilder.Entity<Produto>().Property(p => p.ImagemUrl).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<Produto>().Property(p => p.DataCompra).IsRequired();
            modelBuilder.Entity<Produto>().Property(p => p.Estoque).IsRequired();

            modelBuilder.Entity<Produto>().HasOne(p => p.Categoria).WithMany(c => c.Produtos).HasForeignKey(p => p.CategoriaId);

            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { CategoriaId = 1, Nome = "Bebidas", ImagemUrl = "bebidas.jpg" },
                new Categoria { CategoriaId = 2, Nome = "Lanches", ImagemUrl = "lanches.jpg" },
                new Categoria { CategoriaId = 3, Nome = "Sobremesas", ImagemUrl = "sobremesas.jpg" }
            );

        }
    }
}