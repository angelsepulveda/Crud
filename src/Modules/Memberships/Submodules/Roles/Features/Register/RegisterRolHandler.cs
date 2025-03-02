using Memberships.Submodules.Roles.Dtos;
using Memberships.Submodules.Roles.Entities;

namespace Memberships.Submodules.Roles.Features.Register;

public sealed record RegisterRolPayload(string Name);

public sealed record RegisterRolCommand(RegisterRolPayload Payload) : ICommand<RolDto>;

public class RegisterRolCommandValidator : AbstractValidator<RegisterRolCommand>
{
    public RegisterRolCommandValidator()
    {
        RuleFor(x => x.Payload.Name).NotEmpty().MaximumLength(50);
    }
}

public sealed class RegisterRolHandler(MembershipDbContext dbContext)
    : ICommandHandler<RegisterRolCommand, RolDto>
{
    public async Task<RolDto> Handle(
        RegisterRolCommand request,
        CancellationToken cancellationToken
    )
    {
        Rol rol = Rol.Create(request.Payload.Name);

        dbContext.Roles.Add(rol);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new RolDto(rol.Id.Value, rol.Name);
    }
}
