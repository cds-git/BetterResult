namespace Tests;

public class ElseTests
{
    [Fact]
    public void Else_Should_ReturnOriginalResult_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var nextResult = result.Else(error => Result.Failure(Error.Unexpected()));

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Else_Should_ReturnNextResult_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Failure message");
        var result = Result.Failure(error);

        // Act
        var nextResult = result.Else(err => Result.Success());

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ElseAsync_Should_ReturnOriginalResult_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var nextResult = await result.ElseAsync(error => Task.FromResult(Result.Failure(Error.Unexpected())));

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ElseAsync_Should_ReturnNextResult_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Failure message");
        var result = Result.Failure(error);

        // Act
        var nextResult = await result.ElseAsync(err => Task.FromResult(Result.Success()));

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Else_WithValue_Should_ReturnOriginalResult_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var nextResult = result.Else(error => Result<int>.Failure(Error.Unexpected()));

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
        nextResult.Value.Should().Be(42);
    }

    [Fact]
    public void Else_WithValue_Should_ReturnNextResult_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Failure message");
        var result = Result<int>.Failure(error);

        // Act
        var nextResult = result.Else(err => Result<int>.Success(100));

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
        nextResult.Value.Should().Be(100);
    }

    [Fact]
    public async Task ElseAsync_WithValue_Should_ReturnOriginalResult_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var nextResult = await result.ElseAsync(error => Task.FromResult(Result<int>.Failure(Error.Unexpected())));

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
        nextResult.Value.Should().Be(42);
    }

    [Fact]
    public async Task ElseAsync_WithValue_Should_ReturnNextResult_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Failure message");
        var result = Result<int>.Failure(error);

        // Act
        var nextResult = await result.ElseAsync(err => Task.FromResult(Result<int>.Success(100)));

        // Assert
        nextResult.IsSuccess.Should().BeTrue();
        nextResult.Value.Should().Be(100);
    }
}