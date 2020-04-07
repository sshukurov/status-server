using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tedu.Server.Status.DataAccess.Entities;

namespace Tedu.Server.Status.DataAccess.Commands
{
    public class ProbeCommands : IProbeCommands
    {
        private readonly TeduStatusDbContext _dbContext;

        public ProbeCommands(TeduStatusDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(Probe[] probes)
        {
            await _dbContext.AddRangeAsync((IList<Probe>)probes);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Expression<Func<Probe, bool>> predicate)
        {
            List<Probe> probes = await _dbContext.Set<Probe>().Where(predicate).ToListAsync();
            _dbContext.RemoveRange(probes);
            await _dbContext.SaveChangesAsync();
        }
    }
}