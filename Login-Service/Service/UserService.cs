using Login_Service.Context;
using Login_Service.Models;
using Login_Service.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Login_Service.Service
{
    public class UserService : IUserService
    {
        private readonly UserContext userContext;
        private readonly IConfiguration config;


        public UserService(UserContext userContext, IConfiguration config)
        {
            this.userContext = userContext;
            this.config = config;
        }

        private User GetUser(string userName)
        {
            return (User)userContext.User.FirstOrDefault(e => e.UserName == userName);
        }

        public void Register(User user)
        {
            if(GetUser(user.UserName) != null)
            {
                throw new DuplicateNameException();
            }

            user.Id = new Guid();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            userContext.Add(user);
            userContext.SaveChanges();
        }        

        public string Authenticate(string userName, string password)
        {
            var user = GetUser(userName);
            if (user == null)
            {
                throw new NullReferenceException();
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new UnauthorizedAccessException();
            }

            return GenerateToken(user.Id.ToString(), userName);
        }

        private string GenerateToken(string id, string userName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, id),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            };

            var token = new JwtSecurityToken(config["JWTF:Issuer"],
                config["JWT:Issuer"],
                claims,
                expires: DateTime.Now.Add(TimeSpan.FromMinutes(20)),
                signingCredentials: credentials);

            var encoded = new JwtSecurityTokenHandler().WriteToken(token);

            return encoded;
        }


    }
}
