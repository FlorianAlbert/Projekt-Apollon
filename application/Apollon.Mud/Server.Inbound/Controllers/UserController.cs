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
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// ToDo
        /// </summary>
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="registrationRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/registration/request")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Player, Admin")]
        public async Task<IActionResult> RegistrateUser([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var succeeded = await _userService.RequestUserRegistration(registrationRequestDto.UserEmail,
                registrationRequestDto.Password);
            if (succeeded) return Ok();
            return BadRequest();
        }

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/registration/confirmation/{userId}/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin, Player")]
        public async Task<IActionResult> ConfirmUserRegistration([FromRoute] Guid userId, [FromRoute] string token)
        {
            var succeeded = await _userService.ConfirmUserRegistration(userId, token);
            if (succeeded) return Ok();
            return BadRequest();
        }

        [HttpPost]
        [Route("/delete/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
        {
            var succeeded = await _userService.DeleteUser(userId);
            if (succeeded) return Ok();
            return BadRequest();
        }

        [HttpGet]
        [Route("/users")]
        [ProducesResponseType(typeof(ICollection<DungeonUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            if (users == null || users.Count == 0) return BadRequest();
            var usersDto = users.Select(x => new DungeonUserDto()
            {
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                LastActive = x.LastActive
            });
            return Ok(usersDto);
        }

        [HttpGet]
        [Route("/user/{userId}")]
        [ProducesResponseType(typeof(DungeonUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser([FromRoute] Guid userId)
        {
            var user = await _userService.GetUser(userId);
            if (user == null) return BadRequest();
            var userDto = new DungeonUserDto()
            {
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                LastActive = user.LastActive
            };
            return Ok(userDto);
        }

        [HttpPost]
        [Route("/password/reset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "Admin, Player")]
        public async Task<IActionResult> RequestPasswordResetDto([FromBody] RequestPasswordResetDto requestPasswordResetDto)
        {
            var succeeded = await _userService.RequestPasswordReset(requestPasswordResetDto.UserEmail);
            if (succeeded) return Ok();
            return BadRequest();
        }

        [HttpPost]
        [Route("/password/confirm/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "Admin, Player")]
        public async Task<IActionResult> ConfirmPasswordReset([FromBody] PasswortResetConfirmationDto passwortResetConfirmationDto, [FromRoute] Guid userId)
        {
            var succeeded = await _userService.ConfirmPasswordReset(userId, passwortResetConfirmationDto.Token, passwortResetConfirmationDto.NewPassword);
            if (succeeded) return Ok();
            return BadRequest();
        }

        [HttpPost]
        [Route("/password/reset/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin, Player")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto, [FromRoute] Guid userId)
        {
            var succeeded = await _userService.ChangePassword(userId, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            if (succeeded) return Ok();
            return BadRequest();
        }
    }
}
