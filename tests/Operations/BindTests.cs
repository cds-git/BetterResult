namespace BetterResult.Tests;

public class BindTests
{
    [Fact]
    public void Bind_Should_Chain_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(3);

        // Act
        var r2 = result.Bind(x => Result<string>.Success($"n={x}"));

        // Assert
        r2.IsSuccess.Should().BeTrue();
        r2.Value.Should().Be("n=3");
    }

    [Fact]
    public void Bind_Should_PreserveError_When_ResultIsFailure()
    {
        // Arrange
        var original = Error.Validation("E9", "bad input");
        var result = Result<int>.Failure(original);

        // Act
        var r2 = result.Bind(x => Result<string>.Success($"n={x}"));

        // Assert
        r2.IsFailure.Should().BeTrue();
        r2.Error.Should().Be(original);
    }

    [Fact]
    public async Task BindAsync_Should_Chain_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(4);

        // Act
        var r2 = await result.BindAsync(x => Task.FromResult(Result<double>.Success(x / 2.0)));

        // Assert
        r2.IsSuccess.Should().BeTrue();
        r2.Value.Should().BeApproximately(2.0, 0.0001);
    }

    [Fact]
    public async Task BindAsync_Should_PreserveError_When_ResultIsFailure()
    {
        // Arrange
        var original = Error.Failure("E10", "fail");
        var result = Result<int>.Failure(original);

        // Act
        var r2 = await result.BindAsync(x => Task.FromResult(Result<string>.Success("won’t run")));

        // Assert
        r2.IsFailure.Should().BeTrue();
        r2.Error.Should().Be(original);
    }

    [Fact]
    public void BindWithSelector_Should_PassBothValues_When_BothSucceed()
    {
        Result<int> user = 7;

        var summary = user.Bind(id => Result<string>.Success($"cart{id}"), (id, cart) => $"{id}:{cart}");

        summary.Value.Should().Be("7:cart7");
    }

    [Fact]
    public void BindWithSelector_Should_ReturnBindError_When_BindFails()
    {
        var error = Error.NotFound("NO_CART", "No cart");
        Result<int> user = 7;
        var selectorCalled = false;

        var summary = user.Bind(_ => Result<string>.Failure(error), (id, cart) =>
        {
            selectorCalled = true;
            return $"{id}:{cart}";
        });

        summary.Error.Should().Be(error);
        selectorCalled.Should().BeFalse();
    }

    [Fact]
    public void BindWithSelector_Should_NotRunBind_When_ResultIsFailure()
    {
        var error = Error.NotFound("NO_USER", "No user");
        Result<int> user = error;
        var bindCalled = false;

        var summary = user.Bind(id =>
        {
            bindCalled = true;
            return Result<string>.Success($"cart{id}");
        }, (id, cart) => $"{id}:{cart}");

        summary.Error.Should().Be(error);
        bindCalled.Should().BeFalse();
    }

    [Fact]
    public async Task BindAsyncWithSelector_Should_PassBothValues_When_BindIsAsync()
    {
        Result<int> user = 7;

        var summary = await user.BindAsync(
            id => Task.FromResult(Result<string>.Success($"cart{id}")),
            (id, cart) => $"{id}:{cart}");

        summary.Value.Should().Be("7:cart7");
    }

    [Fact]
    public async Task BindAsyncWithSelector_Should_PassBothValues_When_SourceIsTaskAndBindIsSync()
    {
        var user = Task.FromResult(Result<int>.Success(7));

        var summary = await user.BindAsync(id => Result<string>.Success($"cart{id}"), (id, cart) => $"{id}:{cart}");

        summary.Value.Should().Be("7:cart7");
    }

    [Fact]
    public async Task BindAsyncWithSelector_Should_PassBothValues_When_SourceIsTaskAndBindIsAsync()
    {
        var user = Task.FromResult(Result<int>.Success(7));

        var summary = await user.BindAsync(
            async id =>
            {
                await Task.Yield();
                return Result<string>.Success($"cart{id}");
            },
            (id, cart) => $"{id}:{cart}");

        summary.Value.Should().Be("7:cart7");
    }

    [Fact]
    public async Task BindAsyncWithSelector_Should_ReturnBindError_When_AsyncBindFails()
    {
        var error = Error.NotFound("NO_CART", "No cart");
        Result<int> user = 7;

        var summary = await user.BindAsync(
            _ => Task.FromResult(Result<string>.Failure(error)),
            (id, cart) => $"{id}:{cart}");

        summary.Error.Should().Be(error);
    }
}
