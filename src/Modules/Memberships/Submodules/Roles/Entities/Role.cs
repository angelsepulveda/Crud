using Memberships.Submodules.Roles.ValueObjects;

namespace Memberships.Submodules.Roles.Entities;

public class Role : Entity<RoleId>
{
    public string Name { get; private set; }
    public bool Enable { get; private set; }

    private Role(RoleId id, string name, bool enable)
    {
        Id = id;
        Name = name;
        Enable = enable;
    }

    public static Role Create(string name)
    {
        const bool enable = true;
        return new Role(new RoleId(Guid.NewGuid()), name, enable);
    }

    public void Update(string name)
    {
        Name = name;
    }

    public void Delete()
    {
        Enable = false;
    }
}
