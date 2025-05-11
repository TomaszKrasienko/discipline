using discipline.hangfire.browse_planned.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.hangfire.browse_planned.DAL.EntityTypeConfigurations;

internal sealed class PlannedTaskViewModelTypeConfiguration : IEntityTypeConfiguration<PlannedTaskViewModel>
{
    public void Configure(EntityTypeBuilder<PlannedTaskViewModel> builder)
    {
        builder.ToView("PlannedTasksView", "browse-planned");
        
        builder.HasNoKey();
    }
}