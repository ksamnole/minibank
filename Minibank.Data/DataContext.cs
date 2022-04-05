using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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

        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class Factory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder()
                .UseNpgsql("Host=localhost;Port=5432;Database=Minibank;Username=postgres;Password=1122334455")
                .Options;

            return new DataContext(options);
        }
    }
}
