using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogApi.Extensions;
using BlogApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Services;

public class TokenService
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler(); // cria instancia do token handler

        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey); // cria uma chave para ser utilizada no token descriptor

        var claims = user.GetClaims();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature) //chave para encriptar e desencriptar
        }; // cria uma especificação desse token

        var token = tokenHandler.CreateToken(tokenDescriptor); // cria o token

        return tokenHandler.WriteToken(token); //escreva uma string baseada no token
    }
}