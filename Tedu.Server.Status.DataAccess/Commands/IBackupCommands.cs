using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tedu.Server.Status.DataAccess.Entities;

namespace Tedu.Server.Status.DataAccess.Commands
{
    public interface IBackupCommands
    {
        Task AddAsync(Backup backup);
        Task RemoveAsync(Expression<Func<Backup, bool>> predicate);
    }
}
