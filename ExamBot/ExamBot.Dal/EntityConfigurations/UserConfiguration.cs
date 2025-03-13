using ExamBot.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamBot.Dal.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.BotUserId);

        builder.Property(u => u.FirstName).HasMaxLength(20).IsRequired(true);
        builder.Property(u => u.LastName).HasMaxLength(20).IsRequired(true);

        builder.Property(u => u.PhoneNumber).HasMaxLength(20).IsRequired(true);


        builder.HasIndex(u => u.ChatId).IsUnique(true);

        builder.HasOne(u => u.FillData)
            .WithOne()
            .HasForeignKey<FillData>(ui => ui.BotUserId);
    }
}
