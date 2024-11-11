var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Catalog_Api>("catalog-api");

builder.Build().Run();
