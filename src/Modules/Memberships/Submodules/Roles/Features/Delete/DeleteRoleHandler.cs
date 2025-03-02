using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Roles.ValueObjects;
using Modules.Memberships.Submodules.Roles.Contracts.Services;

namespace Memberships.Submodules.Roles.Features.Delete;

public sealed record DeleteRoleCommand(Guid Id) : ICommand;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
    }
}

public sealed class DeleteRoleHandler(
    MembershipDbContext dbContext,
    IGetByIdRoleService getByIdRoleService
) : ICommandHandler<DeleteRoleCommand>
{
    public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        Role roleDeleted = await getByIdRoleService.HandleAsync(new RoleId(request.Id));

        roleDeleted.Delete();

        dbContext.Roles.Update(roleDeleted);

        int result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result == 0)
            throw new BadRequestException("The role could not be deleted");

        return Unit.Value;
    }
}
