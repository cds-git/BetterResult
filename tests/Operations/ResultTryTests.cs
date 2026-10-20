namespace BetterResult.Tests.Operations;

public class ResultTryTests
{
    [Fact]
    public void Try_Should_ReturnValue_When_OperationSucceeds()
    {
        Result<int> result = Result.Try(() => int.Parse("42"));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Try_Should_ReturnUnexpectedError_When_OperationThrows()
    {
        var result = Result.Try(() => int.Parse("not a number"));

        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Unexpected);
        result.Error.Code.Should().Be("EXCEPTION");
        result.Error.GetMetadata<string>("ExceptionType").Should().Be("FormatException");
        result.Error.Metadata.Should().NotContainKey("StackTrace");
    }

    [Fact]
    public void Try_Should_UseMappedError_When_OperationThrowsWithErrorMapper()
    {
        var result = Result.Try(
            () => int.Parse("not a number"),
            _ => Error.Validation("NOT_A_NUMBER", "Not a number"));

        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Validation);
        result.Error.Code.Should().Be("NOT_A_NUMBER");
    }

    [Fact]
    public void Try_Should_UseDefaultError_When_ErrorMapperReturnsNull()
    {
        var result = Result.Try(() => int.Parse("not a number"), _ => null!);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("EXCEPTION");
    }

    [Fact]
    public void Try_Should_Rethrow_When_OperationIsCancelled()
    {
        var act = () => Result.Try<int>(() => throw new OperationCanceledException());

        act.Should().Throw<OperationCanceledException>();
    }

    [Fact]
    public void Try_Should_ReturnNoValue_When_ActionSucceeds()
    {
        var calls = 0;

        Result<NoValue> result = Result.Try(() => { calls++; });

        result.IsSuccess.Should().BeTrue();
        calls.Should().Be(1);
    }

    [Fact]
    public void Try_Should_ReturnUnexpectedError_When_ActionThrows()
    {
        var result = Result.Try(() => throw new InvalidOperationException("Boom"));

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("EXCEPTION");
        result.Error.GetMetadata<string>("ExceptionType").Should().Be("InvalidOperationException");
    }

    [Fact]
    public void Try_Should_UseMappedError_When_ActionThrowsWithErrorMapper()
    {
        var result = Result.Try(
            () => throw new InvalidOperationException("Boom"),
            _ => Error.Conflict("BOOM", "Boom"));

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("BOOM");
    }

    [Fact]
    public async Task TryAsync_Should_ReturnValue_When_OperationSucceeds()
    {
        Result<int> result = await Result.TryAsync(async () =>
        {
            await Task.Yield();
            return 42;
        });

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task TryAsync_Should_ReturnUnexpectedError_When_OperationThrows()
    {
        var result = await Result.TryAsync<int>(async () =>
        {
            await Task.Yield();
            throw new TimeoutException("Too slow");
        });

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("EXCEPTION");
        result.Error.GetMetadata<string>("ExceptionType").Should().Be("TimeoutException");
    }

    [Fact]
    public async Task TryAsync_Should_UseMappedError_When_OperationThrowsWithErrorMapper()
    {
        var result = await Result.TryAsync<int>(
            async () =>
            {
                await Task.Yield();
                throw new TimeoutException("Too slow");
            },
            _ => Error.Timeout("TOO_SLOW", "Too slow"));

        result.IsFailure.Should().BeTrue();
        result.Error.Type.Should().Be(ErrorType.Timeout);
        result.Error.Code.Should().Be("TOO_SLOW");
    }

    [Fact]
    public async Task TryAsync_Should_Rethrow_When_OperationIsCancelled()
    {
        var act = () => Result.TryAsync<int>(async () =>
        {
            await Task.Yield();
            throw new TaskCanceledException();
        });

        await act.Should().ThrowAsync<TaskCanceledException>();
    }

    [Fact]
    public async Task TryAsync_Should_ReturnNoValue_When_TaskSucceeds()
    {
        var calls = 0;

        Result<NoValue> result = await Result.TryAsync(async () =>
        {
            await Task.Yield();
            calls++;
        });

        result.IsSuccess.Should().BeTrue();
        calls.Should().Be(1);
    }

    [Fact]
    public async Task TryAsync_Should_UseMappedError_When_TaskThrowsWithErrorMapper()
    {
        var result = await Result.TryAsync(
            async () =>
            {
                await Task.Yield();
                throw new InvalidOperationException("Boom");
            },
            _ => Error.Conflict("BOOM", "Boom"));

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("BOOM");
    }

    [Fact]
    public void Try_Should_JoinPipeline_When_OperationSucceeds()
    {
        var result = Result.Try(() => int.Parse("21"))
            .Map(n => n * 2);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }
}
