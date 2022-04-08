using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Core.Domains.BankAccounts.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Minibank.Data.HistoryTransfers
{
    [Table("history_transfer")]
    public class HistoryTransferDbModel
    {
        public string Id { get; set; }
        public float Amount { get; set; }
        public Currency Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }

    internal class Map : IEntityTypeConfiguration<HistoryTransferDbModel>
    {
        public void Configure(EntityTypeBuilder<HistoryTransferDbModel> builder)
        {

        }
    }
}
