using PumpMaster.Api.Services;
using PumpMaster.Api.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Azure.Cosmos;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<CosmosClient>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("CosmosDb");
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
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            return new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) };
        }
    };
    return new CosmosClient(connectionString, options);
});

builder.Services.AddSingleton<CosmosDbService>(provider =>
{
    var cosmosClient = provider.GetRequiredService<CosmosClient>();
    var logger = provider.GetRequiredService<ILogger<CosmosDbService>>();
    return new CosmosDbService(cosmosClient, "PumpMaster", "telemetry", logger);
});

builder.Services.AddScoped<TelemetryBroadcastService>();
builder.Services.AddSignalR();
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<TelemetryHub>("/telemetryHub");

app.Run();
