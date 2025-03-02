namespace Modules.Memberships.Submodules.Roles.Exceptions;

public class RolNotFoundException : NotFoundException
{
    public RolNotFoundException(Guid id)
        : base($"Rol with id {id} not found") { }
}
