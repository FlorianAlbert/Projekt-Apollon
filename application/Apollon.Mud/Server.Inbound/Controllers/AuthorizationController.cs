using System;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Shared.Dungeon.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Apollon.Mud.Shared.UserManagement.Authorization;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    /// <summary>
    /// ToDo Tests
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        /// <summary>
        /// ToDo
        /// </summary>
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="authorizationService"></param>
        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="authorizationRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/login")]
        [ProducesResponseType(typeof(AuthorizationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] AuthorizationRequestDto authorizationRequestDto)
        {
            var loginResult = _authorizationService.Login(authorizationRequestDto.UserEmail, authorizationRequestDto.Password);

            if (loginResult.Status == LoginResultStatus.Unauthorized) return Unauthorized();
            else if (loginResult.Status == LoginResultStatus.OK)
            {
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
            else return BadRequest();
        }
    }
}
