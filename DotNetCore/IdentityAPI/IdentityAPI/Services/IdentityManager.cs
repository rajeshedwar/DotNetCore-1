using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityAPI.Intrastructures;
using IdentityAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAPI.Services
{
    public class IdentityManager : IIdentityManager
    {
        private IdentityDbContext db;
        private IConfiguration config;
        public IdentityManager(IdentityDbContext dbContext, IConfiguration configuration)
        {
            this.db = dbContext;
            this.config = configuration;
        }

        public async Task<dynamic> AddUserAsync(User user)
        {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return new
            {
                user.Id,
                user.FistName,
                user.LastName,
                UserId = user.Email,
                user.ContactNo,
            };
        }

        public string ValidateUser(Login login)
        {
            var result = db.Users.SingleOrDefault(c => c.Email == login.Email && c.Password == login.Password);
            if (result != null)
            {
                string token = GenerateToken(login.Email, login.Password);
                return token;
            }
            return null;
        }

        private string GenerateToken(string userid, string pwd)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,userid),
                new Claim(JwtRegisteredClaimNames.Email,userid),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");
            if (userid == "siva@gmail.com")
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("Jwt:Secret")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: config.GetValue<string>("Jwt:Issuer"),
                audience: config.GetValue<string>("Jwt:Audience"),
                claims: claimsIdentity.Claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
