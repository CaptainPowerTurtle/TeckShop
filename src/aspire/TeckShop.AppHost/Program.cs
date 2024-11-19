using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("redis");

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume(isReadOnly: false);
var catalogdb = postgres.AddDatabase("catalogdb");

var rabbitmq = builder.AddRabbitMQ("rabbitmq").WithManagementPlugin();

var keycloak = builder.AddKeycloakContainer("keycloak", "25.0.6")
    .WithDataVolume()
    .WithImport("./KeycloakConfiguration/teckshop-realm.json");

var realm = keycloak.AddRealm("TeckShop");

var catalogMigrationService = builder.AddProject<Projects.Catalog_MigrationService>("catalog-migrationservice")
    .WithReference(catalogdb)
    .WaitFor(catalogdb);

var catalogapi = builder.AddProject<Projects.Catalog_Api>("catalog-api")
    .WithReference(cache)
    .WithReference(catalogdb)
    .WithReference(rabbitmq)
    .WithReference(keycloak)
    .WithReference(realm)
    .WaitForCompletion(catalogMigrationService)
    .WaitFor(cache)
    .WaitFor(rabbitmq);

IResourceBuilder<ProjectResource> yarp = builder.AddProject<Projects.Yarp_Gateway>("yarp-gateway")
    .WaitFor(catalogapi)
    .WithReference(catalogapi);

builder.AddPnpmApp("teckshop-admin", "../../web", "dev:admin")
    .WithPnpmPackageInstallation()
    .WithExternalHttpEndpoints()
    .WaitFor(yarp)
    .WithReference(yarp)
    .WithReference(keycloak)
    .WithReference(realm)
    .WithEnvironment("TECKNET_BACKEND_API_URL", yarp.GetEndpoint("http"))
    .WithEnvironment("KEYCLOAK_CLIENT_ID", "admin")
    .WithEnvironment("KEYCLOAK_CLIENT_SECRET", "RZe7Hh5sDzcSZRoFHtXNhhPjNJBCP5Dx")
    .WithEnvironment("KEYCLOAK_ISSUER", $"{keycloak.GetEndpoint($"http")}/realms/TeckShop");

builder.Build().Run();
