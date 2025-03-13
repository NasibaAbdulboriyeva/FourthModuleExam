using TelegramBot.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatBot.Dal.EntityConfigurations;

public class BotUserConfiguration : IEntityTypeConfiguration<BotUser>
{
    public void Configure(EntityTypeBuilder<BotUser> builder)
    {
        builder.ToTable("BotUser");
        builder.HasKey(u => u.BotUserId);

        builder.Property(u => u.FirstName).IsRequired(true);
        builder.Property(u => u.LastName).IsRequired(true);
        builder.Property(u => u.PhoneNumber).IsRequired(true);
        builder.Property(u => u.Email).IsRequired(true);
        builder.Property(u => u.BirthDate).IsRequired(true);
        builder.Property(u => u.Address).IsRequired(true);

        builder.HasIndex(u => u.ChatId).IsUnique(true);

    }
}
