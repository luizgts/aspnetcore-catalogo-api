using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CatalogoApi.Models;
using CatalogoApi.Filters;
using CatalogoApi.Repository;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaRepository _repository;
    private readonly ILogger _logger;

    public CategoriasController(ICategoriaRepository repository, ILogger<CategoriasController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    // [HttpGet("produtos")]
    // public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutos()
    // {
    //     _logger.LogInformation("# GET api/categoria/produtos");
    //     return await _context.Categorias.Include(p => p.Produtos).ToListAsync();
    // }


    [HttpGet]
    // [ServiceFilter(typeof(ApiLoggingFilter))] // Filtro personalizado
    public async Task<ActionResult<IEnumerable<Categoria>>> GetAll()
    {
        // _logger.LogInformation("# GET api/categoria");
        
        var categorias = await _repository.GetAllAsync();
        return Ok(categorias);
    }


    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<Categoria>> GetById(int id)
    {
        var categoria =  await _repository.GetOneAsync(id);

        // _logger.LogInformation($"# GET api/categoria/produtos/{id}");

        if(categoria is null)
        {
            _logger.LogInformation($"# GET api/categoria/produtos/{id} | Not Found");
            return NotFound("Categoria não encontrada");
        }

        return Ok(categoria);
    }


    [HttpPost]
    public ActionResult Add(Categoria categoria)
    {

        if (categoria is null)
        {
            return BadRequest();
        }

        var newCategoria = _repository.Create(categoria);
        
        // Location Header 
        return new CreatedAtRouteResult("ObterCategoria", new { id = newCategoria.CategoriaId }, newCategoria);
    }


    [HttpPut("{id:int}")]
    public ActionResult Update(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId) 
        {
            return BadRequest();
        }

        var updatedCategoria = _repository.Update(categoria);

        return Ok(updatedCategoria);
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var categoria = await _repository.GetOneAsync(id);

        if (categoria is null)
        {
            return NotFound();
        }

        var deletedCategoria = _repository.Delete(id);

        return Ok(deletedCategoria);
    }
}