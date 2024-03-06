namespace CatalogoApi.Repository;

using CatalogoApi.Context;
using CatalogoApi.Models;
using CatalogoApi.Pagination;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext db) : base(db) {}

    public PagedList<Categoria> GetCategoriasByFilters(CategoriasParameters categoriasParameters)
    {
        var filters = categoriasParameters;

        var categorias = GetAll()
            .OrderBy(item => item.CategoriaId)
            .AsQueryable();
        
        return PagedList<Categoria>.ToPagedList(categorias, filters.PageNumber, filters.PageSize);
    }

    public PagedList<Categoria> GetCategoriasByNome(CategoriasFilterNome categoriasFilterNome)
    {
        var filters = categoriasFilterNome;

        var categorias = GetAll().AsQueryable();

        if(!string.IsNullOrEmpty(filters.Nome))
        {
            categorias = categorias.Where(item => item!.Nome!.Contains(filters.Nome));
        }

        return PagedList<Categoria>.ToPagedList(categorias, filters.PageNumber, filters.PageSize);
    }
}