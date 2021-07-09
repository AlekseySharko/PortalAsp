using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace PortalAsp.EfCore.Identity.JwtAuth
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly SymmetricSecurityKey _key;

        public JwtGenerator(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Authentication:JwtKey"]));
        }

        public string CreateToken(string id)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, id) };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenOptions = new JwtSecurityToken(
                "https://localhost:5000",
                "http://localhost:4200/",
                claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials
            );
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.WriteToken(tokenOptions);

            return token;
        }
    }
}