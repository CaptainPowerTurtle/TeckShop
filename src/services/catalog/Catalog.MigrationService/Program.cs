using Catalog.Infrastructure.Persistence;
using Catalog.MigrationService;
using Catalog.Infrastructure;
using Catalog.MigrationService;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using TeckShop.Infrastructure.Auth;
using TeckShop.Infrastructure.Multitenant;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<ApiDbInitializer>();

builder.AddServiceDefaults();

var migrationAssembly = typeof(IAssemblyMarker).Assembly;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("catalogdb"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly(migrationAssembly.FullName);
    }));

builder.Services.AddMultitenantExtension(null);

var app = builder.Build();

app.Run();
