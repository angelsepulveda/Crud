using Modules.Memberships.Submodules.Roles.Contracts.Services;
using Modules.Memberships.Submodules.Roles.Services;

namespace Memberships.Submodules.Roles.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddRolesServices(this IServiceCollection services)
    {
        services.AddScoped<IGetByIdRoleService, GetByIdRoleService>();

        return services;
    }
}
