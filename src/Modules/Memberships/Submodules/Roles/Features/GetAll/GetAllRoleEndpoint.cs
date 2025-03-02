using Memberships.Submodules.Roles.Dtos;

namespace Memberships.Submodules.Roles.Features.GetAll;

public class GetAllRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/roles",
                async (ISender sender) =>
                {
                    List<RoleDto> roles = await sender.Send(new GetAllRoleQuery());

                    return Results.Ok(roles);
                }
            )
            .WithName("GetAllRole")
            .Produces<List<RoleDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Retrieve all roles")
            .WithDescription(
                "This endpoint retrieves a list of all roles available in the system. "
                    + "It returns a list of role details upon successful retrieval."
            )
            .WithTags("Roles");
    }
}
