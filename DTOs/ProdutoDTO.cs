using System.ComponentModel.DataAnnotations;

namespace CatalogoApi.DTOs;

public class ProdutoDTO
{
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(80, MinimumLength = 5)]
    [PrimeiraLetraMaiuscula]
    public string? Nome { get; set; }

    [Required]
    [StringLength(256, MinimumLength = 5)]
    public string? Descricao { get; set; }

    [Required]
    [Range(1, 1000, ErrorMessage = "O preço deve estar entre 1 e 1000")]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(256)]
    public string? ImagemUrl { get; set; }
    
    public int CategoriaId { get; set; }

}