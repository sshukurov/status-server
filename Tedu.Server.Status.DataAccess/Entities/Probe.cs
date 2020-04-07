using System;

namespace Tedu.Server.Status.DataAccess.Entities
{
    public class Probe : EntityWithId
    {
        public int ServerId { get; set; }

        public DateTime CheckedDateTimeUtc { get; set; }

        public ProbeType Type { get; set; }

        public ProbeResult Result { get; set; }

        public Server Server { get; set; }
    }
}
