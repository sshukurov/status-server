using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tedu.Server.Status.DataAccess.Entities;
using Tedu.Server.Status.DataAccess.Models;

namespace Tedu.Server.Status.DataAccess.Queries
{
    public class BackupQueries : IBackupQueries
    {
        private readonly TeduStatusDbContext _dbContext;

        public BackupQueries(TeduStatusDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<BackupModel> GetLatestServerBackupAsync(int serverId)
        {
            return _dbContext.Set<Backup>()
                .Where(x => x.ServerId == serverId)
                .OrderByDescending(x => x.CreatedDateTimeUtc)
                .Select(x => new BackupModel
                {
                    BackupDurationCopySeconds = x.BackupDurationCopySeconds,
                    BackupDurationTotalSeconds = x.BackupDurationTotalSeconds,
                    LastBackupEndDateTimeUtc = x.LastBackupEndDateTimeUtc,
                    BackupsAmount = x.BackupsAmount,
                    IsStatusOk = x.IsStatusOk,
                    LastBackupSizeBytes = x.LastBackupSizeBytes,
                    LastBackupStartDateTimeUtc = x.LastBackupStartDateTimeUtc,
                    OldestBackupEndDateTimeUtc = x.OldestBackupEndDateTimeUtc,
                    DiskFreeBytes = x.DiskFreeBytes,
                    DiskUsedBytes = x.DiskUsedBytes
                })
                .FirstOrDefaultAsync();
        }
    }
}
