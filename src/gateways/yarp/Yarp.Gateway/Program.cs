using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;
using Swashbuckle.AspNetCore.SwaggerGen;
using TeckShop.Infrastructure;
using TeckShop.Infrastructure.Auth;
using Yarp.Configs;
using Yarp.ReverseProxy.Swagger;
using Yarp.ReverseProxy.Swagger.Extensions;

var builder = WebApplication.CreateBuilder(args);
bool enableSwagger = false;
bool enableFastEndpoints = false;
bool enableCaching = false;

// Add keycloak Authentication.
builder.Services.AddKeycloak(builder.Configuration, builder.Environment);

builder.AddInfrastructure(swaggerDocumentOptions: [], enableSwagger: enableSwagger, enableFastEndpoints: enableFastEndpoints, addCaching: enableCaching);

// REVERSE PROXY
var configuration = builder.Configuration.GetSection("ReverseProxy");
builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration)
    .AddSwagger(configuration);

// TRACES AND LOGS OF THE GATEWAY
builder.Services.AddOpenTelemetry()
    .WithMetrics(builder => builder.AddPrometheusExporter())
    .WithTracing(yarp =>
    {
        // Listen to the YARP tracing activities
        yarp.AddSource("Yarp.ReverseProxy");
    });

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

// APP
var app = builder.Build();

app.UseInfrastructure(builder.Environment, enableSwagger: enableSwagger, enableFastEndpoints: enableFastEndpoints);

// MIDDLEWARES
// NOTE: optional middlerwares that might be used in the gateway.
// Omit these lines if not needed.
// app.UseMiddleware<APIKeyMiddleware>();
// app.UseMiddleware<CustomAuthenticationMiddleware>();
// app.UseMiddleware<MembershipAndThrottlingMiddleware>();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

// REVERSE PROXY
app.MapReverseProxy();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseAuthentication();
app.UseAuthorization();

// SWAGGER
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "docs";
    var config = app.Services.GetRequiredService<IOptionsMonitor<ReverseProxyDocumentFilterConfig>>().CurrentValue;
    foreach (var cluster in config.Clusters)
    {
        options.SwaggerEndpoint($"/swagger/{cluster.Key}/swagger.json", cluster.Key);
    }
});

await app.RunAsync();