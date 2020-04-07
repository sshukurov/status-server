using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tedu.Server.Status.DataAccess.Entities
{
    public class Server : EntityWithId
    {
        [Required]
        public string Host { get; set; }

        public IList<Probe> Probes { get; set; }

        public IList<Backup> Backups { get; set; }
    }
}
