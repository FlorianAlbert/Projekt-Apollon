﻿using System;
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
    public class AuthorizationService : IAuthorizationService
    {
        /// <summary>
        /// The service for user-db functionality.
        /// </summary>
        private readonly IUserDbService _userDbService;

        /// <summary>
        /// Manager to validate the login.
        /// </summary>
        private readonly SignInManager<DungeonUser> _signInManager;

        /// <summary>
        /// ToDo
        /// </summary>
        private readonly UserManager<DungeonUser> _userManager;

        /// <summary>
        /// Configuration to load the settings.
        /// </summary>
        private IConfiguration _configuration;

        /// <summary>
        /// The secret to generate a JWT.
        /// </summary>
        private readonly string _tokenSecret;

        public AuthorizationService(IUserDbService userDbService, SignInManager<DungeonUser> signInManager, UserManager<DungeonUser> userManager, IConfiguration configuration)
        {
            _userDbService = userDbService;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _tokenSecret = _configuration.GetSection("AuthorizationToken").Value;  //ToDo passt es so?
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
                if (await _userManager.IsInRoleAsync(user, role)) //ToDo Aufruf in UserDbService kapseln für konsistente Logik
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