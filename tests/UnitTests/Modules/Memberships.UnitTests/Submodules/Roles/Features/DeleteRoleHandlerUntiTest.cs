using Memberships.Submodules.Roles.Features.Delete;
using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Roles.ValueObjects;
using Modules.Memberships.Submodules.Roles.Contracts.Services;

public class DeleteRoleHandlerTests
{
    private readonly Mock<IGetByIdRoleService> _mockGetByIdRoleService;
    private readonly MembershipDbContext _dbContext;
    private readonly DeleteRoleHandler _handler;

    public DeleteRoleHandlerTests()
    {
        // Usamos un DbContext en memoria para pruebas
        var options = new DbContextOptionsBuilder<MembershipDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new MembershipDbContext(options);

        _mockGetByIdRoleService = new Mock<IGetByIdRoleService>();
        _handler = new DeleteRoleHandler(_dbContext, _mockGetByIdRoleService.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteRole_WhenRoleExists()
    {
        // Arrange
        Role role = Role.Create("Admin");
        Guid roleId = role.Id.Value;

        _dbContext.Roles.Add(role);

        await _dbContext.SaveChangesAsync();

        _mockGetByIdRoleService
            .Setup(service => service.HandleAsync(It.IsAny<RoleId>()))
            .ReturnsAsync(role);

        DeleteRoleCommand command = new(roleId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Role? deletedRole = await _dbContext.Roles
            .Where(x => x.Id == role.Id && x.Enable)
            .FirstOrDefaultAsync();

        Assert.Null(deletedRole);
    }
}
