namespace Tests;

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