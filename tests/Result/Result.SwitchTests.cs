namespace Tests;

public class SwitchTests
{
    [Fact]
    public void Switch_Should_ExecuteOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = (Result)true;
        var successCalled = false;
        var failureCalled = false;

        // Act
        result.Switch(() => successCalled = true, _ => failureCalled = true);

        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
    }

    [Fact]
    public void Switch_Should_ExecuteOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Some error");
        var result = (Result)error;
        var successCalled = false;
        var failureCalled = false;

        // Act
        result.Switch(() => successCalled = true, _ => failureCalled = true);

        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
    }

    [Fact]
    public async Task SwitchAsync_Should_ExecuteOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = (Result)true;
        var successCalled = false;
        var failureCalled = false;

        // Act
        await result.SwitchAsync(() => { successCalled = true; return Task.CompletedTask; }, _ => { failureCalled = true; return Task.CompletedTask; });

        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
    }

    [Fact]
    public async Task SwitchAsync_Should_ExecuteOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Some error");
        var result = (Result)error;
        var successCalled = false;
        var failureCalled = false;

        // Act
        await result.SwitchAsync(() => { successCalled = true; return Task.CompletedTask; }, _ => { failureCalled = true; return Task.CompletedTask; });

        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
    }
}
