using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Minibank.Data.BankAccounts.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Minibank.Data.Users
{
    [Table("user")]
    public class UserDbModel
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        public virtual List<BankAccountDbModel> BankAccounts { get; set; }
    }

    internal class Map : IEntityTypeConfiguration<UserDbModel>
    {
        public void Configure(EntityTypeBuilder<UserDbModel> builder)
        {
            builder.Property(it => it.Id)
                .HasColumnName("id");

            builder.Property(it => it.Login)
                .HasColumnName("login");

            builder.Property(it => it.Email)
                .HasColumnName("email");
        }
    }
}
