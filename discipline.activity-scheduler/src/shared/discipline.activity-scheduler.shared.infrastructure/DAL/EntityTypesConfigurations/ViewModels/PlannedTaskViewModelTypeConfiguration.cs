using discipline.activity_scheduler.shared.abstractions.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.activity_scheduler.shared.infrastructure.DAL.EntityTypesConfigurations.ViewModels;

internal sealed class PlannedTaskViewModelTypeConfiguration : IEntityTypeConfiguration<PlannedTaskViewModel>
{
    public void Configure(EntityTypeBuilder<PlannedTaskViewModel> builder)
    {
        builder.ToView("PlannedTaskView");
        builder.HasNoKey();
    }
}