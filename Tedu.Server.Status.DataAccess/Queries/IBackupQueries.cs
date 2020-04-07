using System.Threading.Tasks;
using Tedu.Server.Status.DataAccess.Models;

namespace Tedu.Server.Status.DataAccess.Queries
{
    public interface IBackupQueries
    {
        Task<BackupModel> GetLatestServerBackupAsync(int serverId);
    }
}
