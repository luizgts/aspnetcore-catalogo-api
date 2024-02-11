using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CatalogoApi.Models;
using CatalogoApi.Repository;

namespace CatalogoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProtudosController : ControllerBase
{

    private readonly IProdutoRepository _repository;
    // private readonly IConfiguration _config;

    public ProtudosController(IProdutoRepository repository)
    {
        _repository = repository;
        // _config = config;
    }

    // [HttpGet("config")]
    // public ActionResult<string> GetConfigs() 
    // {
    //     var version = _config["version"];
    //     var author = _config["author:name"];

    //     return Ok($"Version: {version} | Author: {author}");
    // }

    // Otimização com o método AsNoTracking
    // Deve ser utilizado em consultas de leitura
    // O contexto mantem o rastreamento dos objetos em memória adicionando uma sobrecarga ao sistema
    [HttpGet]
    public ActionResult<IEnumerable<Produto>> GetAll()
    {
        var produtos = _repository.GetAll();

        if(produtos is null) {
            return NotFound();
        }

        return Ok(produtos);
    }


    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> GetById(int id)
    {
        var produto =  _repository.Get(p => p.ProdutoId == id);

        if(produto is null)
        {
            return NotFound();
        }

        return Ok(produto);
    }


    [HttpGet("categoria/{id:int}")]
    public ActionResult<IEnumerable<Produto>> GetByCategoria(int id)
    {
        var produtos =  _repository.GetProdutosByCategoria(id);

        if(produtos is null)
        {
            return NotFound();
        }

        return Ok(produtos);
    }


    [HttpPost]
    public ActionResult Add(Produto produto)
    {

        if (produto is null)
        {
            return BadRequest();
        }

        var newProduto = _repository.Create(produto);

        // Location Header 
        return new CreatedAtRouteResult("ObterProduto", new { id = newProduto.ProdutoId }, newProduto);
    }


    [HttpPut("{id:int}")]
    public ActionResult Update(int id, Produto produto)
    {
        if (id != produto.ProdutoId) {
            return BadRequest();
        }

        var updatedProduto = _repository.Update(produto);

        return Ok(updatedProduto);

    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _repository.Get(p => p.ProdutoId == id);
        
        if (produto is null)
        {
            return NotFound();
        }

        var deletedProduto = _repository.Delete(produto);

        return Ok(deletedProduto);

    }

}