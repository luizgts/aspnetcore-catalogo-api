using Microsoft.AspNetCore.Mvc;
using CatalogoApi.Models;
using CatalogoApi.Repository;
using CatalogoApi.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http.HttpResults;
using CatalogoApi.Pagination;
using Newtonsoft.Json;
using X.PagedList;

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
    public async Task<ActionResult<IEnumerable<ProdutoDTOResponse>>> GetAll()
    {
        var produtos = await _unitOfWork.ProdutoRepository.GetAllAsync();

        if(produtos is null)
            return NotFound();

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTOResponse>>(produtos);

        return Ok(produtosDTO);
    }


    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetByFilters([FromQuery] ProdutosParameters produtosParameters)
    {
        var produtos = await _unitOfWork.ProdutoRepository.GetProdutosByFiltersAsync(produtosParameters);

        return ObterProdutos(produtos);
    }

    [HttpGet("filter/preco/pagination")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetByFiltersPreco([FromQuery] ProdutosFilterPreco produtoFilterPreco)
    {
        var produtos = await _unitOfWork.ProdutoRepository.GetProdutosByPrecoAsync(produtoFilterPreco);

        return ObterProdutos(produtos);
    }

    private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(IPagedList<Produto>? produtos)
    {

        if (produtos is null)
            return NotFound();

        var metadata = new
        {
            produtos.Count,
            produtos.PageSize,
            produtos.PageCount,
            produtos.TotalItemCount,
            produtos.HasNextPage,
            produtos.HasPreviousPage
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDTO);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public async Task<ActionResult<ProdutoDTO>> GetById(int id)
    {
        var produto =  await _unitOfWork.ProdutoRepository.GetAsync(p => p.ProdutoId == id);

        if(produto is null)
            return NotFound();

        var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

        return Ok(produtoDTO);
    }


    [HttpGet("categoria/{id:int}")]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetByCategoriaId(int id)
    {
        var produtos =  await _unitOfWork.ProdutoRepository.GetProdutosByCategoriaAsync(id);

        if(produtos is null)
            return NotFound();

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDTO);
    }


    [HttpPost]
    public async Task<ActionResult<ProdutoDTO>> Add(ProdutoDTO produtoDTO)
    {

        if (produtoDTO is null)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDTO);

        var newProduto = _unitOfWork.ProdutoRepository.Create(produto);

        await _unitOfWork.CommitAsync();

        var newProdutoDTO = _mapper.Map<ProdutoDTO>(newProduto);

        // Location Header 
        return new CreatedAtRouteResult("ObterProduto", new { id = newProdutoDTO.ProdutoId }, newProdutoDTO);
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> Update(int id, ProdutoDTO produtoDTO)
    {
        if (id != produtoDTO.ProdutoId)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDTO);

        var updatedProduto = _unitOfWork.ProdutoRepository.Update(produto);

        await _unitOfWork.CommitAsync();

        var updatedProdutoDTO = _mapper.Map<ProdutoDTO>(updatedProduto);

        return Ok(updatedProdutoDTO);

    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> Delete(int id)
    {
        var produto = await _unitOfWork.ProdutoRepository.GetAsync(p => p.ProdutoId == id);
        
        if (produto is null)
            return NotFound();

        var deletedProduto = _unitOfWork.ProdutoRepository.Delete(produto);

        await _unitOfWork.CommitAsync();

        var deletedProdutoDTO = _mapper.Map<ProdutoDTO>(deletedProduto);

        return Ok(deletedProdutoDTO);

    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ProdutoDTOResponse>> Patch(
        int id, JsonPatchDocument<ProdutoDTORequest> patchProdutoDTO
    )
    {
        if (patchProdutoDTO is null || id <= 0)
            return BadRequest();
        
        var produto = await _unitOfWork.ProdutoRepository.GetAsync(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound();
        
        var produtoDTORequest = _mapper.Map<ProdutoDTORequest>(produto);

        patchProdutoDTO.ApplyTo(produtoDTORequest, ModelState);

        if(!ModelState.IsValid || !TryValidateModel(produtoDTORequest))
            return BadRequest(ModelState);
        
        _mapper.Map(produtoDTORequest, produto);
        
        _unitOfWork.ProdutoRepository.Update(produto);
        await _unitOfWork.CommitAsync();

        return Ok(_mapper.Map<ProdutoDTOResponse>(produto));
    }

}