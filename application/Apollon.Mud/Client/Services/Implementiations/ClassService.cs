using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Shared.Dungeon.Class;
using Apollon.Mud.Shared.Dungeon.Npc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Services.Implementiations
{
    public class ClassService : IClassService
    {
        /// <summary>
        /// TODO
        /// </summary>
        public HttpClient HttpClient { get; }

        /// <summary>
        /// TODO
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="httpClient"></param>
        public ClassService(IHttpClientFactory httpClientFactory)
        {
            HttpClient = httpClientFactory.CreateClient("RestHttpClient");
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="classDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<Guid> CreateNewClass(ClassDto classDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PostAsJsonAsync("api/classes/" + dungeonId, classDto, cancellationToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseGuid = await response.Content.ReadFromJsonAsync<Guid>();
                return responseGuid;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="classDto"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<ClassDto> UpdateClass(ClassDto classDto, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.PutAsJsonAsync("api/classes", classDto, cancellationToken);

            if (response.StatusCode == HttpStatusCode.BadRequest) return await response.Content.ReadFromJsonAsync<ClassDto>();

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteClass(Guid classId, Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.DeleteAsync("api/classes/" + dungeonId + "/" + classId, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <returns></returns>
        public async Task<ICollection<ClassDto>> GetAllClasses(Guid dungeonId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/classes" + dungeonId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ICollection<ClassDto>>();

            return null;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dungeonId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public async Task<ClassDto> GetClass(Guid dungeonId, Guid classId)
        {
            CancellationToken cancellationToken = CancellationTokenSource.Token;

            var response = await HttpClient.GetAsync("api/classes/" + dungeonId + "/" + classId, cancellationToken);

            if (response.StatusCode == HttpStatusCode.OK) return await response.Content.ReadFromJsonAsync<ClassDto>();

            return null;
        }

    }
}
