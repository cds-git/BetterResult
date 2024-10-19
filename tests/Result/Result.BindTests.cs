namespace Tests;

public class BindTests
{
    [Fact]
    public void Bind_Should_ReturnNextSuccessResult_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var nextResult = result.Bind(() => Result.Success());

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Bind_Should_ReturnFailureResult_When_ResultIsFailure()
    {
        // Arrange
        var result = (Result)Error.Failure("E001", "Failure message");

        // Act
        var nextResult = result.Bind(() => Error.Unexpected());

        // Assert
        nextResult.IsFailure.Should().BeTrue();
        nextResult.Error.Type.Should().Be(ErrorType.Failure);
    }

    [Fact]
    public async Task BindAsync_Should_ReturnNextSuccessResult_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var nextResult = await result.BindAsync(() => Task.FromResult(Result.Success()));

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task BindAsync_Should_ReturnFailureResult_When_ResultIsFailure()
    {
        // Arrange
        var result = (Result)Error.Failure("E001", "Failure message");

        // Act
        var nextResult = await result.BindAsync(() => Task.FromResult(Result.Failure(Error.Unexpected())));

        // Assert
        nextResult.IsFailure.Should().BeTrue();
        nextResult.Error.Type.Should().Be(ErrorType.Failure);
    }

    [Fact]
    public void Bind_WithValue_Should_ReturnNextSuccessResult_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var nextResult = result.Bind(value => Result<string>.Success($"Value is {value}"));

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
        nextResult.Value.Should().Be("Value is 42");
    }

    [Fact]
    public void Bind_WithValue_Should_ReturnFailureResult_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Failure message");
        var result = Result<int>.Failure(error);

        // Act
        var nextResult = result.Bind(value => Result<string>.Success($"Value is {value}"));

        // Assert
        nextResult.IsFailure.Should().BeTrue();
        nextResult.Error.Should().Be(error);
    }

    [Fact]
    public async Task BindAsync_WithValue_Should_ReturnNextSuccessResult_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var nextResult = await result.BindAsync(value => Task.FromResult( Result<string>.Success($"Value is {value}")));

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
        nextResult.Value.Should().Be("Value is 42");
    }

    [Fact]
    public async Task BindAsync_WithValue_Should_ReturnFailureResult_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Failure message");
        var result = Result<int>.Failure(error);

        // Act
        var nextResult = await result.BindAsync(value => Task.FromResult(Result<string>.Success($"Value is {value}")));

        // Assert
        nextResult.IsFailure.Should().BeTrue();
        nextResult.Error.Should().Be(error);
    }
}