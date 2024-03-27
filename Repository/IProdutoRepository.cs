namespace CatalogoApi.Repository;

using CatalogoApi.Models;
using CatalogoApi.Pagination;
using X.PagedList;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> GetProdutosByCategoriaAsync(int id);

    // IEnumerable<Produto> GetProdutosByFilters(ProdutosParameters produtosParameters);
    Task<IPagedList<Produto>> GetProdutosByFiltersAsync(ProdutosParameters produtosParameters);

    Task<IPagedList<Produto>> GetProdutosByPrecoAsync(ProdutosFilterPreco produtosFilterPreco);
}