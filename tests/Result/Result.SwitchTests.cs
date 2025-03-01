namespace Tests;

public class SwitchTests
{
    [Fact]
    public void Switch_Should_ExecuteOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();
        var successCalled = false;
        var failureCalled = false;

        // Act
        result.Switch(
            onSuccess: () => successCalled = true,
            onFailure: _ => failureCalled = true
        );

        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
    }

    [Fact]
    public void Switch_Should_ExecuteOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Some error");
        var result = Result.Failure(error);
        var successCalled = false;
        var failureCalled = false;

        // Act
        result.Switch(
            onSuccess: () => successCalled = true,
            onFailure: _ => failureCalled = true
        );

        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
    }

    [Fact]
    public async Task SwitchAsync_Should_ExecuteOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result.Success();
        var successCalled = false;
        var failureCalled = false;

        // Act
        await result.SwitchAsync(
            onSuccess: () => { successCalled = true; return Task.CompletedTask; },
            onFailure: _ => { failureCalled = true; return Task.CompletedTask; }
        );

        // Assert
        successCalled.Should().BeTrue();
        failureCalled.Should().BeFalse();
    }

    [Fact]
    public async Task SwitchAsync_Should_ExecuteOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E001", "Some error");
        var result = Result.Failure(error);
        var successCalled = false;
        var failureCalled = false;

        // Act
        await result.SwitchAsync(
            onSuccess: () => { successCalled = true; return Task.CompletedTask; },
            onFailure: _ => { failureCalled = true; return Task.CompletedTask; }
        );

        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
    }

    [Fact]
    public void Switch_WithValue_Should_ExecuteOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var successCalled = false;
        var capturedValue = 0;
        var failureCalled = false;

        // Act
        result.Switch(
            onSuccess: value => { successCalled = true; capturedValue = value; },
            onFailure: _ => failureCalled = true
        );

        // Assert
        successCalled.Should().BeTrue();
        capturedValue.Should().Be(42);
        failureCalled.Should().BeFalse();
    }

    [Fact]
    public void Switch_WithValue_Should_ExecuteOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E002", "Value error");
        var result = Result<int>.Failure(error);
        var successCalled = false;
        Error? capturedError = null;

        // Act
        result.Switch(
            onSuccess: value => successCalled = true,
            onFailure: err => capturedError = err
        );

        // Assert
        successCalled.Should().BeFalse();
        capturedError.Should().Be(error);
    }

    [Fact]
    public async Task SwitchAsync_WithValue_Should_ExecuteOnSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var successCalled = false;
        var capturedValue = 0;
        var failureCalled = false;

        // Act
        await result.SwitchAsync(
            onSuccess: value => { successCalled = true; capturedValue = value; return Task.CompletedTask; },
            onFailure: _ => { failureCalled = true; return Task.CompletedTask; }
        );

        // Assert
        successCalled.Should().BeTrue();
        capturedValue.Should().Be(42);
        failureCalled.Should().BeFalse();
    }

    [Fact]
    public async Task SwitchAsync_WithValue_Should_ExecuteOnFailure_When_ResultIsFailure()
    {
        // Arrange
        var error = Error.Failure("E002", "Value error");
        var result = Result<int>.Failure(error);
        var successCalled = false;
        var failureCalled = false;
        Error? capturedError = null;

        // Act
        await result.SwitchAsync(
            onSuccess: value => { successCalled = true; return Task.CompletedTask; },
            onFailure: err => { failureCalled = true; capturedError = err; return Task.CompletedTask; }
        );

        // Assert
        successCalled.Should().BeFalse();
        failureCalled.Should().BeTrue();
        capturedError.Should().Be(error);
    }
}