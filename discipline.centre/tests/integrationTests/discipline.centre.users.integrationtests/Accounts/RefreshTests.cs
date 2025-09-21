using System.Net;
using System.Net.Http.Json;
using discipline.centre.integrationtests.sharedkernel;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Accounts.Commands;
using discipline.centre.users.application.Accounts.DTOs;
using discipline.centre.users.application.Accounts.DTOs.Requests;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.infrastructure.DAL.Accounts.Documents;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;
using discipline.centre.users.infrastructure.RefreshToken.Configuration;
using discipline.centre.users.tests.shared_kernel.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.users.integrationTests.Accounts;

[Collection("users-module")]
public sealed class RefreshTests() : BaseTestsController("users-module")
{
    [Fact]
    public async Task GivenRefreshRequest_WhenCallTo_api_accounts_refreshed_ThenReturns200StatusCodeWithTokenAndRefreshToken_StoreRefreshToken()
    {
        // Arrange
        var accountDocument = AccountDocumentFakeDataFactory
            .Get()
            .WithSubscriptionOrder();

        var subscription = await TestAppDb
            .GetCollection<SubscriptionDocument>()
            .Find(x => x.Type == SubscriptionType.Standard.Value)
            .SingleAsync();
        
        // preparing subscription order with proper identifier
        var subscriptionOrder = accountDocument.SubscriptionOrders.Single();
        subscriptionOrder.SubscriptionId = subscription.Id.ToString();
        accountDocument.SubscriptionOrders =
        [
            subscriptionOrder,
        ];
        
        var password = AccountDocumentFakeDataFactory.GetPassword();
        
        await TestAppDb
            .GetCollection<AccountDocument>()
            .InsertOneAsync(accountDocument);
        
        // signing in and retrieving token
        var command = new SignInCommand(accountDocument.Login, password!);
        var signInResponse = await HttpClient.PostAsJsonAsync("api/accounts/signed-in", command);
        var signInResult = await signInResponse.Content.ReadFromJsonAsync<TokensDto>();

        var request = new RefreshRequestDto(signInResult!.RefreshToken, accountDocument.Id);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/accounts/refreshed", request);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<TokensDto>();
        result.ShouldNotBeNull();
        result.Token.ShouldNotBeEmpty();
        result.RefreshToken.ShouldNotBeEmpty();

        var refreshToken = _testRedisCache
            .GetValueAsync<RefreshTokenDto>(accountDocument.Id);

        refreshToken.ShouldNotBeNull();
        refreshToken.Value.ShouldBe(result.RefreshToken);
    }
    
        [Fact]
    public async Task GivenRefreshRequest_WhenCallTo_api_accounts_refreshed_AfterExpiryTime_ThenReturns400StatusCode()
    {
        // Arrange
        var accountDocument = AccountDocumentFakeDataFactory
            .Get()
            .WithSubscriptionOrder();

        var subscription = await TestAppDb
            .GetCollection<SubscriptionDocument>()
            .Find(x => x.Type == SubscriptionType.Standard.Value)
            .SingleAsync();
        
        // preparing subscription order with proper identifier
        var subscriptionOrder = accountDocument.SubscriptionOrders.Single();
        subscriptionOrder.SubscriptionId = subscription.Id.ToString();
        accountDocument.SubscriptionOrders =
        [
            subscriptionOrder,
        ];
        
        var password = AccountDocumentFakeDataFactory.GetPassword();
        
        await TestAppDb
            .GetCollection<AccountDocument>()
            .InsertOneAsync(accountDocument);
        
        // signing in and retrieving token
        var command = new SignInCommand(accountDocument.Login, password!);
        var signInResponse = await HttpClient.PostAsJsonAsync("api/accounts/signed-in", command);
        var signInResult = await signInResponse.Content.ReadFromJsonAsync<TokensDto>();

        var request = new RefreshRequestDto(signInResult!.RefreshToken, accountDocument.Id);
        
        await Task.Delay(TimeSpan.FromSeconds(_expiryInSeconds + 1));
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/accounts/refreshed", request);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        result!.Title.ShouldBe("Refresh.Unauthorized");
    }

    [Fact]
    public async Task GivenInvalidRefreshRequest_WhenCallTo_api_accounts_refreshed_ThenReturns422WithErrorCode()
    {
        // Arrange
        var request = new RefreshRequestDto(string.Empty, AccountId.New().ToString());
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/accounts/refreshed", request);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails!.Title.ShouldBe("Validation.RefreshToken.Empty");
    }
    
    #region Arrange

    private TestRedisCache _testRedisCache = null!;
    private readonly int _expiryInSeconds = 5;
    
    protected override void ConfigureServices(IServiceCollection services)
    {
        _testRedisCache = new TestRedisCache();
         
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            redisOptions.Configuration = _testRedisCache.ConnectionString;
        });

        var option = Options.Create(new RefreshTokenOptions()
        {
            Expiry = TimeSpan.FromSeconds(_expiryInSeconds),
            Length = 10
        });
        
        services.AddSingleton<IOptions<RefreshTokenOptions>>(option);
         
        base.ConfigureServices(services);
    }
    
    #endregion
}