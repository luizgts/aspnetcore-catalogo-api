namespace CatalogoApi.Repository;

using System.Collections.Generic;
using CatalogoApi.Context;
using CatalogoApi.Models;
using CatalogoApi.Pagination;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext db) : base(db) {}

    public IEnumerable<Produto> GetProdutosByCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
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

    public PagedList<Produto> GetProdutosByFilters(ProdutosParameters produtosParameters)
    {
        var filters = produtosParameters;

        var produtos = GetAll()
            .OrderBy(item => item.ProdutoId)
            .AsQueryable();
        
        return PagedList<Produto>.ToPagedList(produtos, filters.PageNumber, filters.PageSize);

    }

    public PagedList<Produto> GetProdutosByPreco(ProdutosFilterPreco produtosFilterPreco)
    {
        var filters = produtosFilterPreco;
        var produtos = GetAll().AsQueryable();

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

        return PagedList<Produto>.ToPagedList(produtos, filters.PageNumber, filters.PageSize);
    }
}