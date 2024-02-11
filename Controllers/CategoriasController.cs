using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CatalogoApi.Models;
using CatalogoApi.Filters;
using CatalogoApi.Repository;
using CatalogoApi.DTOs;

namespace CatalogoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public CategoriasController(IUnitOfWork unitOfWork, ILogger<CategoriasController> logger)
    {
        _unitOfWork = unitOfWork;
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
    public ActionResult<IEnumerable<CategoriaDTO>> GetAll()
    {
        // _logger.LogInformation("# GET api/categoria");
        
        var categorias = _unitOfWork.CategoriaRepository.GetAll();

        if(categorias is null)
            return NotFound();
        
        var categoriasDTO = categorias.Select(categoria => new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        });

        return Ok(categoriasDTO);
    }


    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> GetById(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(p => p.CategoriaId == id);

        // _logger.LogInformation($"# GET api/categoria/produtos/{id}");

        if(categoria is null)
        {
            _logger.LogInformation($"# GET api/categoria/produtos/{id} | Not Found");
            return NotFound("Categoria não encontrada");
        }

        var categoriaDTO = new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        };

        return Ok(categoriaDTO);
    }


    [HttpPost]
    public ActionResult<CategoriaDTO> Add(CategoriaDTO categoriaDTO)
    {

        if (categoriaDTO is null)
        {
            return BadRequest();
        }

        var categoria = new Categoria()
        {
            CategoriaId = categoriaDTO.CategoriaId,
            Nome = categoriaDTO.Nome,
            ImagemUrl = categoriaDTO.ImagemUrl
        };

        var newCategoria = _unitOfWork.CategoriaRepository.Create(categoria);
        _unitOfWork.Commit();

        var newCategoriaDTO = new CategoriaDTO()
        {
            CategoriaId = newCategoria.CategoriaId,
            Nome = newCategoria.Nome,
            ImagemUrl = newCategoria.ImagemUrl
        };
        
        // Location Header 
        return new CreatedAtRouteResult("ObterCategoria", new { id = newCategoriaDTO.CategoriaId }, newCategoriaDTO);
    }


    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Update(int id, CategoriaDTO categoriaDTO)
    {
        if (id != categoriaDTO.CategoriaId) 
        {
            return BadRequest();
        }

        var categoria = new Categoria()
        {
            CategoriaId = categoriaDTO.CategoriaId,
            Nome = categoriaDTO.Nome,
            ImagemUrl = categoriaDTO.ImagemUrl
        };

        var updatedCategoria = _unitOfWork.CategoriaRepository.Update(categoria);
        _unitOfWork.Commit();

        var updatedCategoriaDTO = new CategoriaDTO()
        {
            CategoriaId = updatedCategoria.CategoriaId,
            Nome = updatedCategoria.Nome,
            ImagemUrl = updatedCategoria.ImagemUrl
        };

        return Ok(updatedCategoriaDTO);
    }


    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound();
        }

        var deletedCategoria = _unitOfWork.CategoriaRepository.Delete(categoria);
        _unitOfWork.Commit();

        var deletedCategoriaDTO = new CategoriaDTO()
        {
            CategoriaId = deletedCategoria.CategoriaId,
            Nome = deletedCategoria.Nome,
            ImagemUrl = deletedCategoria.ImagemUrl
        };

        return Ok(deletedCategoriaDTO);
    }
}