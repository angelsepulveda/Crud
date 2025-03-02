using Memberships.Submodules.Roles.Dtos;
using Memberships.Submodules.Roles.Entities;
using Memberships.Submodules.Roles.Features.Register;

namespace Memberships.UnitTests.Submodules.Roles.Features;

public class RegisterRolHandlerUnitTest
{
    private readonly Mock<MembershipDbContext> _dbContextMock;
    private readonly RegisterRolHandler _handler;

    public RegisterRolHandlerUnitTest()
    {
        DbContextOptions<MembershipDbContext> options = new DbContextOptionsBuilder<MembershipDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContextMock = new Mock<MembershipDbContext>(options);
        _handler = new RegisterRolHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRegisterRolSuccessfully()
    {
        // Arrange
        RegisterRolCommand command = new(new RegisterRolPayload("Admin"));
        _dbContextMock.Setup(db => db.Roles.Add(It.IsAny<Rol>())).Verifiable();
        _dbContextMock
            .Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        RolDto result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Admin", result.Name);
        _dbContextMock.Verify(db => db.Roles.Add(It.IsAny<Rol>()), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Validator_ShouldHaveValidationError_WhenNameIsEmpty()
    {
        // Arrange
        RegisterRolCommandValidator validator = new RegisterRolCommandValidator();
        RegisterRolCommand command = new(new RegisterRolPayload(""));

        // Act
        TestValidationResult<RegisterRolCommand>? result;
        result = validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Payload.Name);
    }

    [Fact]
    public void Validator_ShouldNotHaveValidationError_WhenNameIsValid()
    {
        // Arrange
        RegisterRolCommandValidator validator = new();
        RegisterRolCommand command = new(new RegisterRolPayload("Admin"));

        // Act
        TestValidationResult<RegisterRolCommand>? result = validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Payload.Name);
    }
}
