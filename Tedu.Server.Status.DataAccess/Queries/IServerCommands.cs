using System.Threading.Tasks;

namespace Tedu.Server.Status.DataAccess.Queries
{
    public interface IServerCommands
    {
        Task AddAsync(Entities.Server server);
    }
}