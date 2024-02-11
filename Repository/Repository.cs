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


    public IEnumerable<T> GetAll()
    {
        return _db.Set<T>().AsNoTracking().ToList();
    }


    public T? Get(Expression<Func<T, bool>> predicate)
    {
        return _db.Set<T>().FirstOrDefault(predicate);
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