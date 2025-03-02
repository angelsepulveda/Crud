namespace Memberships.Submodules.Roles.Features.Delete;

public class DeleteRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/api/roles/{Id:guid}",
                async (Guid Id, ISender sender) =>
                {
                    Unit result = await sender.Send(new DeleteRoleCommand(Id));

                    return Results.Ok(result);
                }
            )
            .WithName("DeleteRole")
            .Produces<Unit>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a Role")
            .WithDescription(
                "This endpoint allows you to delete an existing role by providing the role ID in the URL. "
                    + "It returns a status indicating whether the deletion was successful."
            )
            .WithTags("Roles");
    }
}
