using Persistence.Context;
using Associated.Application.Common.Model.Response;
using Associated.Application.Auth.Service;
using Microsoft.EntityFrameworkCore;
using Application.Jwt.Extension;
using System.Security.Claims;
using Associated.Jwt.Interface.Service;
using Application.Auth.Model.Request;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Application.User.Model.Request;
using Application.User.Service;

namespace Application.Auth.Service
{
    public class AuthService
    {
        private IConfiguration _config;
        private readonly IJwtService _jwtService;
        private readonly UserService _userService;
        private readonly RelationalContext _relationalContext;
        public AuthService(IConfiguration config, IJwtService jwtService, UserService userService, RelationalContext relationalContext)
        {
            _config = config;
            _jwtService = jwtService;
            _userService = userService;
            _relationalContext = relationalContext;
        }

        public async Task<ResponseModel<bool>> Register([FromBody] CreateUserRequestModel request)
        {
            var response = await _userService.Create(request);
            return response;
        }


        public async Task<ResponseModel<string>> Login(LoginRequestModel request)
        {
            var response = new ResponseModel<string>();

            var user = await _relationalContext.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Email == request.Email);
            if (user is not null)
            {
                if (PasswordService.VerifyPassword(user.Password, request.Password))
                {
                    var claims = new List<Claim> {
                        new Claim(Claims.Id, user.Id.ToString()),
                        new Claim(Claims.FirstName, user.FirstName),
                        new Claim(Claims.LastName, user.LastName),
                        new Claim(Claims.Role, user.Role.Name)
                    };
                    response.Result = _jwtService.GenerateToken(claims);
                    response.StatusCode = 200;
                }
                else
                {
                    response.AddError("Password", "Invalid password.");
                }
            }
            else
            {
                response.AddError("Email", "Invalid email.");
            }

            return response;
        }

        public ResponseModel<string> RefreshToken(RefreshRequest request)
        {
            var response = new ResponseModel<string>();

            var refreshTokenClaims = ValidateRefreshToken(request.RefreshToken);

            if (refreshTokenClaims is null)
            {
                response.AddError("Invalid Token", "Invalid JWT Token");
                return response;
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var newToken = GenerateToken(refreshTokenClaims, credentials);

            response.StatusCode = 200;
            response.Result = newToken;

            return response;
        }
        private string GenerateToken(List<Claim> claims, SigningCredentials credentials)
        {
            var Sectoken = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(5), // You may adjust the expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(Sectoken);
        }
        private List<Claim> ValidateRefreshToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)),
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
            };

            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out _);
                return principal.Claims.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
