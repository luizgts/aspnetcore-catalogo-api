namespace CatalogoApi.Repository;

using CatalogoApi.Models;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosByCategoria(int id);
}