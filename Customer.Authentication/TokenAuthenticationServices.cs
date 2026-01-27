using Dapper;
using Customer_Union.Infrastructure.Data;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Customer_Union.Authentication
{
    public class TokenAuthenticationServices(string issuerSigningKey, IDbConnectionFactory connectionFactory) : ITokenAuthenticationServices
    {
        private const string REVOKE_TOKEN_QUERY = @"
            INSERT INTO InvalidTokens (jti, createdAt, note)
            VALUES (@jti, @createdAt, @note)";

        private const string CHECK_TOKEN_BLACKLIST_QUERY = "SELECT TOP 1 1 FROM InvalidTokens WHERE jti = @jti";

        public string GenerateTokenAsync(string clientCode)
        {
            var tokenExpiryMonths = 6; // Default token expiry in months
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var expires = now.AddMonths(tokenExpiryMonths);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, clientCode),
                new Claim(JwtRegisteredClaimNames.Name, "Application"),
                new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string tokenValue, out SecurityToken? token, out ClaimsPrincipal? principal)
        {
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                principal = handler.ValidateToken(tokenValue, validationParameters, out token);
                return true;
            }
            catch (Exception)
            {
                principal = null;
                token = null;
                return false;
            }
        }

        public bool RevokeTokenAsync(string jti)
        {
            using var connection = connectionFactory.CreateConnection();
           
            var result = connection.Execute(
                REVOKE_TOKEN_QUERY, 
                new
                {
                    jti,
                    createdAt = DateTime.Now,
                    note = "Token revoked"
                });

            return result == 1 ? true : false;
        }

        public bool IsRevokedToken(string jti)
        {
            using var connection = connectionFactory.CreateConnection();

            return connection.ExecuteScalar<bool>(CHECK_TOKEN_BLACKLIST_QUERY, new { jti });

        }
    }
}
