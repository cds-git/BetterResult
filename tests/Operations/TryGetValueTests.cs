namespace BetterResult.Tests.Operations;

public class TryGetValueTests
{
    [Fact]
    public void TryGetValue_Should_ReturnTrueAndNonNullValue_When_ResultIsSuccess()
    {
        Result<string> result = "hello";

        var length = result.TryGetValue(out var value, out _) ? value.Length : -1;

        length.Should().Be(5);
    }

    [Fact]
    public void TryGetValue_Should_ReturnValueAndDefaultError_When_ResultIsSuccess()
    {
        Result<int> result = 42;

        var found = result.TryGetValue(out var value, out var error);

        found.Should().BeTrue();
        value.Should().Be(42);
        error.Should().BeNull();
    }

    [Fact]
    public void TryGetValue_Should_ReturnError_When_ResultIsFailure()
    {
        Result<int> result = Error.NotFound("MISSING", "Missing");

        var code = result.TryGetValue(out _, out var error) ? null : error.Code;

        code.Should().Be("MISSING");
    }
}
