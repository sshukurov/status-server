using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tedu.Server.Status.DataAccess.Commands;
using Tedu.Server.Status.DataAccess.Entities;
using Tedu.Server.Status.DataAccess.Models;
using Tedu.Server.Status.DataAccess.Queries;
using Tedu.Server.Status.Web.Models;

namespace Tedu.Server.Status.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackupsController : ControllerBase
    {
        private readonly IBackupCommands _backupCommands;
        private readonly IBackupQueries _backupQueries;
        private readonly IServerQueries _serverQueries;

        public BackupsController(
            IBackupQueries backupQueries,
            IServerQueries serverQueries,
            IBackupCommands backupCommands)
        {
            _backupQueries = backupQueries;
            _backupCommands = backupCommands;
            _serverQueries = serverQueries;
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody]BackupInputModel backupInput)
        {
            DataAccess.Entities.Server server = await _serverQueries.FindAsync(backupInput.Host);
            if (server == null)
            {
                return BadRequest();
            }

            await _backupCommands.AddAsync(new Backup
            {
                ServerId = server.Id,
                IsStatusOk = backupInput.IsStatusOk,
                BackupsAmount = backupInput.BackupsAmount,
                LastBackupEndDateTimeUtc = backupInput.LastBackupEndDateTimeUtc,
                BackupDurationTotalSeconds = backupInput.BackupDurationTotalSeconds,
                BackupDurationCopySeconds = backupInput.BackupDurationCopySeconds,
                CreatedDateTimeUtc = DateTime.Now,
                LastBackupSizeBytes = backupInput.LastBackupSizeBytes,
                OldestBackupEndDateTimeUtc = backupInput.OldestBackupEndDateTimeUtc,
                LastBackupStartDateTimeUtc = backupInput.LastBackupStartDateTimeUtc,
                DiskFreeBytes = backupInput.DiskFreeBytes,
                DiskUsedBytes = backupInput.DiskUsedBytes
            });
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<BackupModel>> GetAsync(int serverId)
        {
            BackupModel backup = await _backupQueries.GetLatestServerBackupAsync(serverId);
            return Ok(backup);
        }
    }
}
