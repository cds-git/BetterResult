namespace BetterResult.Tests;

public class MatchTests
{
    // Generic Match<TValue, TResult> - sync

    [Fact]
    public void Match_Success_InvokesMapValue()
    {
        var result = Result<int>.Success(5);

        var output = result.Match(
            value => value * 2,
            error => -1);

        output.Should().Be(10);
    }

    [Fact]
    public void Match_Nullable_Test()
    {
        var result = ResultWithNullable(null);

        var output = result.Match(
            value => value is null ? 0 : value * 2,
            error => 1);

        output.Should().Be(0);
    }

    [Fact]
    public void Match_Nullable_Test2()
    {
        var result = ResultWithNullable(5);

        var output = result.Match(
            value => value is null ? 0 : value * 2,
            error => 1);

        output.Should().Be(10);
    }

    private static Result<int?> ResultWithNullable(int? value) => value is null ? null : value;

    [Fact]
    public void Match_Failure_InvokesMapError()
    {
        var err = Error.Validation("E1", "bad");
        var result = Result<int>.Failure(err);

        var output = result.Match(
            value => value * 2,
            error => error.Code == "E1" ? 99 : 0);

        output.Should().Be(99);
    }

    // Generic MatchAsync on Task<Result<TValue>>

    [Fact]
    public async Task MatchAsync_TaskResult_SyncMappers()
    {
        var resultTask = Task.FromResult(Result<int>.Success(7));

        var output = await resultTask.MatchAsync(
            v => v + 1,
            err => -1);

        output.Should().Be(8);
    }

    [Fact]
    public async Task MatchAsync_TaskResult_SyncMappers_OnFailure()
    {
        var err = Error.Failure("E2", "fail");
        var resultTask = Task.FromResult(Result<int>.Failure(err));

        var output = await resultTask.MatchAsync(
            v => v + 1,
            error => error.Code == "E2" ? -2 : 0);

        output.Should().Be(-2);
    }

    // Generic MatchAsync on Result<TValue> with async mapValue

    [Fact]
    public async Task MatchAsync_Result_AsyncValueMapper()
    {
        var result = Result<int>.Success(3);

        var output = await result.MatchAsync(
            async v =>
            {
                await Task.Delay(1);
                return v * 3;
            },
            err => -1);

        output.Should().Be(9);
    }

    [Fact]
    public async Task MatchAsync_Result_AsyncValueMapper_OnFailure()
    {
        var err = Error.Unexpected("E3", "oops");
        var result = Result<int>.Failure(err);

        var output = await result.MatchAsync(
            async v => await Task.FromResult(v * 3),
            error => -3);

        output.Should().Be(-3);
    }

    // Generic MatchAsync on Result<TValue> with async error mapper

    [Fact]
    public async Task MatchAsync_Result_AsyncErrorMapper_OnFailure()
    {
        var err = Error.Conflict("E4", "conflict");
        var result = Result<int>.Failure(err);

        var output = await result.MatchAsync(
            v => v + 5,
            async error =>
            {
                await Task.Delay(1);
                return error.Code.Length;
            });

        output.Should().Be(err.Code.Length);
    }

    // Generic MatchAsync with both async

    [Fact]
    public async Task MatchAsync_Result_BothAsync()
    {
        var result = Result<int>.Success(4);

        var output = await result.MatchAsync(
            async v =>
            {
                await Task.Delay(1);
                return v - 1;
            },
            async error =>
            {
                await Task.Delay(1);
                return -99;
            });

        output.Should().Be(3);
    }

    // Non-generic Match<TResult> - sync

    [Fact]
    public void NonGeneric_Match_Success()
    {
        var result = Result.Success();

        var output = result.Match(
            () => "OK",
            err => "ERR");

        output.Should().Be("OK");
    }

    [Fact]
    public void NonGeneric_Match_Failure()
    {
        var err = Error.Timeout("E5", "timeout");
        var result = Result.Failure(err);

        var output = result.Match(
            () => "OK",
            error => error.Message);

        output.Should().Be("timeout");
    }

    // Non-generic MatchAsync

    [Fact]
    public async Task NonGeneric_MatchAsync_TaskResult_SyncMappers()
    {
        var resultTask = Task.FromResult(Result.Success());

        var output = await resultTask.MatchAsync(
            () => 42,
            err => -1);

        output.Should().Be(42);
    }

    [Fact]
    public async Task NonGeneric_MatchAsync_Result_AsyncValueMapper()
    {
        var result = Result.Success();

        var output = await result.MatchAsync(
            async () =>
            {
                await Task.Delay(1);
                return "done";
            },
            err => "fail");

        output.Should().Be("done");
    }

    [Fact]
    public async Task NonGeneric_MatchAsync_Result_AsyncErrorMapper()
    {
        var err = Error.Unavailable("E6", "down");
        var result = Result.Failure(err);

        var output = await result.MatchAsync(
            () => "OK",
            async error =>
            {
                await Task.Delay(1);
                return error.Code;
            });

        output.Should().Be("E6");
    }
}