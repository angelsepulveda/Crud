using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Roles.ValueObjects;

namespace Modules.Memberships.Submodules.Roles.Contracts.Services;

public interface IGetByIdRoleService
{
    Task<Role> HandleAsync(RoleId id);
}
