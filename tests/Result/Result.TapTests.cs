namespace Tests;

public class TapTests
{
    [Fact]
    public void Tap_Should_ExecuteAction_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();
        var actionExecuted = false;

        // Act
        result.Tap(() => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public void Tap_Should_NotExecuteAction_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Unexpected("E001", "Failure message");
        var result = Result.Failure(error);
        var actionExecuted = false;

        // Act
        result.Tap(() => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeFalse();
    }

    [Fact]
    public async Task TapAsync_Should_ExecuteAction_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();
        var actionExecuted = false;

        // Act
        await result.TapAsync(() =>
        {
            actionExecuted = true;
            return Task.CompletedTask;
        });

        // Assert
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public async Task TapAsync_Should_NotExecuteAction_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Failure message");
        var result = Result.Failure(error);
        var actionExecuted = false;

        // Act
        await result.TapAsync(() =>
        {
            actionExecuted = true;
            return Task.CompletedTask;
        });

        // Assert
        actionExecuted.Should().BeFalse();
    }

    [Fact]
    public void Tap_WithValue_Should_ExecuteAction_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var actionExecuted = false;

        // Act
        result.Tap(value => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public void Tap_WithValue_Should_NotExecuteAction_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Failure message");
        var result = Result<int>.Failure(error);
        var actionExecuted = false;

        // Act
        result.Tap(value => actionExecuted = true);

        // Assert
        actionExecuted.Should().BeFalse();
    }

    [Fact]
    public async Task TapAsync_WithValue_Should_ExecuteAction_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var actionExecuted = false;

        // Act
        await result.TapAsync(value =>
        {
            actionExecuted = true;
            return Task.CompletedTask;
        });

        // Assert
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public async Task TapAsync_WithValue_Should_NotExecuteAction_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Failure message");
        var result = Result<int>.Failure(error);
        var actionExecuted = false;

        // Act
        await result.TapAsync(value =>
        {
            actionExecuted = true;
            return Task.CompletedTask;
        });

        // Assert
        actionExecuted.Should().BeFalse();
    }
}