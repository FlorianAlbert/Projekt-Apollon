using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apollon.Mud.Shared.UserManagement.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Apollon.Mud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        //ToDo wie machen wir die DI?
        private IAuthorizationService _authorizationService;

        [HttpPost]
        [Route("/login")]
        [ProducesResponseType(typeof(AuthorizationResponseDto), StatusCodes.Status200OK)]
        public AuthorizationResponseDto Login([FromBody] AuthorizationRequestDto authorizationRequestDto)
        {
            //ToDo implement
            var loginResult = _authorizationService.Login();
            return null;
        }
    }
}
