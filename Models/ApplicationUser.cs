using Microsoft.AspNetCore.Identity;

namespace CatalogoApi.Models;

// Essa classe vai extender a tabela AspNetUsers com novos campos
public class ApplicationUser: IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
