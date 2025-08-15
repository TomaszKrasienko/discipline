using System.Net;
using System.Net.Http.Json;
using discipline.centre.integrationTests.sharedKernel;
using discipline.centre.users.application.Accounts.Commands;
using discipline.centre.users.application.Users.DTOs;
using discipline.centre.users.infrastructure.DAL.Accounts.Documents;
using discipline.centre.users.tests.sharedkernel.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace discipline.centre.users.integrationTests.Accounts;

[Collection("users-module")]
public sealed class SignInTests() : BaseTestsController("users-module")
{
    [Fact]
    public async Task GivenSignInRequest_WhenCallTo_api_accounts_signed_in_ThenReturns200StatusCodeWithTokenAndRefreshToken_StoreRefreshToken()
    {
        // Arrange
        var accountDocument = AccountDocumentFakeDataFactory
            .Get()
            .WithSubscriptionOrder();

        var password = AccountDocumentFakeDataFactory.GetPassword();
        
        await TestAppDb
            .GetCollection<AccountDocument>()
            .InsertOneAsync(accountDocument);
        
        
        var command = new SignInCommand(accountDocument.Login, password!);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/accounts/signed-in", command);
        
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
     public async Task GivenSignInRequestWithNotExistingAccount_WhenCallTo_api_accounts_signed_in_ThenReturns400StatusCode()
     {
         // Arrange
         var command = new SignInCommand("test@test.pl", "Test123!");
         
         // Act
         var response = await HttpClient.PostAsJsonAsync("api/accounts/signed-in", command);
         
         // Assert
         response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
         
         var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
         result!.Title.ShouldBe("SignIn.Unauthorized");
     }

     [Fact]
     public async Task GivenInvalidSignInRequest_WhenCallTo_api_accounts_signed_in_ThenReturns422StatusCodeWithErrorCode()
     {
         
         // Arrange
         var command = new SignInCommand(string.Empty, "Test123!");
         
         // Act
         var response = await HttpClient.PostAsJsonAsync("api/accounts/signed-in", command);
         
         // Assert
         response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
         
         var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
         problemDetails!.Title.ShouldBe("Validation.Email.Empty");
     }
     
     #region Arrange

     private TestRedisCache _testRedisCache = null!;
     
     protected override void ConfigureServices(IServiceCollection services)
     {
         _testRedisCache = new TestRedisCache();
         
         services.AddStackExchangeRedisCache(redistOptions =>
         {
             redistOptions.Configuration = _testRedisCache.ConnectionString;
         });
         
         base.ConfigureServices(services);
     }
     
     #endregion
}