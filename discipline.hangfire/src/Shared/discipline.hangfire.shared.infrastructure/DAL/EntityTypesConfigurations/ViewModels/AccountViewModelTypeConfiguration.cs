using discipline.hangfire.shared.abstractions.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace discipline.hangfire.infrastructure.DAL.EntityTypesConfigurations.ViewModels;

internal sealed class AccountViewModelTypeConfiguration : IEntityTypeConfiguration<AccountViewModel>
{
    public void Configure(EntityTypeBuilder<AccountViewModel> builder)
    {
        builder.ToView("AccountsView");

        builder.HasNoKey();
    }
}