using System.ComponentModel.DataAnnotations;

namespace CatalogoApi.DTOs;

public class ProdutoDTORequest
{
    [Range(1, 9999, ErrorMessage = "O estoque deve estar entre 1 e 9999")]
    public float Estoque { get; set; }
    // public DateTime? DataCadastro { get; set; }

    [Range(1, 1000, ErrorMessage = "O preço deve estar entre 1 e 1000")]
    public decimal Preco { get; set; }

    // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    // {
    //     if (DataCadastro is not null)
    //     {
    //         if(DataCadastro?.Date <= DateTime.Now.Date)
    //             yield return new ValidationResult(
    //                 "A data deve ser maior que a data atual", 
    //                 new[] { nameof(this.DataCadastro) }
    //             );
    //     }
    // }
}