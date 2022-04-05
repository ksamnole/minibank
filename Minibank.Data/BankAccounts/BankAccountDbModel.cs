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
        public float Amount { get; set; }
        public Currency Currency { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
    }

    internal class Map : IEntityTypeConfiguration<BankAccountDbModel>
    {
        public void Configure(EntityTypeBuilder<BankAccountDbModel> builder)
        {
            builder.Property(it => it.Id)
                .HasColumnName("id");

            builder.Property(it => it.IsActive)
                .HasColumnName("is_active");

            builder.Property(it => it.UserId)
                .HasColumnName("user_id");

            builder.HasOne(it => it.User)
                .WithMany(it => it.BankAccounts)
                .HasForeignKey(it => it.UserId);

            builder.Property(it => it.Amount)
                .HasColumnName("amount");

            builder.Property(it => it.Currency)
                .HasColumnName("currency");

            builder.Property(it => it.OpenDate)
                .HasColumnName("open_date");

            builder.Property(it => it.CloseDate)
                .HasColumnName("close_date");
        }
    }
}
