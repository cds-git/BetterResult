namespace BetterResult.Tests;

public class MapErrorTests
{
    [Fact]
    public void MapError_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(99);

        // Act
        var recovered = result.MapError(err => Result<int>.Failure(Error.Unexpected()));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public void MapError_Should_TransformOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var original = Error.Failure("E5", "bad");
        var result = Result<int>.Failure(original);

        // Act
        var recovered = result.MapError(err => Result<int>.Success(42));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(42);
    }

    [Fact]
    public async Task MapErrorAsync_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(7);

        // Act
        var recovered = await result.MapErrorAsync(err => Task.FromResult(Result<int>.Failure(Error.Unexpected())));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(7);
    }

    [Fact]
    public async Task MapErrorAsync_Should_TransformOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var original = Error.Failure("E6", "oops");
        var result = Result<int>.Failure(original);

        // Act
        var recovered = await result.MapErrorAsync(err => Task.FromResult(Result<int>.Success(123)));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(123);
    }

    [Fact]
    public void MapError_ErrorToError_Should_TransformError_OnFailure()
    {
        var original = Error.Failure("E", "msg");
        var result = Result<int>.Failure(original);

        var mapped = result.MapError(err => err.WithMessage("ctx"));

        mapped.IsFailure.Should().BeTrue();
        mapped.Error.Message.Should().Be("ctx: msg");
        mapped.Error.Code.Should().Be("E");
    }

    [Fact]
    public void MapError_ErrorToError_Should_PreserveSuccess()
    {
        var result = Result<int>.Success(42);

        var mapped = result.MapError(err => err.WithMessage("ctx"));

        mapped.IsSuccess.Should().BeTrue();
        mapped.Value.Should().Be(42);
    }

    [Fact]
    public async Task MapErrorAsync_ErrorToError_Should_TransformError_OnFailure()
    {
        var original = Error.Failure("E", "msg");
        var result = Result<int>.Failure(original);

        var mapped = await result.MapErrorAsync(err => Task.FromResult(err.WithMessage("ctx")));

        mapped.IsFailure.Should().BeTrue();
        mapped.Error.Message.Should().Be("ctx: msg");
    }

    [Fact]
    public async Task MapErrorAsync_TaskResult_ErrorToError_Should_TransformError()
    {
        var task = Task.FromResult(Result<int>.Failure(Error.Failure("E", "msg")));

        var mapped = await task.MapErrorAsync(err => err.WithMessage("ctx"));

        mapped.IsFailure.Should().BeTrue();
        mapped.Error.Message.Should().Be("ctx: msg");
    }

    [Fact]
    public async Task MapErrorAsync_TaskResult_AsyncErrorToError_Should_TransformError()
    {
        var task = Task.FromResult(Result<int>.Failure(Error.Failure("E", "msg")));

        var mapped = await task.MapErrorAsync(err => Task.FromResult(err.WithMessage("ctx")));

        mapped.IsFailure.Should().BeTrue();
        mapped.Error.Message.Should().Be("ctx: msg");
    }
}