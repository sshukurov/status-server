using System;

namespace Tedu.Server.Status.DataAccess.Models
{
    public class ServerLatestProbesModel
    {
        public DateTime CheckedDateTimeUtc { get; set; }
        public ServerModel[] Servers { get; set; }
    }
}