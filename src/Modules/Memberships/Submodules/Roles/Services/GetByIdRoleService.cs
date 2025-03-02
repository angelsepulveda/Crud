using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Roles.ValueObjects;
using Modules.Memberships.Submodules.Roles.Contracts.Services;
using Modules.Memberships.Submodules.Roles.Exceptions;

namespace Modules.Memberships.Submodules.Roles.Services;

public class GetByIdRoleService(MembershipDbContext dbContext) : IGetByIdRoleService
{
    public async Task<Role> HandleAsync(RoleId id)
    {
        Role? rol = await dbContext.Roles.Where(x => x.Id == id && x.Enable).FirstOrDefaultAsync();

        if (rol is null)
            throw new RolNotFoundException(id.Value);

        return rol;
    }
}
