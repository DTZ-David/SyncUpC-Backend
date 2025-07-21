using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SyncUpC.Domain.Ports.Configuration.JsonWebToken;
using SyncUpC.Infraestructure.Adapters.Configuration.JsonWebToken;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SyncUpC.Infraestructure.Extensions.JsonWebToken;


public static class JwtExtension
{
    private static readonly JsonSerializerOptions JsonSerializedOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static IServiceCollection AddJsonWebToken(this IServiceCollection services, IConfiguration configuration)
    {
        // Obtener valores desde appsettings.json
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
        var encryptKey = Convert.FromBase64String(jwtSettings["EncryptKey"]!);
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"]!);

        services
            .AddHttpContextAccessor()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    RequireSignedTokens = true,

                    ValidateIssuer = false, // ❌ No usamos Issuer
                    ValidateAudience = false, // ❌ No usamos Audience

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptKey),
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = async context =>
                    {
                        var modelResponse = new { StatusCode = 401, IsSuccess = false, Message = "Token inválido" };

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await JsonSerializer.SerializeAsync(context.Response.Body, modelResponse, JsonSerializedOptions);
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        if (!context.Response.HasStarted)
                        {
                            var modelResponse = new { StatusCode = 401, IsSuccess = false, Message = "Token no autorizado" };

                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            await JsonSerializer.SerializeAsync(context.Response.Body, modelResponse, JsonSerializedOptions);
                        }
                    },
                    OnForbidden = async context =>
                    {
                        var modelResponse = new { StatusCode = 403, IsSuccess = false, Message = "Sin permisos para acceder" };

                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        await JsonSerializer.SerializeAsync(context.Response.Body, modelResponse, JsonSerializedOptions);
                    }
                };

                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;
            });

        services.AddTransient<IJwtService, JwtService>();
        services.AddTransient<IRefreshTokenService, RefreshTokenService>();
        return services;
    }
}
