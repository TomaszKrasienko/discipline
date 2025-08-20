using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using discipline.centre.integrationTests.sharedKernel.InternalAuthentication.TestsOptions;
using discipline.centre.shared.abstractions.Clock;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace discipline.centre.integrationTests.sharedKernel.InternalAuthentication;

internal sealed class InternalJwtAuthenticator(
    DateTimeOffset now,
    IOptions<InternalKeyOptions> options,
    DateTimeOffset? expires = null)
{
    private readonly InternalKeyOptions _options = options.Value;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    
    internal string CreateToken()
    {
        RSA privateRsa = RSA.Create();
        var keyText = File.ReadAllText(_options.PrivateCertPath);
        privateRsa.ImportFromEncryptedPem(input: keyText, password: _options.PrivateCertPassword);
        var privateKey = new RsaSecurityKey(privateRsa);
        var signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);

        var expirationTime = expires ?? now.AddMinutes(1);

        var jwt = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            notBefore: now.DateTime,
            expires: expirationTime.DateTime,
            signingCredentials: signingCredentials);
        
        return _jwtSecurityTokenHandler.WriteToken(jwt);
    }
}