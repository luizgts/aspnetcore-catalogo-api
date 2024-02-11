namespace CatalogoApi.Repository;

using CatalogoApi.Context;
using CatalogoApi.Models;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext db) : base(db) {}

}