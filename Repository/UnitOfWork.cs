using CatalogoApi.Context;

namespace CatalogoApi.Repository;

public class UnitOfWork : IUnitOfWork
{

    private IProdutoRepository? _produtoRepository;
    private ICategoriaRepository? _categoriaRepository;
    public AppDbContext _db;

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
    }


    public IProdutoRepository ProdutoRepository 
    {
        get
        {
            // return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_db);

            if (_produtoRepository is null)
            {
                _produtoRepository = new ProdutoRepository(_db);
            }
            
            return _produtoRepository;
        }
    }

    public ICategoriaRepository CategoriaRepository
    {
        get
        {
            return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_db);
        }
    }

    public async Task CommitAsync()
    {
        await _db.SaveChangesAsync();
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}