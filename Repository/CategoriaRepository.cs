using CatalogoApi.Context;
using CatalogoApi.Models;
using CatalogoApi.Repository;
using Microsoft.EntityFrameworkCore;

public class CategoriaRepository : ICategoriaRepository
{

    private readonly AppDbContext _db;
    public CategoriaRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        return await _db.Categorias.AsNoTracking().ToListAsync();
    }

    public async Task<Categoria> GetOneAsync(int id)
    {
        var categoria = await _db.Categorias.FirstOrDefaultAsync(p => p.CategoriaId == id);

        if (categoria is null)
            throw new ArgumentNullException(nameof(categoria));
        
        return categoria;
    }

    public Categoria Create(Categoria categoria)
    {
        if (categoria is null)
            throw new ArgumentNullException(nameof(categoria));
        
        _db.Categorias.Add(categoria);
        _db.SaveChanges();

        return categoria;
    }

    public Categoria Update(Categoria categoria)
    {
        if(categoria is null)
            throw new ArgumentNullException(nameof(categoria));
        
        _db.Categorias.Update(categoria);
        _db.SaveChanges();

        return categoria;
    }

    public Categoria Delete(int id)
    {
        var categoria = _db.Categorias.Find(id);

        if (categoria == null)
            throw new ArgumentNullException(nameof(categoria));

        _db.Categorias.Remove(categoria);
        _db.SaveChanges();

        return categoria;
    }

}