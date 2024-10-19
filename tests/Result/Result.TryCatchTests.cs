namespace Tests;

public class TryCatchTests
{
    [Fact]
    public void TryCatch_Should_ReturnSuccessResult_When_NoExceptionThrown()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var tryCatchResult = result.TryCatch(
            () => Result.Success(42),
            Error.Failure("E002", "Exception caught"));

        // Assert
        tryCatchResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void TryCatch_Should_ReturnFailureResult_When_ExceptionIsThrown()
    {
        // Arrange
        var result = Result.Success();
        var error = Error.Failure("E002", "Exception caught");

        // Act
        var tryCatchResult = result.TryCatch(
            () => throw new Exception(),
            error);

        // Assert
        tryCatchResult.IsFailure.Should().BeTrue();
        tryCatchResult.Error.Should().Be(error);
    }

    [Fact]
    public async Task TryCatchAsync_Should_ReturnSuccessResult_When_NoExceptionThrown()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var tryCatchResult = await result.TryCatchAsync(
            () => Task.FromResult(Result.Success()),
            Error.Failure("E002", "Exception caught"));

        // Assert
        tryCatchResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task TryCatchAsync_Should_ReturnFailureResult_When_ExceptionIsThrown()
    {
        // Arrange
        var result = Result.Success();
        var error = Error.Failure("E002", "Exception caught");

        // Act
        var tryCatchResult = await result.TryCatchAsync(
            () => throw new Exception(),
            error);

        // Assert
        tryCatchResult.IsFailure.Should().BeTrue();
        tryCatchResult.Error.Should().Be(error);
    }

    [Fact]
    public void TryCatch_WithValue_Should_ReturnSuccessResult_When_NoExceptionThrown()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var tryCatchResult = result.TryCatch(
            value => $"Value is {value}",
            Error.Failure("E002", "Exception caught"));

        // Assert
        tryCatchResult.IsSuccess.Should().BeTrue();
        tryCatchResult.Value.Should().Be("Value is 42");
    }

    [Fact]
    public void TryCatch_WithValue_Should_ReturnFailureResult_When_ExceptionIsThrown()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var error = Error.Failure("E002", "Exception caught");

        // Act
        var tryCatchResult = result.TryCatch<int>(
            value => throw new Exception(),
            error);

        // Assert
        tryCatchResult.IsFailure.Should().BeTrue();
        tryCatchResult.Error.Should().Be(error);
    }

    [Fact]
    public async Task TryCatchAsync_WithValue_Should_ReturnSuccessResult_When_NoExceptionThrown()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var tryCatchResult = await result.TryCatchAsync(
            value => Task.FromResult($"Value is {value}"),
            Error.Failure("E002", "Exception caught"));

        // Assert
        tryCatchResult.IsSuccess.Should().BeTrue();
        tryCatchResult.Value.Should().Be("Value is 42");
    }

    [Fact]
    public async Task TryCatchAsync_WithValue_Should_ReturnFailureResult_When_ExceptionIsThrown()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var error = Error.Failure("E002", "Exception caught");

        // Act
        var tryCatchResult = await result.TryCatchAsync<int>(
            value => throw new Exception(),
            error);

        // Assert
        tryCatchResult.IsFailure.Should().BeTrue();
        tryCatchResult.Error.Should().Be(error);
    }
}