namespace BetterResult.Tests;

public class ResultTests
{
    [Fact]
    public void Success_Should_CreateResultTValue_WithSuccessState_AndCorrectValue()
    {
        // Act
        Result<int> result = 42;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Failure_Should_CreateResultTValue_WithFailureState()
    {
        // Arrange
        Error error = Error.Failure("E001", "Some failure occurred");

        // Act
        Result<int> result = error;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void AccessingValue_Should_ThrowException_When_ResultIsFailure()
    {
        // Arrange
        Error error = Error.Failure("E001", "Some failure occurred");
        Result<int> result = error;

        // Act
        Action act = () => { var value = result.Value; };

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AccessingValue_Should_ReturnSuccessResult_When_ValueIsValid()
    {
        // Act
        Result<int> result = 1;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(1);
    }

    [Fact]
    public void AccessingValue_Should_ReturnSuccessResult_When_ValueIsNullable()
    {
        // Act
        Result<int?> result = (int?)null;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void AccessingError_Should_ThrowException_When_ResultWithValueIsSuccess()
    {
        // Arrange
        Result<int> result = 42;

        // Act
        Action act = () => { var error = result.Error; };

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ImplicitConversion_Should_CreateSuccessResult_When_ValueIsProvided()
    {
        // Act
        Result<int> result = 42;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void ImplicitConversion_Should_CreateFailureResult_When_ErrorIsProvided()
    {
        // Arrange
        Error error = Error.Failure("E001", "Some failure occurred");

        // Act
        Result<int> result = error;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void NoValue_Should_CreateSuccessResult()
    {
        // Act
        Result<NoValue> result = NoValue.Instance;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(NoValue.Instance);
    }

    [Fact]
    public void NoValue_Should_CreateFailureResult_When_ErrorProvided()
    {
        // Arrange
        Error error = Error.Validation("TEST", "Test error");

        // Act
        Result<NoValue> result = error;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void NoValue_Should_ChainWith_ResultT()
    {
        // Arrange
        Result<int> GetUser(int id) => id > 0 ? id : Error.NotFound("NOT_FOUND", "User not found");
        Result<NoValue> ValidateUser(int userId) => userId > 0 ? NoValue.Instance : Error.Validation("INVALID", "Invalid user");

        // Act
        Result<string> result = GetUser(1)
            .Bind(user => ValidateUser(user))
            .Map(_ => "Success!");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("Success!");
    }

    [Fact]
    public void NoValue_Should_PropagateError_InChain()
    {
        // Arrange
        Result<int> GetUser(int id) => id > 0 ? id : Error.NotFound("NOT_FOUND", "User not found");
        Result<NoValue> ValidateUser(int userId) => userId > 0 ? NoValue.Instance : Error.Validation("INVALID", "Invalid user");

        // Act
        Result<string> result = GetUser(-1)
            .Bind(user => ValidateUser(user))
            .Map(_ => "Success!");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("NOT_FOUND");
    }
}