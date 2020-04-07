using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Tedu.Server.Status.DataAccess.Entities;

namespace Tedu.Server.Status.DataAccess
{
    public class TeduStatusDbContext : DbContext
    {
        public TeduStatusDbContext(DbContextOptions<TeduStatusDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapEntityWithId<Entities.Server>(modelBuilder);
            modelBuilder.Entity<Entities.Server>()
                .HasIndex(x => x.Host)
                .IsUnique();
            MapEntityWithId<Probe>(modelBuilder);
            MapEntityWithId<Backup>(modelBuilder);

            modelBuilder.Entity<Entities.Server>().HasMany(x => x.Probes).WithOne(x => x.Server).HasForeignKey(x => x.ServerId);
            modelBuilder.Entity<Entities.Server>().HasMany(x => x.Backups).WithOne().HasForeignKey(x => x.ServerId);

            ApplyLowerCaseNames(modelBuilder);
        }

        // Adapted from https://andrewlock.net/customising-asp-net-core-identity-ef-core-naming-conventions-for-postgresql/
        private static void ApplyLowerCaseNames(ModelBuilder builder)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            foreach (IMutableEntityType entity in builder.Model.GetEntityTypes())
            {
                if (entity.BaseType == null)
                {
                    entity.SetTableName(entity.GetTableName().ToLower(culture));
                }

                foreach (IMutableProperty property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLower(culture));
                }

                foreach (IMutableKey key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToLower(culture));
                }

                foreach (IMutableForeignKey key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().ToLower(culture));
                }

                foreach (IMutableIndex index in entity.GetIndexes())
                {
                    index.SetName(index.GetName().ToLower(culture));
                }
            }
        }

        private static void MapEntityWithId<TEntity>(ModelBuilder builder)
            where TEntity : EntityWithId
        {
            builder.Entity<TEntity>().HasKey(x => x.Id);
            builder.Entity<TEntity>()
                .Property(x => x.Id)
                .UseSerialColumn();
            builder.Entity<TEntity>().ToTable(typeof(TEntity).Name);
        }
    }
}
