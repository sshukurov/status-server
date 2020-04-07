using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tedu.Server.Status.DataAccess.Entities;
using Tedu.Server.Status.DataAccess.Models;

namespace Tedu.Server.Status.DataAccess.Queries
{
    public class ProbeQueries : IProbeQueries
    {
        private readonly TeduStatusDbContext _dbContext;

        public ProbeQueries(TeduStatusDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServerLatestProbesModel> GetLatestProbesAsync()
        {
            DateTime? lastCheckedDateTimeUtc = _dbContext.Set<Probe>()
                .Select(x => (DateTime?)x.CheckedDateTimeUtc)
                .DefaultIfEmpty()
                .Max();
            if (lastCheckedDateTimeUtc == null)
            {
                return null;
            }

            return new ServerLatestProbesModel
            {
                CheckedDateTimeUtc = lastCheckedDateTimeUtc.Value,
                Servers = await _dbContext.Set<Entities.Server>()
                    .OrderBy(x => x.Host)
                    .Select(x => new ServerModel
                    {
                        Id = x.Id,
                        Host = x.Host,
                        Probes = x.Probes
                            .Where(p => p.CheckedDateTimeUtc == lastCheckedDateTimeUtc)
                            .OrderBy(p => p.Type)
                            .Select(p => new ProbeModel
                            {
                                Id = p.Id,
                                Type = p.Type,
                                Result = p.Result
                            }).ToArray(),
                        Backup = x.Backups
                            .OrderByDescending(b => b.CreatedDateTimeUtc)
                            .Select(b => new BackupModel
                            {
                                BackupDurationTotalSeconds = b.BackupDurationTotalSeconds,
                                LastBackupEndDateTimeUtc = b.LastBackupEndDateTimeUtc,
                                BackupsAmount = b.BackupsAmount,
                                LastBackupSizeBytes = b.LastBackupSizeBytes,
                                IsStatusOk = b.IsStatusOk,
                                BackupDurationCopySeconds = b.BackupDurationCopySeconds,
                                OldestBackupEndDateTimeUtc = b.OldestBackupEndDateTimeUtc,
                                LastBackupStartDateTimeUtc = b.LastBackupStartDateTimeUtc,
                                DiskFreeBytes = b.DiskFreeBytes,
                                DiskUsedBytes = b.DiskUsedBytes
                            })
                            .FirstOrDefault()
                    }).ToArrayAsync()
            };
        }
    }
}