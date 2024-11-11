using Catalog.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddCatalogInfrastructure();
var app = builder.Build();

app.MapDefaultEndpoints();
app.UseCatalogInfrastructure();
await app.RunAsync();
