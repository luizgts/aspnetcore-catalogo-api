namespace CatalogoApi.Repository;

using System.Collections.Generic;
using CatalogoApi.Context;
using CatalogoApi.Models;
using CatalogoApi.Pagination;
using X.PagedList;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext db) : base(db) {}

    public async Task<IEnumerable<Produto>> GetProdutosByCategoriaAsync(int id)
    {
        var categorias = await GetAllAsync();
        return categorias.Where(c => c.CategoriaId == id);
    }

    // public IEnumerable<Produto> GetProdutosByFilters(ProdutosParameters produtosParameters)
    // {
    //     var pageSize = produtosParameters.PageSize;
    //     var pageNumber = produtosParameters.PageNumber;

    //     return GetAll()
    //         .OrderBy(item => item.ProdutoId)
    //         .Skip(pageSize * (pageNumber - 1))
    //         .Take(pageSize)
    //         .ToList();
    // }

    public async Task<IPagedList<Produto>> GetProdutosByFiltersAsync(ProdutosParameters produtosParameters)
    {
        var filters = produtosParameters;

        var produtos = await GetAllAsync();
        var produtosOrdenados = produtos
            .OrderBy(item => item.ProdutoId)
            .AsQueryable();
        
        return await produtos.ToPagedListAsync(filters.PageNumber, filters.PageSize);

    }

    public async Task<IPagedList<Produto>> GetProdutosByPrecoAsync(ProdutosFilterPreco produtosFilterPreco)
    {
        var filters = produtosFilterPreco;
        var produtos = await GetAllAsync();

        if (filters.Preco.HasValue && !string.IsNullOrEmpty(filters.PrecoCriterio))
        {
            if (filters.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos
                    .Where(item => item.Preco > filters.Preco.Value)
                    .OrderBy(item => item.Preco);
            }
            else if (filters.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos
                    .Where(item => item.Preco < filters.Preco.Value)
                    .OrderBy(item => item.Preco);
            }
            else if (filters.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
            {
                produtos = produtos
                    .Where(item => item.Preco == filters.Preco.Value)
                    .OrderBy(item => item.Preco);
            }
        }

        return await produtos.ToPagedListAsync(filters.PageNumber, filters.PageSize);
    }
}