namespace Tests;

public class BindTests
{
    [Fact]
    public void Bind_Should_Chain_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(3);

        // Act
        var bound = result.Bind(x => Result<string>.Success($"n={x}"));

        // Assert
        bound.IsSuccess.Should().BeTrue();
        bound.Value.Should().Be("n=3");
    }

    [Fact]
    public void Bind_Should_PreserveError_When_ResultIsFailure()
    {
        // Arrange
        var original = Error.Validation("E9", "bad input");
        var result = Result<int>.Failure(original);

        // Act
        var bound = result.Bind(x => Result<string>.Success($"n={x}"));

        // Assert
        bound.IsFailure.Should().BeTrue();
        bound.Error.Should().Be(original);
    }

    [Fact]
    public async Task BindAsync_Should_Chain_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(4);

        // Act
        var bound = await result.BindAsync(x => Task.FromResult(Result<double>.Success(x / 2.0)));

        // Assert
        bound.IsSuccess.Should().BeTrue();
        bound.Value.Should().BeApproximately(2.0, 0.0001);
    }

    [Fact]
    public async Task BindAsync_Should_PreserveError_When_ResultIsFailure()
    {
        // Arrange
        var original = Error.Failure("E10", "fail");
        var result = Result<int>.Failure(original);

        // Act
        var bound = await result.BindAsync(x => Task.FromResult(Result<string>.Success("won’t run")));

        // Assert
        bound.IsFailure.Should().BeTrue();
        bound.Error.Should().Be(original);
    }

    [Fact]
    public void NonGeneric_Bind_Should_Chain_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var r2 = result.Bind(() => Result.Failure(Error.Unexpected()));

        // Assert
        r2.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void NonGeneric_Bind_Should_PreserveError_When_ResultIsFailure()
    {
        // Arrange
        var original = Error.Failure("E11", "no");
        var result = Result.Failure(original);

        // Act
        var r2 = result.Bind(() => Result.Success());

        // Assert
        r2.IsFailure.Should().BeTrue();
        r2.Error.Should().Be(original);
    }

    [Fact]
    public async Task NonGeneric_BindAsync_Should_Chain_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var r2 = await result.BindAsync(() => Task.FromResult(Result.Success()));

        // Assert
        r2.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task NonGeneric_BindAsync_Should_PreserveError_When_ResultIsFailure()
    {
        // Arrange
        var original = Error.Unexpected("E12", "bad");
        var result = Result.Failure(original);

        // Act
        var r2 = await result.BindAsync(() => Task.FromResult(Result.Success()));

        // Assert
        r2.IsFailure.Should().BeTrue();
        r2.Error.Should().Be(original);
    }
}