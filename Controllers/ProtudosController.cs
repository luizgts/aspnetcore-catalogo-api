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
        var produtos = _repository.GetAll().Take(10).AsNoTracking().ToList();

        if(produtos is null) {
            return NotFound();
        }

        return Ok(produtos);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> GetById(int id)
    {
        var produto =  _repository.GetOne(id);
        if(produto is null)
        {
            return NotFound();
        }

        return Ok(produto);
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

        var hasUpdated = _repository.Update(produto);

        if (hasUpdated)
        {
            return Ok(produto);
        }
        else
        {
            return StatusCode(500);
        }

    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var hadDeleted = _repository.Delete(id);
        
        if (hadDeleted)
        {
            return Ok();
        }

        return StatusCode(500);

    }

}