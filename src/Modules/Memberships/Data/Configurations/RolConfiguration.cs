using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Roles.ValueObjects;

namespace Memberships.Data.Configurations;

public class ActionConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .HasConversion(id => id.Value.ToString(), v => new RoleId(Guid.Parse(v)))
            .HasColumnType("char(36)")
            .HasColumnName("id");

        builder
            .Property(p => p.Name)
            .HasColumnType("varchar(50)")
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Enable).HasColumnName("enable").IsRequired().HasDefaultValue(true);
    }
}
