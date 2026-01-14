namespace BetterResult.Tests;

public class MapTests
{
    [Fact]
    public void Map_Should_TransformValue_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(5);

        // Act
        var mapped = result.Map(x => x * 2);

        // Assert
        mapped.IsSuccess.Should().BeTrue();
        mapped.Value.Should().Be(10);
    }

    [Fact]
    public void Map_Should_PreserveError_When_ResultIsFailure()
    {
        // Arrange
        var err = Error.Validation("E1", "Validation failed");
        var result = Result<int>.Failure(err);

        // Act
        var mapped = result.Map(x => x * 2);

        // Assert
        mapped.IsFailure.Should().BeTrue();
        mapped.Error.Should().Be(err);
    }

    [Fact]
    public async Task MapAsync_Should_TransformValue_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(7);

        // Act
        var mapped = await result.MapAsync(x => Task.FromResult(x + 3));

        // Assert
        mapped.IsSuccess.Should().BeTrue();
        mapped.Value.Should().Be(10);
    }

    [Fact]
    public async Task MapAsync_Should_PreserveError_When_ResultIsFailure()
    {
        // Arrange
        var err = Error.Unexpected("E2", "Boom");
        var result = Result<int>.Failure(err);

        // Act
        var mapped = await result.MapAsync(x => Task.FromResult(x + 3));

        // Assert
        mapped.IsFailure.Should().BeTrue();
        mapped.Error.Should().Be(err);
    }

}