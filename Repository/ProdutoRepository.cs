namespace CatalogoApi.Repository;

using System.Collections.Generic;
using CatalogoApi.Context;
using CatalogoApi.Models;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext db) : base(db) {}

    public IEnumerable<Produto> GetProdutosByCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }
}