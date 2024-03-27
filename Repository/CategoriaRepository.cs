namespace CatalogoApi.Repository;

using CatalogoApi.Context;
using CatalogoApi.Models;
using CatalogoApi.Pagination;
using X.PagedList;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext db) : base(db) {}

    public async Task<IPagedList<Categoria>> GetCategoriasByFiltersAsync(CategoriasParameters categoriasParameters)
    {
        var filters = categoriasParameters;

        var categorias = await GetAllAsync();
        var categoriasOrdenadas = categorias
            .OrderBy(item => item.CategoriaId)
            .AsQueryable();
        
        // return PagedList<Categoria>.ToPagedList(categoriasOrdenadas, filters.PageNumber, filters.PageSize);

        return await categorias.ToPagedListAsync(
            filters.PageNumber,
            filters.PageSize
        );
    }

    public async Task<IPagedList<Categoria>> GetCategoriasByNomeAsync(CategoriasFilterNome categoriasFilterNome)
    {
        var filters = categoriasFilterNome;

        var categorias = await GetAllAsync();

        if(!string.IsNullOrEmpty(filters.Nome))
        {
            categorias = categorias.Where(item => item!.Nome!.Contains(filters.Nome));
        }

        // return PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), filters.PageNumber, filters.PageSize);

        return await categorias.ToPagedListAsync(
            filters.PageNumber,
            filters.PageSize
        );
    }
}