using Memberships.Submodules.Roles.Dtos;
using Memberships.Submodules.Roles.Entities;

namespace Memberships.Submodules.Roles.Features.GetAll;

public sealed record GetAllRoleQuery() : IQuery<List<RoleDto>>;

public sealed class GetAllRoleHandler(MembershipDbContext dbContext)
    : IQueryHandler<GetAllRoleQuery, List<RoleDto>>
{
    public async Task<List<RoleDto>> Handle(
        GetAllRoleQuery request,
        CancellationToken cancellationToken
    )
    {
        List<Role> roles = await dbContext.Roles
            .AsNoTracking()
            .Where(x => x.Enable)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        return roles.Select(x => new RoleDto(x.Id.Value, x.Name)).ToList();
    }
}
