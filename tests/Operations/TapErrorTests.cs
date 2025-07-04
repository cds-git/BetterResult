namespace BetterResult.Tests;

public class TapErrorErrorTests
{
    [Fact]
    public void TapError_Should_InvokeOnSuccess_When_ResultTvalueIsFailure()
    {
        // Arrange
        var error = Error.Validation("E020", "TapError test");
        var result = Result<int>.Failure(error);

        // Act
        var r2 = result.TapError(err => Console.WriteLine(err.Message));

        // Assert
        r2.Should().BeSameAs(result);
    }


    [Fact]
    public async Task TapErrorAsync_Should_InvokeOnSuccess_When_ResultTvalueErrorIsTask()
    {
        // Arrange
        var error = Error.Validation("E020", "TapError test");
        var result = Result.Failure<int>(error);

        // Act
        var r2 = await result.TapErrorAsync((error) => Task.Run(() => Console.WriteLine(error.Message)));

        // Assert
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task TapErrorAsync_Should_InvokeOnSuccess_When_ResultTvalueIsTaskAndError()
    {
        // Arrange
        var error = Error.Validation("E020", "TapError test");
        var result = GetErrorResultAsync<int>(error);

        // Act
        var r2 = await result.TapErrorAsync((value) => Console.WriteLine(error.Message));

        // Assert
        r2.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task TapErrorAsync_Should_InvokeOnSuccess_When_ResultTvalueIsTaskAndErrorIsTask()
    {
        // Arrange
        var error = Error.Validation("E020", "TapError test");
        var result = GetErrorResultAsync<string>(error);

        // Act
        var r2 = await result.TapErrorAsync((error) => Task.Run(() => Console.WriteLine(error.Message)));

        // Assert
        r2.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void TapError_Should_InvokeOnSuccess_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Validation("E020", "TapError test");
        var result = Result.Failure(error);

        // Act
        var r2 = result.TapError((error) => Console.WriteLine(error.Message));

        // Assert
        r2.Should().BeSameAs(result);
    }


    [Fact]
    public async Task TapErrorAsync_Should_InvokeOnSuccess_When_ResultIsTaskAndSuccess()
    {
        // Arrange
        var error = Error.Validation("E020", "TapError test");
        var result = Result.Failure(error);
        var flag = false;

        // Act
        var r2 = await result.TapErrorAsync((error) => Task.Run(() => flag = error.Type is ErrorType.Validation));

        // Assert
        flag.Should().BeTrue();
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task TapErrorAsync_Should_InvokeOnSuccess_When_ResultIsTaskIsFailure()
    {
        // Arrange
        var error = Error.Validation("E020", "TapError test");
        var result = GetErrorResultAsync(error);
        var flag = false;

        // Act
        var r2 = await result.TapErrorAsync((error) => flag = error.Type is ErrorType.Validation);

        // Assert
        flag.Should().BeTrue();
        r2.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task TapErrorAsync_Should_InvokeOnSuccess_When_ResultIsTaskIsTaskAndSuccess()
    {
        // Arrange
        var error = Error.Validation("E020", "TapError test");
        var result = GetErrorResultAsync(error);
        var flag = false;

        // Act
        var r2 = await result.TapErrorAsync((error) => Task.Run(() => flag = error.Type is ErrorType.Validation));

        // Assert
        flag.Should().BeTrue();
        r2.IsFailure.Should().BeTrue();
    }

    private static Task<Result<T>> GetErrorResultAsync<T>(Error error) => Task.FromResult(Result.Failure<T>(error));
    private static Task<Result> GetErrorResultAsync(Error error) => Task.FromResult(Result.Failure(error));
}