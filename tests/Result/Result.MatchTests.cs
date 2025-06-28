namespace Tests;

public class MatchTests
{
    [Fact]
    public void Match_Should_ExecuteOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var output = result.Match(
            onSuccess: () => "Success",
            onFailure: error => $"Error: {error.Message}"
        );

        // Assert
        output.Should().Be("Success");
    }

    [Fact]
    public void Match_Should_ExecuteOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Validation("E001", "Some error");
        var result = Result.Failure(error);

        // Act
        var output = result.Match(
            onSuccess: () => "Success",
            onFailure: err => $"Error: {err.Message}"
        );

        // Assert
        output.Should().Be($"Error: {error.Message}");
    }

    [Fact]
    public async Task MatchAsync_Should_ExecuteOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();

        // Act
        var output = await result.MatchAsync(
            onSuccess: () => Task.FromResult("Success"),
            onFailure: error => Task.FromResult($"Error: {error.Message}")
        );

        // Assert
        output.Should().Be("Success");
    }

    [Fact]
    public async Task MatchAsync_Should_ExecuteOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Validation("E001", "Some error");
        var result = Result.Failure(error);

        // Act
        var output = await result.MatchAsync(
            onSuccess: () => Task.FromResult("Success"),
            onFailure: err => Task.FromResult($"Error: {err.Message}")
        );

        // Assert
        output.Should().Be($"Error: {error.Message}");
    }

    [Fact]
    public void Match_WithValue_Should_ExecuteOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var output = result.Match(
            onSuccess: value => $"Value is {value}",
            onFailure: error => $"Error: {error.Message}"
        );

        // Assert
        output.Should().Be("Value is 42");
    }

    [Fact]
    public void Match_WithValue_Should_ExecuteOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Validation("E002", "Value error");
        var result = Result<int>.Failure(error);

        // Act
        var output = result.Match(
            onSuccess: value => $"Value is {value}",
            onFailure: err => $"Error: {err.Message}"
        );

        // Assert
        output.Should().Be($"Error: {error.Message}");
    }

    [Fact]
    public async Task MatchAsync_WithValue_Should_ExecuteOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var output = await result.MatchAsync(
            onSuccess: value => Task.FromResult($"Value is {value}"),
            onFailure: error => Task.FromResult("Failure")
        );

        // Assert
        output.Should().Be("Value is 42");
    }

    [Fact]
    public async Task MatchAsync_WithValue_Should_ExecuteOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Validation("E002", "Value error");
        var result = Result<int>.Failure(error);

        // Act
        var output = await result.MatchAsync(
            onSuccess: value => Task.FromResult($"Value is {value}"),
            onFailure: err => Task.FromResult($"Error: {err.Message}")
        );

        // Assert
        output.Should().Be($"Error: {error.Message}");
    }
}