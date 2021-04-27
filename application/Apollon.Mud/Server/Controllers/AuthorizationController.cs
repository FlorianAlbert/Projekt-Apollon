using System;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Shared.Dungeon.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Apollon.Mud.Shared.UserManagement.Authorization;

namespace Apollon.Mud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private IAuthorizationService _AuthorizationService;



        [HttpPost]
        [Route("/login")]
        [ProducesResponseType(typeof(AuthorizationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Login([FromBody] AuthorizationRequestDto authorizationRequestDto)
        {
            var loginResult = _AuthorizationService.Login(authorizationRequestDto.UserEmail, authorizationRequestDto.Password);

            return Ok(new AuthorizationResponseDto
            {
                Token = loginResult.Token,
                DungeonUserDto = new DungeonUserDto
                {
                    Email = loginResult.User.Email,
                    EmailConfirmed = loginResult.User.EmailConfirmed,
                    LastActive = loginResult.User.LastActive
                }
            });
        }
    }
}
