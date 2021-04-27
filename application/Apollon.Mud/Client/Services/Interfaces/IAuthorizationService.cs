using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Interfaces
{
    public interface IAuthorizationService
    {
        HttpClient HttpClient { get; }

        void Login(string userId, string secret);
    }
}
