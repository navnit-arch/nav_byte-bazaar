using AzureIntelligentSupplyChain.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAppLogging(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddCorsPolicy(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowedOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "MongoDb": {
    "ConnectionString": "mongodb+srv://user:password@cluster.mongodb.net/?retryWrites=true&w=majority",
    "DatabaseName": "AzureIntelligentSupplyChain",
    "InitializeOnStartup": true
  },
  "Authentication": {
    "Issuer": "AzureIntelligentSupplyChain",
    "Audience": "AzureIntelligentSupplyChainAPI",
    "SecretKey": "your-secret-key-change-in-production-min-32-chars",
    "TokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  },
  "ApplicationInsights": {
    "InstrumentationKey": ""
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:3000",
      "http://localhost:3001"
    ]
  },
  "AllowedHosts": "*"
}
