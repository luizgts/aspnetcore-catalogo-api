namespace CatalogoApi.Repository;

using CatalogoApi.Context;
using CatalogoApi.Models;

public class ProdutoRepository : IProdutoRepository
{
    
    private readonly AppDbContext _db;

    public ProdutoRepository(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<Produto> GetAll()
    {
        return _db.Produtos;
    }

    public Produto GetOne(int id)
    {
        var produto = _db.Produtos.FirstOrDefault(p => p.ProdutoId == id);

        if (produto is null)
            throw new InvalidOperationException("Produto é null");
        
        return produto;
    }

    public Produto Create(Produto produto)
    {
        if (produto is null)
            throw new ArgumentNullException();
        
        _db.Produtos.Add(produto);
        _db.SaveChanges();

        return produto;
    }

    public bool Update(Produto produto)
    {
        if (produto is null)
            throw new ArgumentNullException();
        
        var hasId = _db.Produtos.Any(p => p.ProdutoId == produto.ProdutoId);

        if (hasId)
        {
            _db.Produtos.Update(produto);
            _db.SaveChanges();
            return true;
        }

        return false;
    }

    public bool Delete(int id)
    {
        var produto = _db.Produtos.Find(id);

        if (produto is not null)
        {
            _db.Produtos.Remove(produto);
            _db.SaveChanges();
            return true;
        }

        return false;
    }
}