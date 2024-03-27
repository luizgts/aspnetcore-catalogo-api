using Microsoft.EntityFrameworkCore;
using CatalogoApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CatalogoApi.Context;

// Realiza o mapeamente das entidades para o banco de dados
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    // Representa os mapeamentos
    public DbSet<Produto> Produtos { get; set;}
    public DbSet<Categoria> Categorias { get; set; }

    // Configura o schema necessário para o Identity Framework
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}