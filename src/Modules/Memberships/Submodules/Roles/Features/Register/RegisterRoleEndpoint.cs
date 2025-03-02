using Memberships.Submodules.Roles.Dtos;

namespace Memberships.Submodules.Roles.Features.Register;

public class RegisterRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/roles",
                async (RegisterRolePayload payload, ISender sender) =>
                {
                    RoleDto result = await sender.Send(new RegisterRoleCommand(payload));

                    return Results.Ok(result);
                }
            )
            .WithName("RegisterRol")
            .Produces<RoleDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Register a new role")
            .WithDescription(
                "This endpoint allows you to register a new role by providing the necessary details in the payload. "
                    + "The payload must include the role name and other required information. "
                    + "It returns the registered role details upon successful registration."
            )
            .WithTags("Roles");
    }
}
