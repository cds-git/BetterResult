namespace Tests;

public class DoTests
{
    [Fact]
    public void Do_Should_InvokeOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(8);
        var seen = 0;

        // Act
        var r2 = result.Do(x => seen = x);

        // Assert
        seen.Should().Be(8);
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public void Do_Should_InvokeOnError_When_ResultIsFailure()
    {
        // Arrange
        var original = Error.Validation("E13", "fail");
        var result = Result<int>.Failure(original);
        Error? seen = null;

        // Act
        var r2 = result.Do(err => seen = err);

        // Assert
        seen.Should().Be(original);
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task DoAsync_Should_InvokeOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();
        var flag = false;

        // Act
        var r2 = await result.DoAsync(() => Task.Run(() => flag = true));

        // Assert
        flag.Should().BeTrue();
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task DoAsync_Should_InvokeOnError_When_ResultIsFailure()
    {
        // Arrange
        var original = Error.Failure("E14", "err");
        var result = Result.Failure(original);
        Error? seen = null;

        // Act
        var r2 = await result.DoAsync(err => Task.Run(() => seen = err));

        // Assert
        seen.Should().Be(original);
        r2.Should().BeSameAs(result);
    }
}