using Tedu.Server.Status.DataAccess.Entities;

namespace Tedu.Server.Status.DataAccess.Models
{
    public class ProbeModel
    {
        public int Id { get; set; }
        public ProbeType Type { get; set; }
        public ProbeResult Result { get; set; }
    }
}
