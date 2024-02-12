using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CatalogoApi.Models;
using CatalogoApi.Filters;
using CatalogoApi.Repository;
using CatalogoApi.DTOs;
using CatalogoApi.DTOs.Mapping;

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

        return Ok(categorias.ToDTOList());
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

        var categoriaDTO = categoria.ToDTO();

        return Ok(categoriaDTO);
    }


    [HttpPost]
    public ActionResult<CategoriaDTO> Add(CategoriaDTO categoriaDTO)
    {

        if (categoriaDTO is null)
        {
            return BadRequest();
        }

        var categoria = categoriaDTO.ToModel() ?? new Categoria();

        var newCategoria = _unitOfWork.CategoriaRepository.Create(categoria);
        _unitOfWork.Commit();

        var newCategoriaDTO = newCategoria.ToDTO() ?? new CategoriaDTO();
        
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

        var categoria = categoriaDTO.ToModel() ?? new Categoria();

        var updatedCategoria = _unitOfWork.CategoriaRepository.Update(categoria);
        _unitOfWork.Commit();

        var updatedCategoriaDTO = updatedCategoria.ToDTO() ?? new CategoriaDTO();

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

        var deletedCategoriaDTO = deletedCategoria.ToDTO();

        return Ok(deletedCategoriaDTO);
    }
}