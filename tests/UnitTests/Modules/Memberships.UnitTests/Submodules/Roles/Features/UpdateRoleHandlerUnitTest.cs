using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Roles.Features.Update;
using Memberships.Submodules.Roles.ValueObjects;
using Modules.Memberships.Submodules.Roles.Contracts.Services;

namespace Memberships.UnitTests.Submodules.Roles.Features;

public class UpdateRoleHandlerUnitTest
{
    private readonly Mock<MembershipDbContext> _dbContextMock;
    private readonly Mock<IGetByIdRoleService> _getByIdRoleServiceMock;
    private readonly UpdateRoleHandler _handler;

    public UpdateRoleHandlerUnitTest()
    {
        DbContextOptions<MembershipDbContext> options =
            new DbContextOptionsBuilder<MembershipDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

        _dbContextMock = new Mock<MembershipDbContext>(options);
        _getByIdRoleServiceMock = new Mock<IGetByIdRoleService>();
        _handler = new UpdateRoleHandler(_dbContextMock.Object, _getByIdRoleServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateRoleSuccessfully()
    {
        // Arrange
        Role existingRole = Role.Create("User");
        _getByIdRoleServiceMock
            .Setup(service => service.HandleAsync(It.IsAny<RoleId>()))
            .ReturnsAsync(existingRole);

        UpdateRoleCommand command = new(new UpdateRolePayload(existingRole.Id.Value, "Admin"));
        _dbContextMock
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Admin", existingRole.Name);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Validator_ShouldHaveValidationError_WhenNameIsEmpty()
    {
        // Arrange
        UpdateRoleCommandValidator validator = new();
        UpdateRoleCommand command = new(new UpdateRolePayload(Guid.NewGuid(), ""));

        // Act
        TestValidationResult<UpdateRoleCommand>? result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Payload.Name);
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationError_WhenNameIsValid()
    {
        // Arrange
        UpdateRoleCommandValidator validator = new();
        UpdateRoleCommand command = new(new UpdateRolePayload(Guid.NewGuid(), "Admin"));

        // Act
        TestValidationResult<UpdateRoleCommand>? result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Payload.Name);
    }
}
