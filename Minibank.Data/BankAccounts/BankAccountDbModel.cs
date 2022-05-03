using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Core.Domains.BankAccounts.Enums;
using Minibank.Data.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Minibank.Data.BankAccounts.Repositories
{
    [Table("bank_account")]
    public class BankAccountDbModel
    {
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public virtual UserDbModel User { get; set; }
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
    }

    internal class Map : IEntityTypeConfiguration<BankAccountDbModel>
    {
        public void Configure(EntityTypeBuilder<BankAccountDbModel> builder)
        {
            builder.HasOne(it => it.User)
                .WithMany(it => it.BankAccounts)
                .HasForeignKey(it => it.UserId);
        }
    }
}
