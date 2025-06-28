namespace Tests;

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
    public void NonGeneric_MapError_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var r2 = result.MapError(err => Result.Failure(Error.Unexpected()));

        // Assert
        r2.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void NonGeneric_MapError_Should_TransformOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var original = Error.Failure("E7", "err");
        var result = Result.Failure(original);

        // Act
        var r2 = result.MapError(err => Result.Success());

        // Assert
        r2.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task NonGeneric_MapErrorAsync_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var r2 = await result.MapErrorAsync(err => Task.FromResult(Result.Failure(Error.Unexpected())));

        // Assert
        r2.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task NonGeneric_MapErrorAsync_Should_TransformOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var original = Error.Failure("E8", "err");
        var result = Result.Failure(original);

        // Act
        var r2 = await result.MapErrorAsync(err => Task.FromResult(Result.Success()));

        // Assert
        r2.IsSuccess.Should().BeTrue();
    }
}
