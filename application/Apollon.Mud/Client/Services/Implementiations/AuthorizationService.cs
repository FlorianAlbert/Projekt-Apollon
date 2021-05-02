using Apollon.Mud.Client.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using Apollon.Mud.Shared.UserManagement.Authorization;
using System.Threading;
using System.Net;
using System.Threading.Tasks;
using Apollon.Mud.Client.Data;
using Apollon.Mud.Shared.UserManagement.Registration;

namespace Apollon.Mud.Client.Services.Implementiations
{
    public class AuthorizationService : IAuthorizationService
    {
        /// <summary>
        /// TODO
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public CancellationTokenSource TokenSource { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public UserContext CurrentUserContext { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="userContext"></param>
        public AuthorizationService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            TokenSource = new CancellationTokenSource();
            CurrentUserContext = userContext;
        }
        
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public async Task<bool> Login(string userId, string secret)
        {
            AuthorizationRequestDto userCredentials = new AuthorizationRequestDto();
            userCredentials.UserEmail = userId;
            userCredentials.Password = secret;

            CancellationToken cancellationToken = TokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync(HttpClient.BaseAddress + "POST/api/authorization/login", userCredentials, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                AuthorizationResponseDto responseDto = await response.Content.ReadFromJsonAsync<AuthorizationResponseDto>();
                CurrentUserContext.DungeonUserContext = responseDto.DungeonUserDto;
                CurrentUserContext.Token = responseDto.Token;
                return true;
            }
            return false;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public async Task<bool> Register(string userId, string secret)
        {
            RegistrationRequestDto userCredentials = new RegistrationRequestDto();
            userCredentials.UserEmail = userId;
            userCredentials.Password = secret;

            CancellationToken cancellationToken = TokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync(HttpClient.BaseAddress + "POST/api/user/registration/request", userCredentials, cancellationToken);
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmRegistration(Guid userId)
        {
            CancellationToken cancellationToken = TokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync(HttpClient.BaseAddress + "POST/api/registration/" + userId + "/" + CurrentUserContext.Token, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
