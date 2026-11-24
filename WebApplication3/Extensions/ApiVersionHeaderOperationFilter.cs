using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApplication3.Extensions
{
    // Versiyonlama "api-version" header'ı ile yapıldığı için Swagger'daki her isteğe
    // bu header'ı ekler. Varsayılan değer, açık olan Swagger dokümanının sürümüdür (v1 -> 1.0, v2 -> 2.0).
    public class ApiVersionHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var groupName = context.ApiDescription.GroupName ?? "v1";
            var version = groupName.TrimStart('v', 'V') + ".0";

            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "api-version",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString(version)
                }
            });
        }
    }
}
