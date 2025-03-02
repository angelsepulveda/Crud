using Memberships.Submodules.Roles.Services;

namespace Memberships.Submodules.Roles;

public static class DependencyInjection
{
    public static IServiceCollection AddSubModuleRoles(this IServiceCollection services)
    {
        services.AddRolesServices();

        return services;
    }
}
