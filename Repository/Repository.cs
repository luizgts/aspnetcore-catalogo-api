using System.Linq.Expressions;
using CatalogoApi.Context;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Repository;

public class Repository<T> : IRepository<T> where T : class
{

    protected readonly AppDbContext _db;

    public Repository(AppDbContext db)
    {
        _db = db;
    }


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _db.Set<T>().AsNoTracking().ToListAsync();
    }


    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _db.Set<T>().FirstOrDefaultAsync(predicate);
    }


    public T Create(T entity)
    {
        _db.Set<T>().Add(entity);
        // _db.SaveChanges();
        return entity;
    }

    public T Update(T entity)
    {
        _db.Set<T>().Update(entity);
        // _db.SaveChanges();
        return entity;
    }


    public T Delete(T entity)
    {
        _db.Set<T>().Remove(entity);
        // _db.SaveChanges();
        return entity;
    }
    
}