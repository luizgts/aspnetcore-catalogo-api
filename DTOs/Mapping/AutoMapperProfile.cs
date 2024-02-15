using AutoMapper;
using CatalogoApi.Models;

namespace CatalogoApi.DTOs.Mapping;
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Left type is the Source / Right type is the Destination
        CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        CreateMap<Produto, ProdutoDTO>().ReverseMap();
        CreateMap<Produto, ProdutoDTORequest>().ReverseMap();
        CreateMap<Produto, ProdutoDTOResponse>().ReverseMap();
    }
}