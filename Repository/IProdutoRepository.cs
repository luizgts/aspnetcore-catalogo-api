namespace CatalogoApi.Repository;

using CatalogoApi.Models;
using CatalogoApi.Pagination;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosByCategoria(int id);

    // IEnumerable<Produto> GetProdutosByFilters(ProdutosParameters produtosParameters);
    PagedList<Produto> GetProdutosByFilters(ProdutosParameters produtosParameters);

    PagedList<Produto> GetProdutosByPreco(ProdutosFilterPreco produtosFilterPreco);
}