using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tedu.Server.Status.DataAccess.Entities;

namespace Tedu.Server.Status.DataAccess.Commands
{
    public class BackupCommands : IBackupCommands
    {
        private readonly TeduStatusDbContext _dbContext;

        public BackupCommands(TeduStatusDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Backup backup)
        {
            await _dbContext.AddAsync(backup);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Expression<Func<Backup, bool>> predicate)
        {
            List<Backup> backups = await _dbContext.Set<Backup>().Where(predicate).ToListAsync();
            _dbContext.RemoveRange(backups);
            await _dbContext.SaveChangesAsync();
        }
    }
}
