using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Apollon.Mud.Server.Model.Implementations.User;
using Apollon.Mud.Shared.Dungeon.User;
using Apollon.Mud.Shared.UserManagement.Password;
using Apollon.Mud.Shared.UserManagement.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    /// <summary>
    /// Controller which offers routes for all changes which are connected to an user, like registration, password-resets and so on.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region member
        /// <summary>
        /// The service to execute the requested changes.
        /// </summary>
        private readonly IUserService _userService;
        #endregion


        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region methods
        /// <summary>
        /// Tries to register a use with the email and password from the RegistrationRequestDto.
        /// </summary>
        /// <param name="registrationRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("registration/request")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegistrateUser([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            if (registrationRequestDto is null) return BadRequest();

            var succeeded = await _userService.RequestUserRegistration(registrationRequestDto.UserEmail,
                registrationRequestDto.Password);

            if (succeeded) return Ok();
            return BadRequest();
        }

        /// <summary>
        /// Tries to confirm the user with the given userId and token.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("registration/confirmation/{userId}/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmUserRegistration([FromRoute] Guid userId, [FromRoute] string token)
        {
            if (token is null) return BadRequest();

            if (await _userService.ConfirmUserRegistration(userId, token)) return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Deletes the user with the given userId. Can only be called from Admins.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
        {
            if (await _userService.DeleteUser(userId)) return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Returns all registered users. Can only be called from Admins.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("users")]
        [ProducesResponseType(typeof(DungeonUserDto[]), StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            var userDtos = users.Select(x => new DungeonUserDto()
            {
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                LastActive = x.LastActive,
                Id = Guid.Parse(x.Id)
            }).ToArray();
            return Ok(userDtos);
        }

        /// <summary>
        /// Returns the user with the given userId. Can only be called from Admins.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("user/{userId}")]
        [ProducesResponseType(typeof(DungeonUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId)
        {
            var user = await _userService.GetUser(userId);
            if (user is null) return BadRequest();
            var userDto = new DungeonUserDto
            {
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                LastActive = user.LastActive,
                Id = Guid.Parse(user.Id)
            };
            return Ok(userDto);
        }

        /// <summary>
        /// Requests a password-reset for the user with the given email in the RequestPasswordResetDto.
        /// </summary>
        /// <param name="requestPasswordResetDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("password/reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDto requestPasswordResetDto)
        {
            if (requestPasswordResetDto is null) return BadRequest();

            if (await _userService.RequestPasswordReset(requestPasswordResetDto.UserEmail)) return Ok();

            return BadRequest();
        }

        /// <summary>
        /// Confirms the password-reset from the user with the given userId.
        /// </summary>
        /// <param name="passwordResetConfirmationDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("password/confirm/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmPasswordReset([FromBody] PasswortResetConfirmationDto passwordResetConfirmationDto, [FromRoute] Guid userId)
        {
            if (passwordResetConfirmationDto is null) return BadRequest();

            var succeeded = await _userService.ConfirmPasswordReset(userId, passwordResetConfirmationDto.Token, passwordResetConfirmationDto.NewPassword);

            if (succeeded) return Ok();
            return BadRequest();
        }

        /// <summary>
        /// Changes the password from the user with the given userId.
        /// </summary>
        /// <param name="changePasswordDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("password/change")]
        [Authorize(Roles = "Player, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId)) return BadRequest();

            var user = await _userService.GetUser(userId);
            if (user is null) return BadRequest();

            if (changePasswordDto is null) return BadRequest(); 

            var succeeded = await _userService.ChangePassword(userId, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

            if (succeeded) return Ok();
            return BadRequest();
        }
        #endregion
    }
}
