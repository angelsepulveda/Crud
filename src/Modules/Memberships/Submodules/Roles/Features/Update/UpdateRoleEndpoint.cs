namespace Memberships.Submodules.Roles.Features.Update;

public class UpdateRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
                "/api/roles",
                async (UpdateRolePayload payload, ISender sender) =>
                {
                    Unit result = await sender.Send(new UpdateRoleCommand(payload));

                    return Results.Ok(result);
                }
            )
            .WithName("UpdateRol")
            .Produces<Unit>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update a Role")
            .WithDescription(
                "Updates the details of an existing role based on the provided payload. The payload must include the role ID and the new name."
            )
            .WithTags("Roles");
    }
}
