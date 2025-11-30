using PumpMaster.Api.Services;
using PumpMaster.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<CosmosDbService>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("CosmosDb");
    return new CosmosDbService(connectionString, "PumpMaster", "telemetry");
});

builder.Services.AddScoped<TelemetryBroadcastService>();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
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
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();
app.MapHub<TelemetryHub>("/telemetryHub");

app.Run();
