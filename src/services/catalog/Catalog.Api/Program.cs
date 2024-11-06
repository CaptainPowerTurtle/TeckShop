using Catalog.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

builder.AddCatalogInfrastructure();
var app = builder.Build();
app.UseCatalogInfrastructure();
await app.RunAsync();
