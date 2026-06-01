namespace BetterResult.Tests;

public class MapErrorTests
{
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