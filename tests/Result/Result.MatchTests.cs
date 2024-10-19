namespace Tests;

public class MatchTests
{
    [Fact]
    public void Match_Should_ExecuteOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = (Result)true;

        // Act
        var value = result.Match(() => "Success", _ => "Failure");

        // Assert
        value.Should().Be("Success");
    }

    [Fact]
    public void Match_Should_ExecuteOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Some error");
        var result = (Result)error;

        // Act
        var value = result.Match(() => "Success", _ => "Failure");

        // Assert
        value.Should().Be("Failure");
    }

    [Fact]
    public async Task MatchAsync_Should_ExecuteOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = (Result)true;

        // Act
        var value = await result.MatchAsync(() => Task.FromResult("Success"), _ => Task.FromResult("Failure"));

        // Assert
        value.Should().Be("Success");
    }

    [Fact]
    public async Task MatchAsync_Should_ExecuteOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Some error");
        var result = (Result)error;

        // Act
        var value = await result.MatchAsync(() => Task.FromResult("Success"), _ => Task.FromResult("Failure"));

        // Assert
        value.Should().Be("Failure");
    }
}
