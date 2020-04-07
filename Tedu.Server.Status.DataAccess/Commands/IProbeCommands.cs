using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tedu.Server.Status.DataAccess.Entities;

namespace Tedu.Server.Status.DataAccess.Commands
{
    public interface IProbeCommands
    {
        Task AddRangeAsync(Probe[] probes);
        Task RemoveAsync(Expression<Func<Probe, bool>> predicate);
    }
}
