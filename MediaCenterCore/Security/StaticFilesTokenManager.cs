using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenterCore.Security
{
    public class StaticFilesTokenManager
    {
        private static readonly string SecretKey = "SiQnRAN3+cpjBZX2txWe9KNrkeJyVhiC+8WYl/tIwFI="; // Use a secure key
        private static readonly SymmetricSecurityKey SecurityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        public static string GenerateToken(string mediaId , string fileName, int expiryMinutes = 30)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("mediaId", mediaId),
                new Claim("fileName", fileName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique ID
            }),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static (bool IsValid, string mediaId, string fileName) ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, // Adjust as needed
                    ValidateAudience = false, // Adjust as needed
                    ValidateLifetime = true,
                    IssuerSigningKey = SecurityKey
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var mediaId = principal.FindFirst("mediaId")?.Value;
                var fileName = principal.FindFirst("fileName")?.Value;

                return (true, mediaId , fileName);
            }
            catch (Exception)
            {
                return (false, null , null) ;
            }
        }
    }


}
