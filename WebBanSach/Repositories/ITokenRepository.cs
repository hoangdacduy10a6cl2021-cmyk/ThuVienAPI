using Microsoft.AspNetCore.Identity;

namespace WebBanSach.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}