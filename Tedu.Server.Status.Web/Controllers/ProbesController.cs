using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tedu.Server.Status.DataAccess.Models;
using Tedu.Server.Status.DataAccess.Queries;

namespace Tedu.Server.Status.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProbesController : ControllerBase
    {
        private readonly IProbeQueries _probeQueries;

        public ProbesController(IProbeQueries probeQueries)
        {
            _probeQueries = probeQueries;
        }

        [HttpGet("latest")]
        public async Task<ActionResult<ServerLatestProbesModel>> GetLatestAsync()
        {
            return await _probeQueries.GetLatestProbesAsync();
        }
    }
}
