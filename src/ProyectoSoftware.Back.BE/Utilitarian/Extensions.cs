using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProyectoSoftware.Back.BE.Dtos;
using ProyectoSoftware.Back.BE.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoSoftware.Back.BE.Utilitarian
{
    public static class Extensions
    {

        public static TokenJWT CreatedToken(this string key, UserDto user)
        {            
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var signinkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.UserId)??""),
                    new Claim(ClaimTypes.Email, user.Email??""),
                    new Claim(ClaimTypes.Role, user.Rol??""),
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(signinkey, SecurityAlgorithms.HmacSha256Signature),
                    Issuer = null,
                    Audience = null
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                TokenJWT tokenCreate = new() { Token = tokenString , DateExpirated= DateTime.UtcNow.AddHours(1)};
                return tokenCreate;
            }
            catch (Exception)
            {
                throw;
            }
        }
    public static string Encrypted(this string password)
    {
        IConfiguration configuration = Configuration.GetConfiguration();
        var salEncrypt = configuration["SalEncrypt"];
        password = salEncrypt + password;
        SHA256 sha256 = SHA256.Create();
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] stream;
        StringBuilder sb = new StringBuilder();
        stream = sha256.ComputeHash(encoding.GetBytes(password));
        for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
        return sb.ToString();
    }
    }

}
