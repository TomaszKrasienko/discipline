using System.Security.Cryptography;
using discipline.libs.auth.Configuration.Options;
using discipline.libs.configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace discipline.libs.auth.Configuration;

public static class AuthServicesConfigurationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddConfiguration(configuration)
            .AddTokenValidation();

    private static IServiceCollection AddConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
        => services.ValidateAndBind<AuthOptions, AuthOptionsValidator>(configuration);
    
    private static IServiceCollection AddTokenValidation(this IServiceCollection services)
    {
        var authOptions = services
            .GetOptions<AuthOptions>();
        
        var signingKey = GetRsaSecurityKey(authOptions.PublicCertificatePath);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            ValidIssuer = authOptions.Issuer,
            ValidAudience = authOptions.Audience,
            LogValidationExceptions = true,
            IssuerSigningKey = signingKey
        };

        services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = validationParameters;
            });

        return services;
    }
    
    private static RsaSecurityKey GetRsaSecurityKey(string path)
    {
        RSA publicInternalRsa = RSA.Create();
        var keyText = File.ReadAllText(path);
        publicInternalRsa.ImportFromPem(keyText);
        return new RsaSecurityKey(publicInternalRsa);
    }
}