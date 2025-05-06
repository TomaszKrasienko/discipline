using discipline.hangfire.add_planned_tasks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.hangfire.add_planned_tasks.DAL.EntityTypeConfiguration;

internal sealed class PlannedTaskTypeConfiguration : IEntityTypeConfiguration<PlannedTask>
{
    public void Configure(EntityTypeBuilder<PlannedTask> builder)
    {
        builder.ToTable("PlannedTasks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<string>(x => x.ToString(), y => Ulid.Parse(y));

        builder.Property(x => x.ActivityRuleId)
            .HasColumnName("ActivityRuleId")
            .HasMaxLength(26)
            .IsRequired();
        
        builder.Property(x => x.UserId)
            .HasColumnName("UserId")
            .HasMaxLength(26)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired();
        
        builder.Property(x => x.IsPlannedEnable)
            .HasColumnName("IsPlannedEnable")
            .IsRequired();
        
        builder.Property(x => x.IsActivityCreated)
            .HasColumnName("IsActivityCreated")
            .IsRequired();
        
        builder.Property(x => x.PlannedFor)
            .HasColumnName("PlannedFor");
    }
}