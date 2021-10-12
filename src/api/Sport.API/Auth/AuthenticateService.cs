using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sport.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sport.API.Auth
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly JwtConfig _jwtConfig;

        public AuthenticateService(IOptions<JwtConfig> options)
        {
            _jwtConfig = options.Value;
        }

        public string Authenticate(string username, string password)
        {
            if (!_jwtConfig.Username.Equals(username) || !_jwtConfig.Password.Equals(password))
                throw new NotFoundException("username or password is incorrect");

            return GenerateJwt();
        }

        private string GenerateJwt()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddHours(10);

            var token = new JwtSecurityToken(
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
