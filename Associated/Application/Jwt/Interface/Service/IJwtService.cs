using System.Security.Claims;

namespace Associated.Jwt.Interface.Service
{
    public interface IJwtService
    {
        public string GenerateToken(List<Claim> claims);
        public string RefreshToken();
    }
}
