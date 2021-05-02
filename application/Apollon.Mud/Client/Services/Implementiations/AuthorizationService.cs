using Apollon.Mud.Client.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using Apollon.Mud.Shared.UserManagement.Authorization;
using System.Threading;

namespace Apollon.Mud.Client.Services.Implementiations
{
    public class AuthorizationService : IAuthorizationService
    {
        public HttpClient HttpClient { get; }

        public CancellationTokenSource TokenSource { get; set; }

        public AuthorizationService(IHttpClientFactory httpClientFactory)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
        }

        public void Login(string userId, string secret)
        {
            //HttpClient.PostAsJsonAsync < NutzerDtoMitInfos>()
            AuthorizationRequestDto userCredentials = new AuthorizationRequestDto();
            userCredentials.UserEmail = userId;
            userCredentials.Password = secret;

            CancellationToken cancellationToken = TokenSource.Token;

            var response = HttpClient.PostAsJsonAsync<AuthorizationRequestDto>(HttpClient.BaseAddress + "POST/api/Authorization/...", userCredentials, cancellationToken);

            throw new NotImplementedException();
            //TODO Aus userId und secret ein UserDto machen und das an PostAsJsonAsync senden, HTTPRequest gibt wieder ob Anmeldung erfolgreich und gibt JWT, das können wir in UnserContext speichern, damit user angemeldet bleibt
        }
    }
}
