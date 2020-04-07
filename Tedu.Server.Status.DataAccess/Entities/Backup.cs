using System;

namespace Tedu.Server.Status.DataAccess.Entities
{
    public class Backup : EntityWithId
    {
        public int ServerId { get; set; }

        public bool IsStatusOk { get; set; }

        public int BackupsAmount { get; set; }

        public DateTime CreatedDateTimeUtc { get; set; }

        public DateTime LastBackupStartDateTimeUtc { get; set; }

        public DateTime LastBackupEndDateTimeUtc { get; set; }

        public ulong LastBackupSizeBytes { get; set; }

        public int BackupDurationTotalSeconds { get; set; }

        public int BackupDurationCopySeconds { get; set; }

        public DateTime OldestBackupEndDateTimeUtc { get; set; }

        public ulong DiskUsedBytes { get; set; }

        public ulong DiskFreeBytes { get; set; }
    }
}
