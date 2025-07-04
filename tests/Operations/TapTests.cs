namespace BetterResult.Tests;

public class TapTests
{
    [Fact]
    public void Tap_Should_InvokeOnSuccess_When_ResultTvalueIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(8);
        var seen = 0;

        // Act
        var r2 = result.Tap(x => seen = x);

        // Assert
        seen.Should().Be(8);
        r2.Should().BeSameAs(result);
    }


    [Fact]
    public async Task TapAsync_Should_InvokeOnSuccess_When_ResultTvalueIsTaskAndSuccess()
    {
        // Arrange
        var result = Result.Success(42);
        var isMeaningOfLife = false;

        // Act
        var r2 = await result.TapAsync((value) => Task.Run(() =>
        {
            isMeaningOfLife = value is 42;
            Console.WriteLine($"Is the result the meaning of life? {isMeaningOfLife}");
        }));

        // Assert
        isMeaningOfLife.Should().BeTrue();
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task TapAsync_Should_InvokeOnSuccess_When_ResultIsTaskAndTvalueIsSuccess()
    {
        // Arrange
        var result = GetResultAsync(42);
        var isMeaningOfLife = false;

        // Act
        var r2 = await result.TapAsync((value) =>
        {
            isMeaningOfLife = value is 42;
            Console.WriteLine($"Is the result the meaning of life? {isMeaningOfLife}");
        });

        // Assert
        isMeaningOfLife.Should().BeTrue();
        r2.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task TapAsync_Should_InvokeOnSuccess_When_ResultIsTaskAndTvalueIsTaskAndSuccess()
    {
        // Arrange
        var result = GetResultAsync(42);
        var isMeaningOfLife = false;

        // Act
        var r2 = await result.TapAsync((value) => Task.Run(() =>
        {
            isMeaningOfLife = value is 42;
            Console.WriteLine($"Is the result the meaning of life? {isMeaningOfLife}");
        }));

        // Assert
        isMeaningOfLife.Should().BeTrue();
        r2.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Tap_Should_InvokeOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();
        var seen = false;

        // Act
        var r2 = result.Tap(() => seen = true);

        // Assert
        seen.Should().BeTrue();
        r2.Should().BeSameAs(result);
    }


    [Fact]
    public async Task TapAsync_Should_InvokeOnSuccess_When_ResultIsTaskAndSuccess()
    {
        // Arrange
        var result = Result.Success();
        var flag = false;

        // Act
        var r2 = await result.TapAsync(() => Task.Run(() =>
        {
            flag = true;
            Console.WriteLine($"Should this be enabled? {flag}");
        }));

        // Assert
        flag.Should().BeTrue();
        r2.Should().BeSameAs(result);
    }

    [Fact]
    public async Task TapAsync_Should_InvokeOnSuccess_When_ResultIsTaskIsSuccess()
    {
        // Arrange
        var result = GetResultAsync();
        var flag = false;

        // Act
        var r2 = await result.TapAsync(() =>
        {
            flag = true;
            Console.WriteLine($"Should this be enabled? {flag}");
        });

        // Assert
        flag.Should().BeTrue();
        r2.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task TapAsync_Should_InvokeOnSuccess_When_ResultIsTaskIsTaskAndSuccess()
    {
        // Arrange
        var result = GetResultAsync();
        var flag = false;

        // Act
        var r2 = await result.TapAsync(() => Task.Run(() =>
        {
            flag = true;
            Console.WriteLine($"Should this be enabled? {flag}");
        }));

        // Assert
        flag.Should().BeTrue();
        r2.IsSuccess.Should().BeTrue();
    }

    private static Task<Result<T>> GetResultAsync<T>(T value) => Task.FromResult(Result.Success(value));
    private static Task<Result> GetResultAsync() => Task.FromResult(Result.Success());
}