using System.Threading.Tasks;

namespace Tedu.Server.Status.DataAccess.Queries
{
    public interface IServerQueries
    {
        Task<Entities.Server[]> GetAllServersAsync();
        Task<Entities.Server> GetAsync(int id);
        Task<Entities.Server> FindAsync(string host);
    }
}
