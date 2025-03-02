using Memberships.Submodules.Roles.Dtos;
using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Roles.Features.Register;

namespace Memberships.UnitTests.Submodules.Roles.Features;

public class RegisterRoleHandlerUnitTest
{
    private readonly Mock<MembershipDbContext> _dbContextMock;
    private readonly RegisterRoleHandler _handler;

    public RegisterRoleHandlerUnitTest()
    {
        DbContextOptions<MembershipDbContext> options =
            new DbContextOptionsBuilder<MembershipDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

        _dbContextMock = new Mock<MembershipDbContext>(options);
        _handler = new RegisterRoleHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRegisterRolSuccessfully()
    {
        // Arrange
        RegisterRoleCommand command = new(new RegisterRolePayload("Admin"));
        _dbContextMock.Setup(db => db.Roles.Add(It.IsAny<Role>())).Verifiable();
        _dbContextMock
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        RoleDto result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Admin", result.Name);
        _dbContextMock.Verify(db => db.Roles.Add(It.IsAny<Role>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Validator_ShouldHaveValidationError_WhenNameIsEmpty()
    {
        // Arrange
        RegisterRoleCommandValidator validator = new RegisterRoleCommandValidator();
        RegisterRoleCommand command = new(new RegisterRolePayload(""));

        // Act
        TestValidationResult<RegisterRoleCommand>? result;
        result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Payload.Name);
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationError_WhenNameIsValid()
    {
        // Arrange
        RegisterRoleCommandValidator validator = new();
        RegisterRoleCommand command = new(new RegisterRolePayload("Admin"));

        // Act
        TestValidationResult<RegisterRoleCommand>? result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Payload.Name);
    }
}
