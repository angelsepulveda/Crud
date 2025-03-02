using Memberships.Submodules.Roles.Dtos;

namespace Memberships.Submodules.Roles.Features.Register;

public class RegisterSectionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/sections",
                async (RegisterRolPayload payload, ISender sender) =>
                {
                    RolDto result = await sender.Send(new RegisterRolCommand(payload));

                    return Results.Ok(result);
                }
            )
            .WithName("RegisterRol")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Register a new role")
            .WithDescription(
                "This endpoint allows you to register a new role by providing the necessary details in the payload. It returns the registered role details upon successful registration."
            )
            .WithTags("Roles");
    }
}
