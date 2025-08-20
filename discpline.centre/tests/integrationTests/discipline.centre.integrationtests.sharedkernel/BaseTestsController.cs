using System.Net.Http.Headers;
using discipline.centre.integrationTests.sharedKernel.InternalAuthentication;
using discipline.centre.integrationTests.sharedKernel.InternalAuthentication.TestsOptions;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.shared.infrastructure.Clock;
using discipline.centre.shared.infrastructure.DAL.Collections.Abstractions;
using discipline.centre.shared.infrastructure.ResourceHeader;
using discipline.centre.users.domain.Accounts;
using discipline.centre.users.domain.Accounts.Services;
using discipline.centre.users.domain.Accounts.Specifications.SubscriptionOrder;
using discipline.centre.users.domain.Subscriptions;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.domain.Subscriptions.Policies;
using discipline.centre.users.infrastructure.Auth;
using discipline.centre.users.infrastructure.Auth.Configuration.Options;
using discipline.centre.users.infrastructure.DAL.Accounts.Documents;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;
using discipline.centre.users.infrastructure.Passwords;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace discipline.centre.integrationTests.sharedKernel;

public abstract class BaseTestsController : IDisposable
{
    public TestAppDb TestAppDb { get; set; }
    protected HttpClient HttpClient { get; set; }

    protected BaseTestsController(string moduleName)
    {
        TestAppDb = new TestAppDb(moduleName);
        var app = new TestApp(ConfigureServices);
        HttpClient = app.HttpClient;
    }
    
    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IMongoCollectionNameConvention, TestsMongoCollectionNameConvention>();
        services.AddSingleton<IMongoClient>(sp => TestAppDb.GetMongoClient());
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
   
    protected virtual void Dispose(bool disposed)
    { 
        TestAppDb?.Dispose();
        HttpClient?.Dispose();
    }
    
    protected async Task<Account> AuthorizeWithFreeSubscriptionPicked()
    {
        var subscriptionDocument = await TestAppDb
            .GetCollection<SubscriptionDocument>("users-module")
            .Find(x => x.Type == SubscriptionType.Standard.Value)
            .SingleAsync();

        var passwordHasher = new PasswordHasher<Account>();
        var passwordManager = new PasswordManager(passwordHasher);
        var timeProvider = TimeProvider.System;
        var accountService = new AccountService(
            passwordManager, 
            timeProvider);
        const string password = "password";

        var account = accountService.Create(
            AccountId.New(),
            "joe.doe@discipline.pl",
            password,
            new SubscriptionOrderSpecification(
                SubscriptionId.Parse(subscriptionDocument.Id),
                subscriptionDocument.Type,
                null,
                SubscriptionType.Standard.HasPayment,
                null));

        await TestAppDb
            .GetCollection<AccountDocument>("users-module")
            .InsertOneAsync(account.ToDocument());
        
        Authorize(
            account,
            subscriptionDocument.ToEntity(new StandardSubscriptionPolicy()),
            timeProvider,
            null);
        
        return account;
    }
    
    protected async Task<Account> AuthorizedWithExpiredToken()
    {
        var subscriptionDocument = await TestAppDb
            .GetCollection<SubscriptionDocument>("users-module")
            .Find(x => x.Type == SubscriptionType.Standard.Value)
            .SingleAsync();

        var passwordHasher = new PasswordHasher<Account>();
        var passwordManager = new PasswordManager(passwordHasher);
        var timeProvider = TimeProvider.System;
        var accountService = new AccountService(
            passwordManager, 
            timeProvider);
        const string password = "password";

        var account = accountService.Create(
            AccountId.New(),
            "joe.doe@discipline.pl",
            password,
            new SubscriptionOrderSpecification(
                SubscriptionId.Parse(subscriptionDocument.Id),
                subscriptionDocument.Type,
                null,
                SubscriptionType.Standard.HasPayment,
                null));

        await TestAppDb
            .GetCollection<AccountDocument>("users-module")
            .InsertOneAsync(account.ToDocument());

        var subscription = subscriptionDocument.ToEntity(new StandardSubscriptionPolicy()); 
        
        var optionsProvider = new OptionsProvider();
        var authOptions = optionsProvider.Get<JwtOptions>();
        var authenticator = new JwtAuthenticator(
            timeProvider, 
            Options.Create(authOptions));

        var now = timeProvider.GetUtcNow();
        
        var token = authenticator.CreateToken(
            account.Id,
            subscription.Type.HasExpiryDate,
            DateOnly.FromDateTime(now.AddDays(-1).DateTime),
            subscription.GetAllowedNumberOfDailyTasks(),
            subscription.GetAllowedNumberOfRules());
        
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "bearer",
            token);
        
        return account;
    }
    
    protected virtual void Authorize(
        Account account,
        Subscription subscription,
        TimeProvider timeProvider,
        DateOnly? activeTill)
    {
        var optionsProvider = new OptionsProvider();
        var authOptions = optionsProvider.Get<JwtOptions>();
        var authenticator = new JwtAuthenticator(
            timeProvider, 
            Options.Create(authOptions));
        var token = authenticator.CreateToken(
            account.Id,
            subscription.Type.HasExpiryDate,
            activeTill,
            subscription.GetAllowedNumberOfDailyTasks(),
            subscription.GetAllowedNumberOfRules());
        
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "bearer",
            token);
    }

    protected virtual void Authorize()
    {
        var optionsProvider = new OptionsProvider();
        var internalAuthOptions = optionsProvider.Get<InternalKeyOptions>();
        var now = TimeProvider.System.GetUtcNow();
        var authenticator = new InternalJwtAuthenticator(now, Options.Create(internalAuthOptions));
        var token = authenticator.CreateToken();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
    }
    
    protected virtual string? GetResourceIdFromHeader(HttpResponseMessage httpResponseMessage) 
    {
        if (httpResponseMessage is null)
        {
            throw new InvalidOperationException("Http response message is null");
        }

        if (!httpResponseMessage.Headers.TryGetValues(ResourceHeaderExtension.HeaderName, out var value))
        {
            return null;
        }

        return value.Single();
    }
}