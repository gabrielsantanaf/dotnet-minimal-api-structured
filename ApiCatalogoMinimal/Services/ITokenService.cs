using ApiCatalogoMinimal.Models;

namespace ApiCatalogoMinimal.Services
{
    public interface ITokenService
    {
        string GerarToken(string Key, string issuer, string audience, UserModel user);
    }
}
