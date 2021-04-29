using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Apollon.Mud.Shared.Game.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apollon.Mud.Server.Inbound.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : Controller
    {
        [HttpPost]
        [Authorize]
        [Route("roomMessage")]
        public async Task<IActionResult> PostRoomMessage(ChatMessageDto chatMessageDto)
        {
            var y = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return Ok();
        }
    }
}