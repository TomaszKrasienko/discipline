using discipline.hangfire.activity_rules.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.hangfire.activity_rules.DAL.EntityTypeConfiguration;

internal sealed class ActivityRuleTypeConfiguration : IEntityTypeConfiguration<ActivityRule>
{
    public void Configure(EntityTypeBuilder<ActivityRule> builder)
    {
        builder.ToTable("ActivityRules");
        
        builder.HasKey(x => x.ActivityRuleId);

        builder.Property(x => x.ActivityRuleId)
            .HasColumnName("ActivityRuleId")
            .IsRequired()
            .HasMaxLength(26);
        
        builder.Property(x => x.AccountId)
            .HasColumnName("AccountId")
            .IsRequired()
            .HasMaxLength(26);

        builder.Property(x => x.Title)
            .HasColumnName("Title")
            .HasMaxLength(30);

        builder.Property(x => x.Mode)
            .HasColumnName("Mode")
            .HasMaxLength(20);

        builder.Property(x => x.SelectedDays)
            .HasColumnName("SelectedDays");
    }
}