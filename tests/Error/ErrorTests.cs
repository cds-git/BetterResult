namespace Tests;

public class ErrorTests
{
    [Fact]
    public void WithMessage_Should_PrependNewMessage_When_MessageIsProvided()
    {
        // Arrange
        var error = Error.Failure("E001", "Original error message");

        // Act
        var updatedError = error.WithMessage("New error detail");

        // Assert
        updatedError.Message.Should().Be("New error detail: Original error message");
        updatedError.Code.Should().Be(error.Code);
        updatedError.Type.Should().Be(error.Type);
        updatedError.Metadata.Should().BeNull();
    }

    [Fact]
    public void WithMetadata_Should_AddNewMetadata_When_MetadataIsProvided()
    {
        // Arrange
        var error = Error.Failure("E001", "Some error");
        var metadata = new Dictionary<string, object>
        {
            { "RetryCount", 3 },
            { "Source", "ServiceA" }
        };

        // Act
        var updatedError = error.WithMetadata(metadata);

        // Assert
        updatedError.Metadata.Should().Contain(metadata);
        updatedError.Metadata.Should().HaveCount(2);
    }

    [Fact]
    public void WithMetadata_Should_OverrideMetadata_When_KeyAlreadyExists()
    {
        // Arrange
        var error = Error.Failure("E001", "Some error", new Dictionary<string, object> { { "RetryCount", 1 } });
        var metadata = new Dictionary<string, object> { { "RetryCount", 5 } };

        // Act
        var updatedError = error.WithMetadata(metadata);

        // Assert
        updatedError.Metadata.Should().ContainKey("RetryCount").WhoseValue.Should().Be(5);
    }

    [Fact]
    public void WithMetadata_Should_AddOrUpdateSingleKey_When_KeyAndValueProvided()
    {
        // Arrange
        var error = Error.Failure("E001", "Some error", new Dictionary<string, object> { { "UserId", 1234 } });

        // Act
        var updatedError = error.WithMetadata("RetryCount", 3);

        // Assert
        updatedError.Metadata.Should().Contain(new KeyValuePair<string, object>("RetryCount", 3));
        updatedError.Metadata.Should().Contain(new KeyValuePair<string, object>("UserId", 1234));
    }

    [Fact]
    public void WithMetadata_Should_AddMetadataByType_When_TypeIsProvided()
    {
        // Arrange
        var error = Error.Failure("E001", "Some error");

        // Act
        var updatedError = error.WithMetadata(1000); // Will use "Int32" as key

        // Assert
        updatedError.Metadata.Should().ContainKey("Int32").WhoseValue.Should().Be(1000);
    }

    [Fact]
    public void GetMetadataByKey_Should_ReturnValue_When_KeyExists()
    {
        // Arrange
        var metadata = new Dictionary<string, object> { { "RetryCount", 3 } };
        var error = Error.Failure("E001", "Some error", metadata);

        // Act
        var retryCount = error.GetMetadata<int>("RetryCount");

        // Assert
        retryCount.Should().Be(3);
    }

    [Fact]
    public void GetMetadataByKey_Should_ReturnNull_When_KeyDoesNotExist()
    {
        // Arrange
        var error = Error.Failure("E001", "Some error");

        // Act
        var referenceType = error.GetMetadata<string>("RetryCount");
        var valueType = error.GetMetadata<int>("RetryCount");

        // Assert
        referenceType.Should().BeNull();
        valueType.Should().Be(0);
    }

    [Fact]
    public void GetMetadataByKey_Should_ThrowInvalidCastException_When_TypeDoesNotMatch()
    {
        // Arrange
        var metadata = new Dictionary<string, object> { { "RetryCount", 3 } };
        var error = Error.Failure("E001", "Some error", metadata);

        // Act
        Action act = () => error.GetMetadata<string>("RetryCount");

        // Assert
        act.Should().Throw<InvalidCastException>();
    }

    [Fact]
    public void GetMetadataByType_Should_ReturnValue_When_TypeExists()
    {
        // Arrange
        var metadata = new Dictionary<string, object> { { "RetryCount", 3 }, { "Description", "Error message" } };
        var error = Error.Failure("E001", "Some error", metadata);

        // Act
        var retryCount = error.GetMetadata<int>();

        // Assert
        retryCount.Should().Be(3);
    }

    [Fact]
    public void GetMetadataByType_Should_ReturnDefault_When_TypeDoesNotExist()
    {
        // Arrange
        var error = Error.Failure("E001", "Some error");

        // Act
        var referenceType = error.GetMetadata<string>();
        var valueType = error.GetMetadata<int>();

        // Assert
        referenceType.Should().BeNull();
        valueType.Should().Be(0);
    }
}