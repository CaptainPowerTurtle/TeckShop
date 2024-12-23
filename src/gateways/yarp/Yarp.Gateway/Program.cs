using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using TeckShop.Infrastructure;
using TeckShop.Infrastructure.Auth;
using Yarp.Configs;
using Yarp.ReverseProxy.Swagger;
using Yarp.ReverseProxy.Swagger.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
bool enableSwagger = false;
bool enableFastEndpoints = false;
bool enableCaching = false;

// Add keycloak Authentication.
KeycloakAuthenticationOptions keycloakOptions = builder.Configuration.GetKeycloakOptions<KeycloakAuthenticationOptions>()!;
builder.Services.AddKeycloak(builder.Configuration, builder.Environment, keycloakOptions);

builder.AddInfrastructure(swaggerDocumentOptions: [], enableSwagger: enableSwagger, enableFastEndpoints: enableFastEndpoints, addCaching: enableCaching);

// REVERSE PROXY
var configuration = builder.Configuration.GetSection("ReverseProxy");
builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration)
    .AddSwagger(configuration)
    .AddServiceDiscoveryDestinationResolver();

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

// APP
var app = builder.Build();

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
    var config = app.Services.GetRequiredService<IOptionsMonitor<ReverseProxyDocumentFilterConfig>>().CurrentValue;
    foreach (var cluster in config.Clusters)
    {
        options.SwaggerEndpoint($"/swagger/{cluster.Key}/swagger.json", cluster.Key);
    }
});

await app.RunAsync();
