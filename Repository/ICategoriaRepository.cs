namespace CatalogoApi.Repository;

using CatalogoApi.Models;
using CatalogoApi.Pagination;
using X.PagedList;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IPagedList<Categoria>> GetCategoriasByFiltersAsync(CategoriasParameters categoriasParameters);

    Task<IPagedList<Categoria>> GetCategoriasByNomeAsync(CategoriasFilterNome categoriasFilterNome);
}