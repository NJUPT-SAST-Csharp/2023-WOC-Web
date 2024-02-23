using GeneralWiki.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GeneralWiki.Service
{
    public class JwtService
    {
        private static TokenValidationParameters? _tokenValidationParameters;
        private static SecurityKey? _securityKey;

        public JwtService(TokenValidationParameters tokenValidationParameters, SymmetricSecurityKey securityKey)
        {
            _tokenValidationParameters = tokenValidationParameters;
            _securityKey = securityKey;
        }

        //产生Token
        public static string GenerateToken(User user, JwtSetting jwt)
        {
            var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            // 可以添加角色声明
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(jwt.AccessExpiration),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //认证Token
        public static ClaimsPrincipal ValidateToken(string token) 
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }

        }
        
    }
}
