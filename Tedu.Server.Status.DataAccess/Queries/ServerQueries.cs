using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tedu.Server.Status.DataAccess.Queries
{
    public class ServerQueries : IServerQueries
    {
        private readonly TeduStatusDbContext _dbContext;

        public ServerQueries(TeduStatusDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Entities.Server[]> GetAllServersAsync()
        {
            return _dbContext.Set<Entities.Server>().ToArrayAsync();
        }

        public Task<Entities.Server> GetAsync(int id)
        {
            return _dbContext.Set<Entities.Server>().SingleAsync(x => x.Id == id);
        }

        public Task<Entities.Server> FindAsync(string host)
        {
            return _dbContext.Set<Entities.Server>().FirstOrDefaultAsync(x => x.Host == host);
        }
    }
}