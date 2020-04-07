using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Tedu.Server.Status.DataAccess.Queries;
using Tedu.Server.Status.Web.Configuration;

namespace Tedu.Server.Status.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private const string SettingsAccessTokenHeader = "Settings-Token";

        private readonly IServerQueries _serverQueries;
        private readonly IServerCommands _serverCommands;
        private readonly IHostSettings _hostSettings;

        public ServersController(
            IServerQueries serverQueries,
            IServerCommands serverCommands,
            IHostSettings hostSettings)
        {
            _serverQueries = serverQueries;
            _serverCommands = serverCommands;
            _hostSettings = hostSettings;
        }

        [HttpGet]
        public async Task<ActionResult<DataAccess.Entities.Server[]>> GetAsync()
        {
            return await _serverQueries.GetAllServersAsync();
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync(string host)
        {
            await _serverCommands.AddAsync(new DataAccess.Entities.Server { Host = host });
            return Ok();
        }

        [HttpGet("{id:int:min(1)}/settings")]
        public async Task<ActionResult> GetSettingsAsync(int id)
        {
            DataAccess.Entities.Server server = await _serverQueries.GetAsync(id);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://{server.Host}/api/settings"),
                Headers = { 
                    { HeaderNames.Accept, MediaTypeNames.Application.Json },
                    { SettingsAccessTokenHeader, _hostSettings.SettingsAccessToken }
                }
            };
            using var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest();
            }
            string settings = await response.Content.ReadAsStringAsync();
            return Ok(settings);
        }
    }
}
