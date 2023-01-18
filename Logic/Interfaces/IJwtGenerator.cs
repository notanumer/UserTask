using Core.Entities;
using System.Security.Claims;

namespace Logic.Interfaces
{
    public interface IJwtGenerator
    {
        List<Claim> GetClaims(User user);
        string GenerateJwt(User user);
    }
}
