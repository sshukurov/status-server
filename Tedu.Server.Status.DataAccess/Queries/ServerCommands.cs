using System.Threading.Tasks;

namespace Tedu.Server.Status.DataAccess.Queries
{
    public class ServerCommands : IServerCommands
    {
        private readonly TeduStatusDbContext _dbContext;

        public ServerCommands(TeduStatusDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(Entities.Server server)
        {
            _dbContext.Add(server);
            return _dbContext.SaveChangesAsync();
        }
    }
}