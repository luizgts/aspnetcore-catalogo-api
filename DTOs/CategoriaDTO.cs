using System.ComponentModel.DataAnnotations;

namespace CatalogoApi.DTOs;
public class CategoriaDTO
{
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }
    
    [Required]
    [StringLength(256)]
    public string? ImagemUrl { get; set; }
}