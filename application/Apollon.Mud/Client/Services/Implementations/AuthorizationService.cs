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

namespace Apollon.Mud.Client.Services.Implementiations
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
        /// The user can register with
        /// a password and a email.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="secret"></param>
        /// <returns>wether the registration was successfull</returns>
        public async Task<bool> Register(string userId, string secret)
        {
            RegistrationRequestDto userCredentials = new RegistrationRequestDto
            {
                UserEmail = userId,
                Password = secret
            };

            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/user/registration/request", userCredentials, cancellationToken);
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// After confirming the email with the link, the registration is done.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>wether the confirmation was successfull</returns>
        public async Task<bool> ConfirmRegistration(Guid userId, string token)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsync("api/user/registration/confirmation/" + userId + "/" + token, null, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
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

        /// <summary>
        /// The reset password is sent to the user by email.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>wether the request was successfull</returns>
        public async Task<bool> RequestPasswordReset(string userEmail)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;
            RequestPasswordResetDto resetDto =new RequestPasswordResetDto();
            resetDto.UserEmail = userEmail;

            var response = await HttpClient.PostAsJsonAsync("/api/password/reset", resetDto, cancellationToken);
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// The user is able to set a new password.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="secret"></param>
        /// <returns>wether the reset was successfull</returns>
        public async Task<bool> ResetPassword(Guid userId, string token, string secret)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;
            PasswortResetConfirmationDto confirmationDto = new PasswortResetConfirmationDto
            {
                NewPassword = secret,
                Token = token
            };

            var response = await HttpClient.PostAsJsonAsync("/api/user/password/confirm/" + userId, confirmationDto, cancellationToken);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
