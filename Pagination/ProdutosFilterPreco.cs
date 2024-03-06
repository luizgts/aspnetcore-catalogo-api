namespace CatalogoApi.Pagination;

public class ProdutosFilterPreco : QueryStringParameters
{
    public decimal? Preco { get; set; }
    public string? PrecoCriterio { get; set; }
}
