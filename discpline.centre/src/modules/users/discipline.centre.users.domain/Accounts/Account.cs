using discipline.centre.shared.abstractions.SharedKernel.Aggregate;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.domain.Accounts.Specifications.Account;
using discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;
using discipline.centre.users.domain.Accounts.ValueObjects.Account;
using discipline.centre.users.domain.Accounts.ValueObjects.SubscriptionOrder;
using discipline.centre.users.domain.Subscriptions.Enums;

namespace discipline.centre.users.domain.Accounts;

public sealed class Account : AggregateRoot<AccountId, Ulid>
{
    private readonly HashSet<SubscriptionOrder> _orders = new();
    public Login Login { get; private set; }
    public Password Password { get; private set; }
    public IReadOnlyCollection<SubscriptionOrder> Orders 
        => _orders.ToArray();
    
    //TODO: Unit tests
    public SubscriptionOrder? ActiveSubscriptionOrder 
        => Orders.SingleOrDefault(x => x.Interval.FinishDate is null);
    
    private Account(
        AccountId accountId,
        Login login,
        Password password) : base(accountId) 
    {
        Login = login;
        Password = password;
    }

    /// <summary>
    /// Use only for MongoDB
    /// </summary>
    public Account(
        AccountId accountId,
        Login login,
        Password password,
        HashSet<SubscriptionOrder> orders) : this(
            accountId,
            login,
            password)
    {
        _orders = orders;
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
        
        account.AddOrder(
            SubscriptionOrderId.New(),
            timeProvider,
            order);
        return account;
    }

    internal SubscriptionOrder AddOrder(
        SubscriptionOrderId subscriptionOrderId,
        TimeProvider timeProvider,
        SubscriptionOrderSpecification order)
    {
        var startDay = DateOnly.FromDateTime(timeProvider.GetUtcNow().DateTime);

        DateOnly? finishDate = null;

        if (order.Period is not null)
        {
            finishDate = order.Period == Period.Month 
                ? startDay.AddMonths(1)
                : startDay.AddYears(1);
        }
        
        var interval = Interval.Create(startDay, finishDate);
        var subscription = SubscriptionDetails.Create(
            order.SubscriptionType,
            order.Period,
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
            payment,
            order.SubscriptionId);
        
        _orders.Add(subscriptionOrder);
        return subscriptionOrder;
    }
}