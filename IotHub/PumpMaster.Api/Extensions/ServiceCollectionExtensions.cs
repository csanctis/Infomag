using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using PumpMaster.Api.Services;
using System.Text;

namespace PumpMaster.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<CosmosClient>(provider =>
            {
                var connectionString = configuration.GetConnectionString("CosmosDb");
                var options = new CosmosClientOptions
                {
                    MaxRetryAttemptsOnRateLimitedRequests = 5,
                    MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromSeconds(60),
                    ConnectionMode = ConnectionMode.Gateway,
                    RequestTimeout = TimeSpan.FromSeconds(30),
                    HttpClientFactory = () =>
                    {
                        var handler = new HttpClientHandler()
                        {
                            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                            UseProxy = false
                        };
                        return new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) };
                    }
                };
                return new CosmosClient(connectionString, options);
            });

            services.AddSingleton<ICosmosDbService>(provider =>
            {
                var cosmosClient = provider.GetRequiredService<CosmosClient>();
                var logger = provider.GetRequiredService<ILogger<CosmosDbService>>();
                var databaseName = configuration["CosmosDb:DatabaseName"];
                var containerName = configuration["CosmosDb:ContainerName"];
                return new CosmosDbService(cosmosClient, databaseName, containerName, logger);
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/telemetryHub"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITelemetryBroadcastService, TelemetryBroadcastService>();
            services.AddSignalR();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            return services;
        }
    }
}