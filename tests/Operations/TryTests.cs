namespace BetterResult.Tests;

public class TryTests
{
    [Fact]
    public void Try_Should_TransformValue_When_OperationSucceeds()
    {
        // Arrange
        var result = Result<string>.Success("42");

        // Act
        var transformed = result.Try(x => int.Parse(x));

        // Assert
        transformed.IsSuccess.Should().BeTrue();
        transformed.Value.Should().Be(42);
    }

    [Fact]
    public void Try_Should_ReturnFailure_When_OperationThrows()
    {
        // Arrange
        var result = Result<string>.Success("not a number");

        // Act
        var transformed = result.Try(x => int.Parse(x));

        // Assert
        transformed.IsFailure.Should().BeTrue();
        transformed.Error.Type.Should().Be(ErrorType.Unexpected);
        transformed.Error.Code.Should().Be("EXCEPTION");
    }

    [Fact]
    public void Try_Should_PropagateError_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Validation("INVALID", "Invalid input");
        var result = Result<string>.Failure(error);

        // Act
        var transformed = result.Try(x => int.Parse(x));

        // Assert
        transformed.IsFailure.Should().BeTrue();
        transformed.Error.Should().Be(error);
    }

    [Fact]
    public void Try_Should_IncludeExceptionMetadata_When_OperationThrows()
    {
        // Arrange
        var result = Result<string>.Success("not a number");

        // Act
        var transformed = result.Try(x => int.Parse(x));

        // Assert
        transformed.IsFailure.Should().BeTrue();
        transformed.Error.GetMetadata<string>("ExceptionType").Should().Be("FormatException");
        transformed.Error.GetMetadata<string>("StackTrace").Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Try_Should_TransformValue_When_OperationSucceedsWithErrorMapper()
    {
        // Arrange
        var result = Result<string>.Success("42");
        static Error ErrorMapper(Exception ex) => Error.Validation("CUSTOM_ERROR", ex.Message);

        // Act
        var transformed = result.Try(x => int.Parse(x), ErrorMapper);

        // Assert
        transformed.IsSuccess.Should().BeTrue();
        transformed.Value.Should().Be(42);
    }

    [Fact]
    public void Try_Should_UseCustomError_When_OperationThrowsWithErrorMapper()
    {
        // Arrange
        var result = Result<string>.Success("not a number");
        static Error ErrorMapper(Exception ex) => Error.Validation("CUSTOM_ERROR", $"Custom: {ex.Message}");

        // Act
        var transformed = result.Try(x => int.Parse(x), ErrorMapper);

        // Assert
        transformed.IsFailure.Should().BeTrue();
        transformed.Error.Type.Should().Be(ErrorType.Validation);
        transformed.Error.Code.Should().Be("CUSTOM_ERROR");
        transformed.Error.Message.Should().StartWith("Custom:");
    }

    [Fact]
    public async Task TryAsync_Should_TransformValue_When_OperationSucceeds()
    {
        // Arrange
        var result = Result<string>.Success("42");

        // Act
        var transformed = await result.TryAsync(async x =>
        {
            await Task.Delay(1);
            return int.Parse(x);
        });

        // Assert
        transformed.IsSuccess.Should().BeTrue();
        transformed.Value.Should().Be(42);
    }

    [Fact]
    public async Task TryAsync_Should_ReturnFailure_When_OperationThrows()
    {
        // Arrange
        var result = Result<string>.Success("not a number");

        // Act
        var transformed = await result.TryAsync(async x =>
        {
            await Task.Delay(1);
            return int.Parse(x);
        });

        // Assert
        transformed.IsFailure.Should().BeTrue();
        transformed.Error.Type.Should().Be(ErrorType.Unexpected);
        transformed.Error.Code.Should().Be("EXCEPTION");
    }

    [Fact]
    public async Task TryAsync_Should_PropagateError_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Validation("INVALID", "Invalid input");
        var result = Result<string>.Failure(error);

        // Act
        var transformed = await result.TryAsync(async x =>
        {
            await Task.Delay(1);
            return int.Parse(x);
        });

        // Assert
        transformed.IsFailure.Should().BeTrue();
        transformed.Error.Should().Be(error);
    }

    [Fact]
    public async Task TryAsync_Should_UseCustomError_When_OperationThrowsWithErrorMapper()
    {
        // Arrange
        var result = Result<string>.Success("not a number");
        static Error ErrorMapper(Exception ex) => Error.Validation("CUSTOM_ERROR", $"Custom: {ex.Message}");

        // Act
        var transformed = await result.TryAsync(async x =>
        {
            await Task.Delay(1);
            return int.Parse(x);
        }, ErrorMapper);

        // Assert
        transformed.IsFailure.Should().BeTrue();
        transformed.Error.Type.Should().Be(ErrorType.Validation);
        transformed.Error.Code.Should().Be("CUSTOM_ERROR");
        transformed.Error.Message.Should().StartWith("Custom:");
    }

    [Fact]
    public async Task TryAsync_TaskResult_Should_TransformValue_When_OperationSucceeds()
    {
        // Arrange
        var task = Task.FromResult(Result<string>.Success("42"));

        // Act
        var result = await task.TryAsync(x => int.Parse(x));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task TryAsync_TaskResult_Should_ReturnFailure_When_OperationThrows()
    {
        // Arrange
        var task = Task.FromResult(Result<string>.Success("not a number"));

        // Act
        var result = await task.TryAsync(x => int.Parse(x));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Unexpected);
        result.Error.Code.Should().Be("EXCEPTION");
    }

    [Fact]
    public async Task TryAsync_TaskResult_Should_PropagateError_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Validation("INVALID", "Invalid input");
        var task = Task.FromResult(Result<string>.Failure(error));

        // Act
        var result = await task.TryAsync(x => int.Parse(x));

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public async Task TryAsync_TaskResult_AsyncOperation_Should_TransformValue_When_OperationSucceeds()
    {
        // Arrange
        var task = Task.FromResult(Result<string>.Success("42"));

        // Act
        var result = await task.TryAsync(async x =>
        {
            await Task.Delay(1);
            return int.Parse(x);
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task TryAsync_TaskResult_AsyncOperation_Should_ReturnFailure_When_OperationThrows()
    {
        // Arrange
        var task = Task.FromResult(Result<string>.Success("not a number"));

        // Act
        var result = await task.TryAsync(async x =>
        {
            await Task.Delay(1);
            return int.Parse(x);
        });

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Unexpected);
        result.Error.Code.Should().Be("EXCEPTION");
    }

    [Fact]
    public void Try_Should_ChainWithOtherOperations_When_OperationSucceeds()
    {
        // Arrange
        var result = Result<string>.Success("21");

        // Act
        var final = result
            .Try(x => int.Parse(x))
            .Map(x => x * 2)
            .Match(
                success => $"Success: {success}",
                error => $"Error: {error.Message}"
            );

        // Assert
        final.Should().Be("Success: 42");
    }

    [Fact]
    public void Try_Should_PropagateErrorInChain_When_OperationThrows()
    {
        // Arrange
        var result = Result<string>.Success("not a number");

        // Act
        var final = result
            .Try(x => int.Parse(x))
            .Map(x => x * 2)
            .Match(
                success => $"Success: {success}",
                error => $"Error: {error.Code}"
            );

        // Assert
        final.Should().Be("Error: EXCEPTION");
    }

    [Fact]
    public async Task TryAsync_Should_ChainWithOtherOperations_When_OperationSucceeds()
    {
        // Arrange
        var result = Result<string>.Success("21");

        // Act
        var final = await result
            .TryAsync(async x =>
            {
                await Task.Delay(1);
                return int.Parse(x);
            })
            .MapAsync(x => x * 2)
            .MatchAsync(
                success => $"Success: {success}",
                error => $"Error: {error.Message}"
            );

        // Assert
        final.Should().Be("Success: 42");
    }
}
