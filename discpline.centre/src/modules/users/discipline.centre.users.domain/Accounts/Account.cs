using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts.Specifications.Account;
using discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;
using discipline.centre.users.domain.Accounts.ValueObjects.Account;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;

namespace discipline.centre.users.domain.Accounts;

public sealed class Account : AggregateRoot<AccountId, Ulid>
{
    private readonly HashSet<SubscriptionOrder> _orders = new();
    public Login Login { get; private set; }
    public Password Password { get; private set; }
    public IReadOnlyCollection<SubscriptionOrder> Orders => _orders.ToArray();
    
    private Account(
        AccountId accountId,
        Login login,
        Password password) : base(accountId) 
    {
        Login = login;
        Password = password;
    }

    internal static Account Create(
        AccountId accountId,
        string login,
        PasswordSpecification passwordSpecification,
        TimeProvider timeProvider,
        SubscriptionOrderSpecification order)
    {
        var password = Password.Create(
            passwordSpecification.Password,
            passwordSpecification.HashedPassword);
        
        var account = new Account(
            accountId,
            login,
            password);
        
        account.AddOrder(SubscriptionOrderId.New(), timeProvider, order);
        return account;
    }

    private SubscriptionOrder AddOrder(
        SubscriptionOrderId subscriptionOrderId,
        TimeProvider timeProvider,
        SubscriptionOrderSpecification order)
    {
        var startDay = DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime);

        DateOnly? finishDate = null;

        if (order.ValidityPeriod is not null)
        {
            finishDate = startDay.AddDays(order.ValidityPeriod.Value);
        }
        
        var interval = Interval.Create(startDay, finishDate);
        var subscription = SubscriptionDetails.Create(
            order.SubscriptionType,
            order.ValidityPeriod,
            order.RequirePayment);
        
        Payment? payment = null;
        
        if (order.PaymentValue is not null)
        {
            payment = Payment.Create(
                timeProvider,
                order.PaymentValue.Value);
        }

        var subscriptionOrder = SubscriptionOrder.Create(
            subscriptionOrderId,
            interval, 
            subscription,
            payment);
        
        _orders.Add(subscriptionOrder);
        return subscriptionOrder;
    }
}