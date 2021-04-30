using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        /// <summary>
        /// ToDo
        /// </summary>
        private IUserDBService _userDbService;

        /// <summary>
        /// ToDo
        /// </summary>
        private SignInManager<DungeonUser> _signInManager;

        /// <summary>
        /// ToDo
        /// </summary>
        private UserManager<DungeonUser> _userManager;

        /// <summary>
        /// ToDo
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// ToDo
        /// </summary>
        private string _tokenSecret; //ToDo über Config eintragen lassen

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userDbService"></param>
        /// <param name="signInManager"></param>
        public AuthorizationService(IUserDBService userDbService, SignInManager<DungeonUser> signInManager, UserManager<DungeonUser> userManager, IConfiguration configuration)
        {
            _userDbService = userDbService;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _tokenSecret = _configuration.GetSection("AuthorizationToken").Value;
        }

        public async Task<LoginResult> Login(string email, string secret)
        {
            var user = await _userDbService.GetUserByEmail(email);
            if (user == null) return new LoginResult
            {
                Status = LoginResultStatus.BadRequest
            };

            var task = await _signInManager.CheckPasswordSignInAsync(user, secret, false);
            if (task.Succeeded)
            {
                var token = await GenerateToken(user);

                return new LoginResult
                {
                    User = user,
                    Token = token,
                    Status = LoginResultStatus.OK
                };
            }

            return new LoginResult
            {
                Status = LoginResultStatus.Unauthorized
            };
        }

        //ToDo in UML übertragen
        private async Task<string> GenerateToken(DungeonUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSecret);
            var listClaims = new List<Claim>()
            {
                new Claim("UserId", user.Id),
                new Claim("SessionId", Guid.NewGuid().ToString())
            };

            foreach (var role in Enum.GetNames<Roles>())
            {
                if (await _userManager.IsInRoleAsync(user, role))
                {
                    listClaims.Add(new Claim(ClaimTypes.Role, role));
                }
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(listClaims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}