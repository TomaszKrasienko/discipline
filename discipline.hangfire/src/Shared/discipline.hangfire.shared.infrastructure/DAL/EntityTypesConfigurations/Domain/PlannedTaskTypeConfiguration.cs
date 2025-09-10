using discipline.hangfire.domain.PlannedTasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.hangfire.infrastructure.DAL.EntityTypesConfigurations.Domain;

internal sealed class PlannedTaskTypeConfiguration : IEntityTypeConfiguration<PlannedTask>
{
    public void Configure(EntityTypeBuilder<PlannedTask> builder)
    {
        builder.ToTable("PlannedTasks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ActivityRuleId)
            .HasColumnName("ActivityRuleId")
            .HasMaxLength(26)
            .IsRequired();
        
        builder.Property(x => x.AccountId)
            .HasColumnName("AccountId")
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired();
        
        builder.Property(x => x.PlannedEnabled)
            .HasColumnName("PlannedEnable")
            .IsRequired();
        
        builder.Property(x => x.ActivityCreated)
            .HasColumnName("ActivityCreated")
            .IsRequired();
        
        builder.Property(x => x.PlannedFor)
            .HasColumnName("PlannedFor");
    }
}