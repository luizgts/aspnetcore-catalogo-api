using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CatalogoApi.Context;
using CatalogoApi.Models;

namespace CatalogoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProtudosController : ControllerBase
{

    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public ProtudosController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpGet("config")]
    public ActionResult<string> GetConfigs() 
    {
        var version = _config["version"];
        var author = _config["author:name"];

        return Ok($"Version: {version} | Author: {author}");
    }

    // Otimização com o método AsNoTracking
    // Deve ser utilizado em consultas de leitura
    // O contexto mantem o rastreamento dos objetos em memória adicionando uma sobrecarga ao sistema
    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _context?.Produtos?.AsNoTracking().Take(10).ToList();

        if(produtos is null) {
            return NotFound();
        }

        return produtos;
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto =  _context?.Produtos?.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
        if(produto is null)
        {
            return NotFound();
        }

        return produto;
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {

        if (produto is null)
        {
            return BadRequest();
        }

        _context?.Produtos?.Add(produto);
        _context?.SaveChanges();
        // Location Header 

        return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId) {
            return BadRequest();
        }

        _context.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _context?.Produtos?.FirstOrDefault(p => p.ProdutoId == id);
        if (produto is null)
        {
            return NotFound();
        }

        _context?.Produtos?.Remove(produto);
        _context?.SaveChanges();

        return Ok();
    }

}