using Microsoft.EntityFrameworkCore;
using CatalogoApi.Models;

namespace CatalogoApi.Context;

// Realiza o mapeamente das entidades para o banco de dados
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    // Representa os mapeamentos
    public DbSet<Produto> Produtos { get; set;}
    public DbSet<Categoria> Categorias { get; set; }


}