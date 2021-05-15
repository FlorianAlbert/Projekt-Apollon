using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.User;
using Apollon.Mud.Shared.UserManagement.Password;
using Apollon.Mud.Shared.UserManagement.Registration;

namespace Apollon.Mud.Client.Services.Implementations
{
    public class UserService : IUserService
    {
        /// <inheritdoc cref="IUserService.HttpClient"/>
        public HttpClient HttpClient { get; }

        /// <inheritdoc cref="IUserService.CancellationTokenSource"/>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// This class contains all logic for handling Users between Front- and Backend
        /// </summary>
        /// <param name="httpClientFactory">The HttpClientFactory injected into the service</param>
        /// <param name="userContext">The scoped UserContext of the current connection</param>
        public UserService(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + userContext.Token);
            CancellationTokenSource = new CancellationTokenSource();
        }


        /// <inheritdoc cref="IUserService.RegistrateUser(RegistrationRequestDto)"/>
        public async Task<HttpStatusCode> RegistrateUser(string userId, string secret)
        {
            RegistrationRequestDto userCredentials = new RegistrationRequestDto
            {
                UserEmail = userId,
                Password = secret
            };

            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/user/registration/request", userCredentials, cancellationToken);
            return response.StatusCode;
        }

        /// <summary>
        /// After confirming the email with the link, the registration is done.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>wether the confirmation was successfull</returns>
        public async Task<bool> ConfirmUserRegistration(Guid userId, string token)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsync("api/user/registration/confirmation/" + userId + "/" + token, null, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Calls the UserController to delete the User from the Database
        /// </summary>
        /// <param name="userId">The ID of the User to delete</param>
        /// <param name="dungeonId">The ID of the Dungeon associated with the user</param>
        /// <returns>True if successfull, otherwise false</returns>
        public async Task<bool> DeleteUser(Guid userId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/user/delete/" + userId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Receives all Users from the Database as DungeonUserDtos
        /// </summary>
        /// <returns>A collection of Dungeon-Users if successfull, otherwise null</returns>
        public async Task<ICollection<DungeonUserDto>> GetAllUsers()
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/user/users", cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<DungeonUserDto>>();

            return null;
        }

        /// <summary>
        /// Gets a single User from the Database
        /// </summary>
        /// <param name="dungeonId">The ID of the Dungeon associated with the Dungeon</param>
        /// <param name="userId">The ID of the wanted Dungeon</param>
        /// <returns>An UserDto if succesfull, otherwise null</returns>
        public async Task<DungeonUserDto> GetUser(Guid userId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/user/user/" + userId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<DungeonUserDto>();

            return null;
        }

        /// <summary>
        /// The reset password is sent to the user by email.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>wether the request was successfull</returns>
        public async Task<bool> RequestPasswordReset(string userEmail)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;
            RequestPasswordResetDto resetDto = new RequestPasswordResetDto();
            resetDto.UserEmail = userEmail;

            var response = await HttpClient.PostAsJsonAsync("/api/user/password/reset", resetDto, cancellationToken);
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Confirms the password-reset from the user with the given userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmPasswordReset(string password, string token, Guid userId)
        {
            PasswortResetConfirmationDto passwortResetConfirmationDto = new PasswortResetConfirmationDto
            {
                NewPassword = password,
                Token = token
            };

            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/user/password/confirm/" + userId, passwortResetConfirmationDto, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Changes the password from the user
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(string oldPassword, string newPassword)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;
            ChangePasswordDto changePasswordDto = new ChangePasswordDto
            {
                OldPassword = oldPassword,
                NewPassword = newPassword
            };

            var response = await HttpClient.PostAsJsonAsync("/api/user/password/change/", changePasswordDto, cancellationToken);
            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Changes the Admin Role of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="approved"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> ChangeUserAdmin(Guid userId, bool approved)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("/api/user/admin/" + userId + "?approved=" + approved.ToString().ToLower(), cancellationToken);
            return response.StatusCode;
        }
    }
}

