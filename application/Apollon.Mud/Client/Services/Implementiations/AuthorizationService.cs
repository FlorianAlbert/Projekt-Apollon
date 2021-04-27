using Apollon.Mud.Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementiations
{
    public class AuthorizationService : IAuthorizationService
    {
        public HttpClient HttpClient { get; }

        public AuthorizationService(IHttpClientFactory httpClientFactory)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
        }

        public void Login(string userId, string secret)
        {
            //HttpClient.PostAsJsonAsync < NutzerDtoMitInfos>()

            throw new NotImplementedException();
            //TODO Aus userId und secret ein UserDto machen und das an PostAsJsonAsync senden, HTTPRequest gibt wieder ob Anmeldung erfolgreich und gibt JWT, das können wir in UnserContext speichern, damit user angemeldet bleibt
        }
    }
}
