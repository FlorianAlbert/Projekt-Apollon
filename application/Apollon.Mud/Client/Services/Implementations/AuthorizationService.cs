using Apollon.Mud.Client.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using Apollon.Mud.Shared.UserManagement.Authorization;
using System.Threading;
using System.Net;
using System.Threading.Tasks;
using Apollon.Mud.Shared.UserManagement.Registration;
using Apollon.Mud.Shared.UserManagement.Password;
using Apollon.Mud.Client.Data.Account;

namespace Apollon.Mud.Client.Services.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        /// <summary>
        /// The Rest Http Client injected into the class
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// Creates Cancellation Tokens for each Http Request
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Contains a users information
        /// </summary>
        public UserContext CurrentUserContext { get; set; }

        /// <summary>
        /// The custom AuthenticationProvider to authorize users after logging in
        /// </summary>
        public CustomAuthenticationStateProvider AuthenticationProvider { get; set; }

        /// <summary>
        /// This service contains all the logic
        /// for entitlement management.
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="userContext"></param>
        /// <param name="authenticationStateProvider">The ASP injected to provide authorization</param>
        public AuthorizationService(IHttpClientFactory httpClientFactory, UserContext userContext, CustomAuthenticationStateProvider authenticationStateProvider)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            CancellationTokenSource = new CancellationTokenSource();
            CurrentUserContext = userContext;
            AuthenticationProvider = authenticationStateProvider;
        }
        
        /// <summary>
        /// The method contains the logic
        /// to log in to the website.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="secret"></param>
        /// <returns>wether the Login was successfull</returns>
        public async Task<bool> Login(string userId, string secret)
        {
            AuthorizationRequestDto userCredentials = new AuthorizationRequestDto();
            userCredentials.UserEmail = userId;
            userCredentials.Password = secret;

            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/authorization/login", userCredentials, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                AuthorizationResponseDto responseDto = await response.Content.ReadFromJsonAsync<AuthorizationResponseDto>();
                CurrentUserContext.DungeonUserContext = responseDto.DungeonUserDto;
                CurrentUserContext.Token = responseDto.Token;
                CurrentUserContext.IsAuthorized = true;
                AuthenticationProvider.NotifyStateChanged();
                return true;
            }
            return false;
        }

        /// <summary>
        /// The player will leave the game with the avatar.
        /// </summary>
        public void LogOut()
        {
            CurrentUserContext.IsAuthorized = false;
            CurrentUserContext.Token = string.Empty;
            CurrentUserContext.DungeonUserContext = null;
            AuthenticationProvider.NotifyStateChanged();
        }
    }
}
