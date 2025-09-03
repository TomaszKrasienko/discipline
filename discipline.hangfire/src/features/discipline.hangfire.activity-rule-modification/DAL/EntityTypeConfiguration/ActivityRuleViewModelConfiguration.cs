using discipline.hangfire.shared.abstractions.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.hangfire.activity_rules.DAL.EntityTypeConfiguration;

internal sealed class ActivityRuleViewModelConfiguration : IEntityTypeConfiguration<ActivityRuleViewModel>
{
    public void Configure(EntityTypeBuilder<ActivityRuleViewModel> builder)
    {
        builder.ToView("ActivityRulesView", "activity-rules");

        builder.HasNoKey();
    }
}