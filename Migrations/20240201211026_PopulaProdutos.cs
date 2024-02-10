using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogoApi.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { 
                    "Nome", 
                    "Descricao", 
                    "Preco", 
                    "ImagemUrl", 
                    "Estoque", 
                    "DataCadastro", 
                    "CategoriaId"
                },
                values: new object[,] {
                    { "Coca-Cola Diet", "Refrigerante 350ml", 5.45, "cocacola.jpg", 50.0F, DateTime.Now, 1 },
                    { "Lanche de Atum", "Atum com Maionese", 8.50, "atum.jpg", 10.0F,  DateTime.Now, 2 },
                    { "Pudim de Leite", "Pudim de leite 100g", 6.75, "pudim.jpg", 20.0F,  DateTime.Now, 3 },
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Produtos");
        }
    }
}
