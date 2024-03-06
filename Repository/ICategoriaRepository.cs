namespace CatalogoApi.Repository;

using CatalogoApi.Models;
using CatalogoApi.Pagination;

public interface ICategoriaRepository : IRepository<Categoria>
{
    PagedList<Categoria> GetCategoriasByFilters(CategoriasParameters categoriasParameters);

    PagedList<Categoria> GetCategoriasByNome(CategoriasFilterNome categoriasFilterNome);
}