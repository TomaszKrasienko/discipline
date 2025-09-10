using discipline.hangfire.domain.ActivityRules;
using discipline.hangfire.shared.abstractions.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.hangfire.infrastructure.DAL.EntityTypesConfigurations.ViewModels;

internal sealed class ActivityRuleViewModelTypeConfiguration : IEntityTypeConfiguration<ActivityRuleViewModel>
{
    public void Configure(EntityTypeBuilder<ActivityRuleViewModel> builder)
    {
        builder.ToView("ActivityRulesView");
        builder.HasNoKey();
    }
}