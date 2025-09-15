using System.Net;
using System.Net.Http.Json;
using discipline.centre.integrationtests.sharedkernel;
using discipline.centre.users.application.Accounts.DTOs.Requests;
using discipline.centre.users.domain.Subscriptions.Enums;
using discipline.centre.users.infrastructure.DAL.Accounts.Documents;
using discipline.centre.users.infrastructure.DAL.Subscriptions.Documents;
using discipline.centre.users.infrastructure.DAL.Users.Documents;
using discipline.centre.users.tests.shared_kernel.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Shouldly;
using Xunit;

namespace discipline.centre.users.integrationTests.Accounts;

[Collection("users-module")]
public sealed class SignUpTests() : BaseTestsController("users-module")
{
    [Fact]
    public async Task GivenSignUpRequest_WhenCallTo_api_accounts_ThenReturns204StatusCode_CreateAccountAndUser_SendsEvent()
    {
        // Arrange
        var subscription = await TestAppDb
            .GetCollection<SubscriptionDocument>()
            .Find(x => x.Type == SubscriptionType.Standard.Value)
            .FirstOrDefaultAsync(CancellationToken.None);

        var command = new SignUpRequestDto(
            "joe.doe@discipline.pl",
            "Test123!",
            subscription.Id,
            Period.Month.Value,
            "Joe",
            "Doe",
            null);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/accounts", command);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var doesAccountExist = await TestAppDb
            .GetCollection<AccountDocument>()
            .Find(x => x.Login == command.Email)
            .AnyAsync();
        
        doesAccountExist.ShouldBeTrue();
        
        var doesUserExist = await TestAppDb
            .GetCollection<UserDocument>()
            .Find(x => x.Email == command.Email)
            .AnyAsync();
        
        doesUserExist.ShouldBeTrue();
    }
    
    [Fact]
    public async Task GivenSignUpRequestWithExistingEmail_WhenCallTo_api_accounts_ThenReturns400StatusCode()
    {
        // Arrange
        var accountDocument = AccountDocumentFakeDataFactory
            .Get()
            .WithSubscriptionOrder();
        
        await TestAppDb
            .GetCollection<AccountDocument>()
            .InsertOneAsync(accountDocument);
        
        var subscription = await TestAppDb
            .GetCollection<SubscriptionDocument>()
            .Find(x => x.Type == SubscriptionType.Standard.Value)
            .FirstOrDefaultAsync(CancellationToken.None);
        
        var command = new SignUpRequestDto(
            accountDocument.Login,
            "Test123!",
            subscription.Id,
            Period.Month.Value,
            "Joe",
            "Doe",
            null);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/accounts", command);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GivenInvalidSignUpRequest_WhenCallTo_api_accounts_ThenReturns422StatusCodeWithErrorCode()
    {
        // Arrange
        var accountDocument = AccountDocumentFakeDataFactory
            .Get()
            .WithSubscriptionOrder();
        
        await TestAppDb
            .GetCollection<AccountDocument>()
            .InsertOneAsync(accountDocument);
        
        var subscription = await TestAppDb
            .GetCollection<SubscriptionDocument>()
            .Find(x => x.Type == SubscriptionType.Standard.Value)
            .FirstOrDefaultAsync(CancellationToken.None);
        
        var command = new SignUpRequestDto(
            accountDocument.Login,
            string.Empty,
            subscription.Id,
            Period.Month.Value,
            "Joe",
            "Doe",
            null);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/accounts", command);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails!.Title.ShouldBe("Validation.Password.Empty");
    }
}