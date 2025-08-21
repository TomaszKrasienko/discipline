using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using discipline.centre.shared.abstractions.SharedKernel.TypeIdentifiers;
using discipline.centre.users.application.Accounts.Services;
using discipline.centre.users.domain.Accounts.Enums;
using discipline.centre.users.infrastructure.Auth.Claims;
using discipline.centre.users.infrastructure.Auth.Configuration.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace discipline.centre.users.infrastructure.Auth;

internal sealed class JwtAuthenticator(
    TimeProvider timeProvider,
    IOptions<JwtOptions> options) : IAuthenticator
{
    private readonly KeyPublishingOptions _keyPublishingOptions = options.Value.KeyPublishing;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

    public string CreateToken(
        AccountId accountId,
        bool hasPayedSubscription,
        DateOnly? activeTill,
        int? numberOfDailyTasks,
        int? numberOfRules)
    {
        var privateKey = GetPrivateKey();
        var signingCredentials = new SigningCredentials(
            privateKey, 
            SecurityAlgorithms.RsaSha256);

        var status = numberOfDailyTasks is null || numberOfRules is null 
            ? AccountState.New
            : AccountState.Active;
        
        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.UniqueName, accountId.Value.ToString()),
            new(CustomClaimTypes.Status, status.Value),
            new (CustomClaimTypes.IsPayedSubscription, hasPayedSubscription.ToString())
        ];

        if (activeTill is not null)
        {
            claims.Add(new Claim(CustomClaimTypes.ActiveTill, activeTill.Value.ToString()));
        }
        
        if (numberOfDailyTasks is not null)
        {
            claims.Add(new Claim(
                CustomClaimTypes.NumberOfDailyTasks,
                numberOfDailyTasks.Value.ToString()));
        }

        if (numberOfRules is not null)
        {
            claims.Add(new Claim(
                CustomClaimTypes.NumberOfRules,
                numberOfRules.Value.ToString()));
        }
        
        var now = timeProvider.GetUtcNow().ToLocalTime();
        var expirationTime = now.Add(_keyPublishingOptions.TokenExpiry);

        var jwt = new JwtSecurityToken(
            issuer: _keyPublishingOptions.Issuer,
            audience: _keyPublishingOptions.Audience,
            claims: claims,
            notBefore: now.DateTime,
            expires: expirationTime.DateTime,
            signingCredentials: signingCredentials);

        return _jwtSecurityTokenHandler.WriteToken(jwt);
    }

    private RsaSecurityKey GetPrivateKey()
    {
        RSA privateRsa = RSA.Create();
        var keyText = File.ReadAllText(_keyPublishingOptions.PrivateCertPath);
        privateRsa.ImportFromEncryptedPem(input: keyText, password: _keyPublishingOptions.PrivateCertPassword);
        return new RsaSecurityKey(privateRsa);
    }
}