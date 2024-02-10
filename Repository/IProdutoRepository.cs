using CatalogoApi.Models;

namespace CatalogoApi.Repository;

public interface IProdutoRepository
{
    IQueryable<Produto> GetAll();
    Produto GetOne(int id);
    Produto Create(Produto produto);
    bool Update(Produto produto);
    bool Delete(int id);
}