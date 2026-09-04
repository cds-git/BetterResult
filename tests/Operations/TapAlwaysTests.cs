namespace BetterResult.Tests;

public class TapAlwaysTests
{
    private static readonly Error TestError = Error.Failure("E001", "boom");

    [Fact]
    public void TapAlways_Should_ExposeSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);
        Result<int>? seen = null;

        // Act
        var r2 = result.TapAlways(r => seen = r);

        // Assert
        seen.Should().BeSameAs(result);
        seen!.IsSuccess.Should().BeTrue();
        seen.Value.Should().Be(42);
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public void TapAlways_Should_ExposeError_When_ResultIsFailure()
    {
        // Arrange
        var result = Result<int>.Failure(TestError);
        Result<int>? seen = null;

        // Act
        var r2 = result.TapAlways(r => seen = r);

        // Assert
        seen.Should().BeSameAs(result);
        seen!.IsFailure.Should().BeTrue();
        seen.Error.Should().Be(TestError);
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task TapAlwaysAsync_Should_ExposeSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);
        Result<int>? seen = null;

        // Act
        var r2 = await result.TapAlwaysAsync(async r =>
        {
            await Task.Yield();
            seen = r;
        });

        // Assert
        seen.Should().BeSameAs(result);
        seen!.Value.Should().Be(42);
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task TapAlwaysAsync_Should_ExposeError_When_ResultIsFailure()
    {
        // Arrange
        var result = Result<int>.Failure(TestError);
        Result<int>? seen = null;

        // Act
        var r2 = await result.TapAlwaysAsync(async r =>
        {
            await Task.Yield();
            seen = r;
        });

        // Assert
        seen.Should().BeSameAs(result);
        seen!.Error.Should().Be(TestError);
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task TapAlwaysAsync_OnTask_Should_ExposeSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);
        Result<int>? seen = null;

        // Act
        var r2 = await Task.FromResult(result).TapAlwaysAsync(r => seen = r);

        // Assert
        seen.Should().BeSameAs(result);
        seen!.Value.Should().Be(42);
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task TapAlwaysAsync_OnTask_Should_ExposeError_When_ResultIsFailure()
    {
        // Arrange
        var result = Result<int>.Failure(TestError);
        Result<int>? seen = null;

        // Act
        var r2 = await Task.FromResult(result).TapAlwaysAsync(r => seen = r);

        // Assert
        seen.Should().BeSameAs(result);
        seen!.Error.Should().Be(TestError);
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task TapAlwaysAsync_OnTask_Should_ExposeSuccess_When_ActionIsAsync()
    {
        // Arrange
        var result = Result<int>.Success(42);
        Result<int>? seen = null;

        // Act
        var r2 = await Task.FromResult(result).TapAlwaysAsync(async r =>
        {
            await Task.Yield();
            seen = r;
        });

        // Assert
        seen.Should().BeSameAs(result);
        seen!.Value.Should().Be(42);
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task TapAlwaysAsync_OnTask_Should_ExposeError_When_ActionIsAsync()
    {
        // Arrange
        var result = Result<int>.Failure(TestError);
        Result<int>? seen = null;

        // Act
        var r2 = await Task.FromResult(result).TapAlwaysAsync(async r =>
        {
            await Task.Yield();
            seen = r;
        });

        // Assert
        seen.Should().BeSameAs(result);
        seen!.Error.Should().Be(TestError);
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task TapAlwaysAsync_Should_AwaitAction_Before_ReturningResult()
    {
        // A completion source the action blocks on: if the implementation forgot to await,
        // the returned task would already be complete before the gate is released.
        // Arrange
        var gate = new TaskCompletionSource<bool>();
        var completed = false;
        var result = Result<int>.Success(1);

        // Act
        var pending = result.TapAlwaysAsync(async _ =>
        {
            await gate.Task.ConfigureAwait(false);
            completed = true;
        });

        // Assert
        pending.IsCompleted.Should().BeFalse();
        gate.SetResult(true);
        await pending;
        completed.Should().BeTrue();
    }

    [Fact]
    public void TapAlways_Should_PropagateException_When_ActionThrowsOnSuccess()
    {
        // Arrange
        var result = Result<int>.Success(1);

        // Act
        var act = () => result.TapAlways(_ => throw new InvalidOperationException("from action"));

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("from action");
    }

    [Fact]
    public void TapAlways_Should_PropagateException_And_DiscardError_When_ActionThrowsOnFailure()
    {
        // A throwing action replaces a handled Error with an exception. Pinned so the
        // documented trade-off stays visible.
        // Arrange
        var result = Result<int>.Failure(TestError);

        // Act
        var act = () => result.TapAlways(_ => throw new InvalidOperationException("from action"));

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("from action");
    }

    [Fact]
    public async Task TapAlwaysAsync_Should_PropagateException_When_ActionThrows()
    {
        // Arrange
        var result = Result<int>.Failure(TestError);

        // Act
        var act = async () => await result.TapAlwaysAsync(async _ =>
        {
            await Task.Yield();
            throw new InvalidOperationException("from action");
        });

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("from action");
    }

    [Fact]
    public async Task TapAlwaysAsync_OnTask_Should_NotRunAction_When_UpstreamTaskFaults()
    {
        // The action runs only once a Result<T> exists. A faulted upstream task unwinds
        // past it, exactly as a throwing synchronous step does.
        // Arrange
        var invoked = 0;
        var faulted = Task.FromException<Result<int>>(new InvalidOperationException("upstream"));

        // Act
        var act = async () => await faulted.TapAlwaysAsync(_ => invoked++);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("upstream");
        invoked.Should().Be(0);
    }

    [Fact]
    public async Task TapAlwaysAsync_OnTask_Should_NotRunAsyncAction_When_UpstreamTaskFaults()
    {
        // Arrange
        var invoked = 0;
        var faulted = Task.FromException<Result<int>>(new InvalidOperationException("upstream"));

        // Act
        var act = async () => await faulted.TapAlwaysAsync(async _ =>
        {
            await Task.Yield();
            invoked++;
        });

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("upstream");
        invoked.Should().Be(0);
    }

    [Fact]
    public void TapAlways_Should_RunOnEachBranch_When_UsedInPipeline()
    {
        // Arrange
        var successCompletions = 0;
        var failureCompletions = 0;

        // Act
        var success = Result<int>.Success(1)
            .Bind(x => Result<int>.Success(x + 1))
            .TapAlways(_ => successCompletions++);

        var failure = Result<int>.Success(1)
            .Bind(_ => Result<int>.Failure(TestError))
            .TapAlways(_ => failureCompletions++);

        // Assert
        successCompletions.Should().Be(1);
        failureCompletions.Should().Be(1);
        success.IsSuccess.Should().BeTrue();
        failure.IsFailure.Should().BeTrue();
    }
}
