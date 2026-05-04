namespace BetterResult.Tests;

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
        var updatedError = error.WithMetadata(1000); // Will use "System.Int32" as key

        // Assert
        updatedError.Metadata.Should().ContainKey("System.Int32").WhoseValue.Should().Be(1000);
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
    public void GetMetadataByKey_Should_ReturnDefault_When_KeyDoesNotExist()
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

    [Fact]
    public void Metadata_Should_NotBeMutable_ViaDowncast()
    {
        // Regression: an immutable record struct must not let callers mutate the metadata
        // by downcasting the IReadOnlyDictionary back to Dictionary.
        var error = Error.Failure("E001", "Some error", new Dictionary<string, object> { ["k"] = 1 });

        // The metadata is exposed as IReadOnlyDictionary; the underlying type must not be
        // a mutable Dictionary that the caller can downcast to.
        (error.Metadata is Dictionary<string, object>).Should().BeFalse();

        Action mutate = () =>
        {
            if (error.Metadata is IDictionary<string, object> mutable)
                mutable["evil"] = "x";
        };

        mutate.Should().Throw<NotSupportedException>();
        error.Metadata.Should().HaveCount(1);
    }

    [Fact]
    public void EmptyMetadataDictionary_Should_BeStoredAsNull()
    {
        // Regression: an empty input dictionary should not produce a non-null empty Metadata.
        var error = Error.Failure("E001", "Some error", new Dictionary<string, object>());

        error.Metadata.Should().BeNull();
    }

    [Fact]
    public void WithMetadata_Should_BeNoOp_When_NoMetadataIsAdded()
    {
        // Regression: WithMetadata(null) on an error with no metadata used to flip Metadata
        // from null to an empty Dictionary, breaking value-equality and immutability expectations.
        var original = Error.Failure("E001", "Some error");

        var afterDict = original.WithMetadata((Dictionary<string, object>?)null);
        afterDict.Metadata.Should().BeNull();
        afterDict.Should().Be(original);

        var afterTyped = original.WithMetadata<string>(null);
        afterTyped.Metadata.Should().BeNull();
        afterTyped.Should().Be(original);
    }

    [Fact]
    public void Constructor_Should_RejectNullCodeOrMessage()
    {
        Action nullCode = () => Error.Failure(null!, "msg");
        Action nullMessage = () => Error.Failure("CODE", null!);

        nullCode.Should().Throw<ArgumentNullException>();
        nullMessage.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Equals_Should_DistinguishOnTypeCodeMessage()
    {
        var baseline = Error.Failure("E001", "msg");

        baseline.Should().NotBe(Error.Validation("E001", "msg"));
        baseline.Should().NotBe(Error.Failure("E002", "msg"));
        baseline.Should().NotBe(Error.Failure("E001", "different message"));
    }

    [Fact]
    public void Equals_Should_TreatNoMetadataErrorsAsEqual()
    {
        // Two independently-built errors with no metadata are equal under default record-struct
        // equality (Metadata is null on both → reference-equal).
        var a = Error.Failure("E001", "msg");
        var b = Error.Failure("E001", "msg");

        a.Should().Be(b);
        a.GetHashCode().Should().Be(b.GetHashCode());
    }
}