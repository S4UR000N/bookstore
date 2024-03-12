using Application.Jwt.Extension;
using Associated.Jwt.Interface.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Jwt.Service
{
    public class JwtService : IJwtService
    {
        private IConfiguration _conf;
        public JwtService(IConfiguration conf)
        {
            _conf = conf;
        }

        public string GenerateToken(List<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_conf["Jwt:Issuer"],
              _conf["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(5),
              signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(Sectoken).ToString();
            return token;
        }

        public string RefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
