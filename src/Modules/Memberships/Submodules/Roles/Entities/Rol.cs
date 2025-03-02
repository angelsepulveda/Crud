using Memberships.Submodules.Roles.ValueObjects;

namespace Memberships.Submodules.Roles.Entities;

public class Rol : Entity<RolId>
{
    public string Name { get; private set; }
    public bool Enable { get; private set; }

    private Rol(RolId id, string name, bool enable)
    {
        Id = id;
        Name = name;
        Enable = enable;
    }

    public static Rol Create(string name)
    {
        const bool enable = true;
        return new Rol(new RolId(Guid.NewGuid()), name, enable);
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
