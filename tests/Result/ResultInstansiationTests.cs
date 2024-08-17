namespace Tests;

public class ResultInstansiationTests
{
    public record TestUser(string Name);

    [Fact]
    public void CreateFromValue_WhenAccessingValue_ShouldReturnValue()
    {
        // Arrange
        var value = new TestUser("Abe");

        // Act
        var result = Result.Success(value);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<TestUser>();
    }
}