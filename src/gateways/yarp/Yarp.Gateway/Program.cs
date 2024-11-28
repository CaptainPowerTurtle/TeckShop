using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using TeckShop.Core.Exceptions;
using TeckShop.Infrastructure;
using TeckShop.Infrastructure.Auth;
using Yarp.Configs;
using Yarp.ReverseProxy.Swagger;
using Yarp.ReverseProxy.Swagger.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
bool enableSwagger = false;
bool enableFastEndpoints = false;
bool enableCaching = false;

// Add keycloak Authentication.
KeycloakAuthenticationOptions keycloakOptions = builder.Configuration.GetKeycloakOptions<KeycloakAuthenticationOptions>() ?? throw new ConfigurationMissingException("Keycloak");
builder.Services.AddKeycloak(builder.Configuration, builder.Environment, keycloakOptions);

builder.AddInfrastructure(swaggerDocumentOptions: [], enableSwagger: enableSwagger, enableFastEndpoints: enableFastEndpoints, addCaching: enableCaching);

// REVERSE PROXY
IConfigurationSection configuration = builder.Configuration.GetSection("ReverseProxy");

builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration)
    .AddSwagger(configuration)
    .AddServiceDiscoveryDestinationResolver();

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

// APP
WebApplication app = builder.Build();

app.MapDefaultEndpoints();

app.UseInfrastructure(enableSwagger: enableSwagger, enableFastEndpoints: enableFastEndpoints);

// MIDDLEWARES
// NOTE: optional middlerwares that might be used in the gateway.
// Omit these lines if not needed.
// app.UseMiddleware<APIKeyMiddleware>();
// app.UseMiddleware<CustomAuthenticationMiddleware>();
// app.UseMiddleware<MembershipAndThrottlingMiddleware>();

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
    ReverseProxyDocumentFilterConfig config = app.Services.GetRequiredService<IOptionsMonitor<ReverseProxyDocumentFilterConfig>>().CurrentValue;
    foreach (string? cluster in config.Clusters.Select(cluister => cluister.Key))
    {
        options.SwaggerEndpoint($"/swagger/{cluster}/swagger.json", cluster);
    }
});

await app.RunAsync();
