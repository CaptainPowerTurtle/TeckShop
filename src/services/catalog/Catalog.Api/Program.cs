using Catalog.Infrastructure;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddCatalogInfrastructure();
WebApplication app = builder.Build();

app.MapDefaultEndpoints();
app.UseCatalogInfrastructure();
await app.RunAsync();
