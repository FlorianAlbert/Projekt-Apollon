using Apollon.Mud.Client.Data;
using Apollon.Mud.Client.Data.Account;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementations
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public UserContext CurrentUserContext { get; set; }

        public CustomAuthenticationStateProvider(UserContext userContext)
        {
            CurrentUserContext = userContext;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            if (CurrentUserContext.IsAuthorized)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, CurrentUserContext.DungeonUserContext.Email) };
                identity = new ClaimsIdentity(claims, "Server authentication");
            }
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public void NotifyStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

    
}
