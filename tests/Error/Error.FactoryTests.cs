namespace BetterResult.Tests;

public class ErrorFactoryTests
{
    [Fact]
    public void Failure_Should_CreateError_WithCorrectProperties()
    {
        // Act
        var error = Error.Failure("E001", "Some failure occurred");

        // Assert
        error.Code.Should().Be("E001");
        error.Message.Should().Be("Some failure occurred");

        error.Type.Should().Be(ErrorType.Failure);
        error.Metadata.Should().BeNull();
    }

    [Fact]
    public void Unexpected_Should_CreateError_WithCorrectProperties()
    {
        // Act
        var error = Error.Unexpected("E002", "Unexpected error");

        // Assert
        error.Code.Should().Be("E002");
        error.Message.Should().Be("Unexpected error");
        error.Type.Should().Be(ErrorType.Unexpected);
        error.Metadata.Should().BeNull();
    }

    [Fact]
    public void Validation_Should_CreateError_WithCorrectProperties()
    {
        // Act
        var error = Error.Validation("E003", "Validation error");

        // Assert
        error.Code.Should().Be("E003");
        error.Message.Should().Be("Validation error");
        error.Type.Should().Be(ErrorType.Validation);
        error.Metadata.Should().BeNull();
    }

    [Fact]
    public void Forbidden_Should_CreateError_WithCorrectProperties()
    {
        // Act
        var error = Error.Forbidden("E403", "Insufficient permission");

        // Assert
        error.Code.Should().Be("E403");
        error.Message.Should().Be("Insufficient permission");
        error.Type.Should().Be(ErrorType.Forbidden);
        error.Metadata.Should().BeNull();
    }

    [Fact]
    public void Forbidden_Should_UseDefaults_When_NoArgumentsSupplied()
    {
        // Act
        var error = Error.Forbidden();

        // Assert
        error.Code.Should().Be("General.Forbidden");
        error.Message.Should().Be("Forbidden");
        error.Type.Should().Be(ErrorType.Forbidden);
    }

    [Fact]
    public void Forbidden_Should_BeDistinctFrom_Unauthorized()
    {
        // Act
        var forbidden = Error.Forbidden("E403", "Insufficient permission");
        var unauthorized = Error.Unauthorized("E401", "Not authenticated");

        // Assert
        forbidden.Type.Should().NotBe(unauthorized.Type);
    }

    [Theory]
    [InlineData(ErrorType.Failure, 0)]
    [InlineData(ErrorType.Unexpected, 1)]
    [InlineData(ErrorType.Validation, 2)]
    [InlineData(ErrorType.NotFound, 3)]
    [InlineData(ErrorType.Conflict, 4)]
    [InlineData(ErrorType.Unauthorized, 5)]
    [InlineData(ErrorType.Unavailable, 6)]
    [InlineData(ErrorType.Timeout, 7)]
    [InlineData(ErrorType.Forbidden, 8)]
    public void ErrorType_Should_HaveStableNumericValues(ErrorType type, int expected)
    {
        // Callers may persist or transmit ErrorType as an integer, so new members must be
        // appended rather than inserted. Reordering this enum is a breaking change.
        ((int)type).Should().Be(expected);
    }

    [Fact]
    public void Custom_Should_CreateError_WithCorrectProperties()
    {
        // Act
        var error = Error.Create(ErrorType.Failure, "CustomCode", "Custom description");

        // Assert
        error.Code.Should().Be("CustomCode");
        error.Message.Should().Be("Custom description");
        error.Type.Should().Be(ErrorType.Failure);
        error.Metadata.Should().BeNull();
    }

    [Fact]
    public void Error_Should_Allow_Metadata_ToBeIncluded()
    {
        // Arrange
        var metadata = new Dictionary<string, object> { { "Key1", "Value1" }, { "Key2", 42 } };

        // Act
        var error = Error.Failure("E004", "Error with metadata", metadata);

        // Assert
        error.Metadata.Should().Contain(metadata);
        error.Metadata.Should().HaveCount(2);
    }
}