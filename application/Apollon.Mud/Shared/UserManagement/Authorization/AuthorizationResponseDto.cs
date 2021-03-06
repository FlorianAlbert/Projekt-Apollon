using System.Diagnostics.CodeAnalysis;
using Apollon.Mud.Shared.Dungeon.User;
using Newtonsoft.Json;

namespace Apollon.Mud.Shared.UserManagement.Authorization
{
    /// <summary>
    /// Class which represents an authorization request response.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AuthorizationResponseDto
    {
        /// <summary>
        /// The JSON-Web-Token which is generated, if the request was successfully.
        /// </summary>
        [JsonProperty("Token")]
        public string Token { get; set; }

        /// <summary>
        /// The user which was logged in with the credentials.
        /// </summary>
        [JsonProperty("DungeonUserDto")]
        public DungeonUserDto DungeonUserDto { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is AuthorizationResponseDto aRDto)
            {
                return Token == aRDto.Token 
                       && DungeonUserDto.Equals(aRDto.DungeonUserDto);
            }
            return base.Equals(obj);
        }
    }
}