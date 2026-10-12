namespace BetterResult.Tests.Operations;

public class SuccessIfTests
{
    private static Error Rejected => Error.Validation("REJECTED", "Rejected");

    [Fact]
    public void SuccessIf_Should_ReturnValue_When_ConditionIsTrue()
    {
        var result = Result.SuccessIf(true, 42, Rejected);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void SuccessIf_Should_ReturnError_When_ConditionIsFalse()
    {
        var result = Result.SuccessIf(false, 42, Rejected);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("REJECTED");
    }

    [Fact]
    public void SuccessIf_Should_ReturnNoValue_When_ConditionIsTrueWithoutValue()
    {
        Result<NoValue> result = Result.SuccessIf(true, Rejected);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void SuccessIf_Should_ReturnError_When_ConditionIsFalseWithoutValue()
    {
        var result = Result.SuccessIf(false, Rejected);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("REJECTED");
    }

    [Fact]
    public void FailureIf_Should_ReturnError_When_ConditionIsTrue()
    {
        var result = Result.FailureIf(true, 42, Rejected);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("REJECTED");
    }

    [Fact]
    public void FailureIf_Should_ReturnValue_When_ConditionIsFalse()
    {
        var result = Result.FailureIf(false, 42, Rejected);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void FailureIf_Should_ReturnError_When_ConditionIsTrueWithoutValue()
    {
        var result = Result.FailureIf(true, Rejected);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("REJECTED");
    }

    [Fact]
    public void FailureIf_Should_ReturnNoValue_When_ConditionIsFalseWithoutValue()
    {
        Result<NoValue> result = Result.FailureIf(false, Rejected);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void SuccessIf_Should_Throw_When_ErrorIsDefaultAndConditionIsFalse()
    {
        var act = () => Result.SuccessIf(false, 42, default);

        act.Should().Throw<ArgumentException>();
    }
}
