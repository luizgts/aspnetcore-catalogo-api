using CatalogoApi.Models;

namespace CatalogoApi.DTOs.Mapping;

public static class CategoriaDTOMappingExtensions
{
    public static CategoriaDTO? ToDTO(this Categoria categoria)
    {
        if (categoria is null)
            return null;
        
        return new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        };
    }

    public static Categoria? ToModel(this CategoriaDTO categoriaDTO)
    {
        if (categoriaDTO is null)
            return null;
        
        return new Categoria()
        {
            CategoriaId = categoriaDTO.CategoriaId,
            Nome = categoriaDTO.Nome,
            ImagemUrl = categoriaDTO.ImagemUrl
        };
    }

    public static IEnumerable<CategoriaDTO> ToDTOList(this IEnumerable<Categoria> categorias)
    {
        if (categorias is null || !categorias.Any())
            return new List<CategoriaDTO>();

        return categorias.Select(categoria => new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImagemUrl = categoria.ImagemUrl
        }).ToList();
    }
}