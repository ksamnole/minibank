using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Minibank.Data.BankAccounts.Repositories;
using Minibank.Data.HistoryTransfers;
using Minibank.Data.Users;

namespace Minibank.Data
{
    public class DataContext : DbContext
    {
        public DbSet<UserDbModel> Users { get; set; }
        public DbSet<HistoryTransferDbModel> Transfers { get; set; }
        public DbSet<BankAccountDbModel> BankAccounts{ get; set; }

        public DataContext(DbContextOptions options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSnakeCaseNamingConvention();
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class Factory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder()
                .UseNpgsql("FakeConnectionStringOnlyForMigrations")
                .Options;

            return new DataContext(options);
        }
    }
}
