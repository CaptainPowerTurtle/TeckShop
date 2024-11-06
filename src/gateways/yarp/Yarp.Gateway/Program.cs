using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;
using Swashbuckle.AspNetCore.SwaggerGen;
using Yarp.ReverseProxy.Swagger;
using Yarp.ReverseProxy.Swagger.Extensions;
using Yarp.Configs;
using Yarp.Gateway.Extensions;
using TeckShop.Infrastructure;
using TeckShop.Infrastructure.Auth;

var builder = WebApplication.CreateBuilder(args);
bool enableSwagger = false;
bool enableFastEndpoints = false;
bool enableCaching = false;
bool isDevelopment = builder.Environment.IsDevelopment();

//if (string.IsNullOrEmpty(connectionString))
//    throw new ArgumentNullException(nameof(connectionString), "Application Insights connection string is not set.");

//builder.Logging.AddApplicationInsights(
//        configureTelemetryConfiguration: (config) =>
//            config.ConnectionString = connectionString,
//            configureApplicationInsightsLoggerOptions: (options) =>
//            {
//                options.TrackExceptionsAsExceptionTelemetry = true;
//            }
//    );

//SECURITY SETTINS
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddKeycloak(builder.Configuration, builder.Environment);

builder.AddInfrastructure(swaggerDocumentOptions: [], enableSwagger: enableSwagger, enableFastEndpoints: enableFastEndpoints, addCaching: enableCaching);

//SERVICES
builder.Services.AddBusinessServices();

//REVERSE PROXY
var configuration = builder.Configuration.GetSection("ReverseProxy");
builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration)
    .AddSwagger(configuration);
//NOTE: use this to disable the propagation of the activity headers (https://microsoft.github.io/reverse-proxy/articles/distributed-tracing.html)
//.ConfigureHttpClient((context, handler) => handler.ActivityHeadersPropagator = null);

//TRACES AND LOGS OF THE GATEWAY
builder.Services.AddOpenTelemetry()
    .WithMetrics(builder => builder.AddPrometheusExporter())
    .WithTracing(t =>
    {
        // Listen to the YARP tracing activities
        t.AddSource("Yarp.ReverseProxy");
    });

//SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

//APP
var app = builder.Build();

app.UseInfrastructure(builder.Environment, enableSwagger: enableSwagger, enableFastEndpoints: enableFastEndpoints);

//MIDDLEWARES
//NOTE: optional middlerwares that might be used in the gateway.
//Omit these lines if not needed.
//app.UseMiddleware<APIKeyMiddleware>();
//app.UseMiddleware<CustomAuthenticationMiddleware>();
//app.UseMiddleware<MembershipAndThrottlingMiddleware>();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

//REVERSE PROXY
app.MapReverseProxy();

//app.UseOutputCache();

//TECHNICAL SETTINGS
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//SWAGGER
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

app.Run();