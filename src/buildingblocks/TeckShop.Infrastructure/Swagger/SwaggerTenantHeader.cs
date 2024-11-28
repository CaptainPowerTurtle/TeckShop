using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace TeckShop.Infrastructure.Swagger
{
    internal sealed class SwaggerTenantHeader : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            OpenApiParameter hdrParameter = new()
            {
                Name = "x-tenant-id",
                Kind = OpenApiParameterKind.Header,
                IsRequired = true,
                Type = JsonObjectType.String,
                Default = "",
                Description = "The Id of the tenant"
            };

            context.OperationDescription.Operation.Parameters.Add(hdrParameter);

            return true;
        }
    }
}
