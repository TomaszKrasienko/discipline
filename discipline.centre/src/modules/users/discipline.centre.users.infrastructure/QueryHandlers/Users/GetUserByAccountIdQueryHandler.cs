using discipline.centre.shared.abstractions.CQRS.Queries;
using discipline.centre.users.application.Users.DTOs.Responses;
using discipline.centre.users.application.Users.Queries;
using discipline.centre.users.infrastructure.DAL;
using discipline.centre.users.infrastructure.DAL.Accounts.Documents;
using discipline.centre.users.infrastructure.DAL.Users.Documents;
using MongoDB.Driver;

namespace discipline.centre.users.infrastructure.QueryHandlers.Users;

internal sealed class GetUserByAccountIdQueryHandler(
    UsersMongoContext context) : IQueryHandler<GetUserByAccountIdQuery, UserResponseDto?>, IQuery<UserResponseDto>
{
    public async Task<UserResponseDto?> HandleAsync(GetUserByAccountIdQuery query, CancellationToken cancellationToken = default)
    {
        var account = await context
            .GetCollection<AccountDocument>()
            .Find(x => x.Id == query.AccountId.ToString())
            .SingleOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            return null;
        }
        
        var user = await context
            .GetCollection<UserDocument>()
            .Find(x => x.AccountId == account.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return null;
        }

        var activeSubscriptionOrder = account
            .SubscriptionOrders
            .Single(x => x.Interval.FinishDate is null);

        return new UserResponseDto(
            user.Id,
            user.AccountId,
            user.Email,
            user.FirstName,
            user.LastName,
            new SubscriptionOrderResponseDto(
                new SubscriptionOrderIntervalResponseDto(
                    activeSubscriptionOrder.Interval.StartDate,
                    activeSubscriptionOrder.Interval.PlanedFinishDate,
                    activeSubscriptionOrder.Interval.FinishDate),
                new SubscriptionOrderSubscriptionDetailsResponseDto(
                    activeSubscriptionOrder.SubscriptionDetails.Type,
                    activeSubscriptionOrder.SubscriptionDetails.ValidityPeriod,
                    activeSubscriptionOrder.SubscriptionDetails.RequirePayment),
                activeSubscriptionOrder.Payment is null
                    ? null
                    : new SubscriptionOrderPaymentResponseDto(
                        activeSubscriptionOrder.Payment.CreatedAt,
                        activeSubscriptionOrder.Payment.Value),
                activeSubscriptionOrder.SubscriptionId));
    }
}