namespace Tests;

public class ResultFactoryTests
{
    [Fact]
    public void Success_Should_CreateResult_WithSuccessState()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Failure_Should_CreateResult_WithFailureState()
    {
        // Arrange
        var error = Error.Failure("E001", "Some failure occurred");

        // Act
        var result = Result.Failure(error);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void AccessingError_Should_ThrowException_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        Action act = () => { var error = result.Error; };

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot access Error when result is of success");
    }

    [Fact]
    public void Success_Should_CreateResultTValue_WithSuccessState_AndCorrectValue()
    {
        // Act
        var result = Result.Success(42);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Failure_Should_CreateResultTValue_WithFailureState()
    {
        // Arrange
        var error = Error.Failure("E001", "Some failure occurred");

        // Act
        var result = Result.Failure<int>(error);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void AccessingValue_Should_ThrowException_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Some failure occurred");
        var result = Result.Failure<int>(error);

        // Act
        Action act = () => { var value = result.Value; };

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot access the value when result is of type failure. Check IsFailure before accessing value!");
    }

   [Fact]
    public void AccessingValue_Should_ReturnSuccessResult_When_ValueIsValid()
    {
        // Act
        var result = Result.Success<int>(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(1);
    }

   [Fact]
    public void AccessingValue_Should_ReturnSuccessResult_When_ValueIsNullable()
    {
        // Act
        var result = Result.Success<int?>(null);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void AccessingError_Should_ThrowException_When_ResultWithValueIsSuccess()
    {
        // Arrange
        var result = Result.Success(42);

        // Act
        Action act = () => { var error = result.Error; };

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot access the error when the result is of type success. Check IsFailure before accessing Error!");
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
        var error = Error.Failure("E001", "Some failure occurred");

        // Act
        Result<int> result = error;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }
}