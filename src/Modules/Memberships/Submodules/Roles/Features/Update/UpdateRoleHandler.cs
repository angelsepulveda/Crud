using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Roles.ValueObjects;
using Modules.Memberships.Submodules.Roles.Contracts.Services;

namespace Memberships.Submodules.Roles.Features.Update;

public sealed record UpdateRolePayload(Guid Id, string Name);

public sealed record UpdateRoleCommand(UpdateRolePayload Payload) : ICommand;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Payload.Id).NotNull();
        RuleFor(x => x.Payload.Name).NotEmpty().MaximumLength(50);
    }
}

public sealed class UpdateRoleHandler(
    MembershipDbContext dbContext,
    IGetByIdRoleService getByIdRoleService
) : ICommandHandler<UpdateRoleCommand>
{
    public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        Role rolUpdated = await getByIdRoleService.HandleAsync(new RoleId(request.Payload.Id));

        rolUpdated.Update(request.Payload.Name);

        int result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result == 0)
            throw new BadRequestException("The role could not be updated");

        return Unit.Value;
    }
}
