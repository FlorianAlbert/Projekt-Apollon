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
using Apollon.Mud.Shared.UserManagement.Password;

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

        public CustomAuthenticationStateProvider AuthenticationProvider { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="userContext"></param>
        public AuthorizationService(IHttpClientFactory httpClientFactory, UserContext userContext, CustomAuthenticationStateProvider authenticationStateProvider)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            TokenSource = new CancellationTokenSource();
            CurrentUserContext = userContext;
            AuthenticationProvider = authenticationStateProvider;
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
        /// TODO
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public async Task<bool> Register(string userId, string secret)
        {
            RegistrationRequestDto userCredentials = new RegistrationRequestDto
            {
                UserEmail = userId,
                Password = secret
            };

            CancellationToken cancellationToken = TokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/user/registration/request", userCredentials, cancellationToken);
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmRegistration(Guid userId, string token)
        {
            CancellationToken cancellationToken = TokenSource.Token;

            var response = await HttpClient.PostAsync("api/user/registration/confirmation/" + userId.ToString() + "/" + token, null, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void LogOut()
        {
            CurrentUserContext.IsAuthorized = false;
            CurrentUserContext.Token = string.Empty;
            CurrentUserContext.DungeonUserContext = null;
            AuthenticationProvider.NotifyStateChanged();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public async Task<bool> RequestPasswordReset(string userEmail)
        {
            CancellationToken cancellationToken = TokenSource.Token;
            RequestPasswordResetDto resetDto =new RequestPasswordResetDto();
            resetDto.UserEmail = userEmail;

            var response = await HttpClient.PostAsJsonAsync("/api/password/reset", resetDto, cancellationToken);
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public async Task<bool> ResetPasswordRequest(Guid userId, string token, string secret)
        {
            CancellationToken cancellationToken = TokenSource.Token;
            PasswortResetConfirmationDto confirmationDto = new PasswortResetConfirmationDto
            {
                NewPassword = secret,
                Token = token
            };

            var response = await HttpClient.PostAsJsonAsync("/api/user/password/confirm/" + userId.ToString(), confirmationDto, cancellationToken);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
