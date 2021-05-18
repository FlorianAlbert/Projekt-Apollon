using System;
using System.Threading.Tasks;
using Apollon.Mud.Server.Model.Implementations;
using Apollon.Mud.Shared.Dungeon.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Apollon.Mud.Shared.UserManagement.Authorization;
using Microsoft.AspNetCore.Authorization;
using IAuthorizationService = Apollon.Mud.Server.Domain.Interfaces.UserManagement.IAuthorizationService;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    /// <summary>
    /// Controller which is responsible for the authorization.
    /// </summary>
    [Route("api/authorization")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        /// <summary>
        /// The service to validate the credentials.
        /// </summary>
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Validates the credentials and if it is successfully returns a AuthorizationResponseDto in the IActionResult.
        /// </summary>
        /// <param name="authorizationRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(AuthorizationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthorizationRequestDto authorizationRequestDto)
        {
            if (authorizationRequestDto is null) return BadRequest();

            var loginResult = await _authorizationService.Login(authorizationRequestDto.UserEmail, authorizationRequestDto.Password);

            if (loginResult.Status == LoginResultStatus.Unauthorized) return Unauthorized();
            if (loginResult.Status == LoginResultStatus.OK)
            {
                return Ok(new AuthorizationResponseDto
                {
                    Token = loginResult.Token,
                    DungeonUserDto = new DungeonUserDto
                    {
                        Email = loginResult.User.Email,
                        EmailConfirmed = loginResult.User.EmailConfirmed,
                        IsAdmin = loginResult.IsAdmin,
                        LastActive = loginResult.User.LastActive,
                        Id = Guid.Parse(loginResult.User.Id)
                    }
                });
            }
            return BadRequest();
        }
    }
}
