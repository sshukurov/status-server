using Tedu.Server.Status.DataAccess.Entities;

namespace Tedu.Server.Status.DataAccess.Models
{
    public class ServerModel
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public ProbeModel[] Probes { get; set; }
        public BackupModel Backup { get; set; }
    }
}