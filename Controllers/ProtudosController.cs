using Microsoft.AspNetCore.Mvc;
using CatalogoApi.Models;
using CatalogoApi.Repository;
using CatalogoApi.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CatalogoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProtudosController : ControllerBase
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    // private readonly IConfiguration _config;

    public ProtudosController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
    public ActionResult<IEnumerable<ProdutoDTOResponse>> GetAll()
    {
        var produtos = _unitOfWork.ProdutoRepository.GetAll();

        if(produtos is null)
            return NotFound();

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTOResponse>>(produtos);

        return Ok(produtosDTO);
    }


    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> GetById(int id)
    {
        var produto =  _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

        if(produto is null)
            return NotFound();

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

        return Ok(produtoDTO);
    }


    [HttpGet("categoria/{id:int}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetByCategoriaId(int id)
    {
        var produtos =  _unitOfWork.ProdutoRepository.GetProdutosByCategoria(id);

        if(produtos is null)
            return NotFound();

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDTO);
    }


    [HttpPost]
    public ActionResult<ProdutoDTO> Add(ProdutoDTO produtoDTO)
    {

        if (produtoDTO is null)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDTO);

        var newProduto = _unitOfWork.ProdutoRepository.Create(produto);
        _unitOfWork.Commit();

        var newProdutoDTO = _mapper.Map<ProdutoDTO>(newProduto);

        // Location Header 
        return new CreatedAtRouteResult("ObterProduto", new { id = newProdutoDTO.ProdutoId }, newProdutoDTO);
    }


    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> Update(int id, ProdutoDTO produtoDTO)
    {
        if (id != produtoDTO.ProdutoId)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDTO);

        var updatedProduto = _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();

        var updatedProdutoDTO = _mapper.Map<ProdutoDTO>(updatedProduto);

        return Ok(updatedProdutoDTO);

    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);
        
        if (produto is null)
            return NotFound();

        var deletedProduto = _unitOfWork.ProdutoRepository.Delete(produto);
        _unitOfWork.Commit();

        var deletedProdutoDTO = _mapper.Map<ProdutoDTO>(deletedProduto);

        return Ok(deletedProdutoDTO);

    }

    [HttpPatch("{id:int}")]
    public ActionResult<ProdutoDTOResponse> Patch(
        int id, JsonPatchDocument<ProdutoDTORequest> patchProdutoDTO
    )
    {
        if (patchProdutoDTO is null || id <= 0)
            return BadRequest();
        
        var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound();
        
        var produtoDTORequest = _mapper.Map<ProdutoDTORequest>(produto);

        patchProdutoDTO.ApplyTo(produtoDTORequest, ModelState);

        if(!ModelState.IsValid || !TryValidateModel(produtoDTORequest))
            return BadRequest(ModelState);
        
        _mapper.Map(produtoDTORequest, produto);
        
        _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();

        return Ok(_mapper.Map<ProdutoDTOResponse>(produto));
    }

}