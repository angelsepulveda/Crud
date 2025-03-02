using Memberships.Submodules.Roles.Dtos;
using Memberships.Submodules.Roles.Entities;

namespace Memberships.Submodules.Roles.Features.Register;

public sealed record RegisterRolePayload(string Name);

public sealed record RegisterRoleCommand(RegisterRolePayload Payload) : ICommand<RoleDto>;

public class RegisterRoleCommandValidator : AbstractValidator<RegisterRoleCommand>
{
    public RegisterRoleCommandValidator()
    {
        RuleFor(x => x.Payload.Name).NotEmpty().MaximumLength(50);
    }
}

public sealed class RegisterRoleHandler(MembershipDbContext dbContext)
    : ICommandHandler<RegisterRoleCommand, RoleDto>
{
    public async Task<RoleDto> Handle(
        RegisterRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        Role role = Role.Create(request.Payload.Name);

        dbContext.Roles.Add(role);

        int result = await dbContext.SaveChangesAsync(cancellationToken);

        if (result == 0)
            throw new BadRequestException("The role could not be registered");

        return new RoleDto(role.Id.Value, role.Name);
    }
}
