using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Customization_Management_API.Filters;

/// <summary>
/// Documenting response status.
/// </summary>
public class AuthResponsesOperationFilter : IOperationFilter
{
    public void Apply( OpenApiOperation operation, OperationFilterContext context )
    {
        var authAttributes = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Union(context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>());

        if( authAttributes.Any() )
        {
            operation.Responses.Add("401", new OpenApiResponse{ Description = "Unauthorized - JWT token is required but was not provided or is invalid." } );

            var roles = authAttributes.Where(a => !string.IsNullOrEmpty(a.Roles)).Select(a => a.Roles);
            if( roles.Any() )
                operation.Responses.Add("403", new OpenApiResponse{ Description = string.Format("Forbidden - The user does not have one of the required roles: ({0})", string.Join(", ", roles)) } );
        }
    }
} 