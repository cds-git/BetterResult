#if NET11_0_OR_GREATER
namespace BetterResult.Tests;

public class UnionTests
{
    private sealed record UserNotFound(int UserId)
        : Error(ErrorType.NotFound, "USER_NOT_FOUND", $"User {UserId} was not found");

    private static string Describe(Result<int> result) => result switch
    {
        int value => $"value {value}",
        Error error => $"error {error.Code}",
    };

    [Fact]
    public void Switch_Should_MatchValue_When_ResultIsSuccess()
    {
        Result<int> result = 42;

        var description = Describe(result);

        description.Should().Be("value 42");
    }

    [Fact]
    public void Switch_Should_MatchError_When_ResultIsFailure()
    {
        Result<int> result = Error.NotFound("MISSING", "Missing");

        var description = Describe(result);

        description.Should().Be("error MISSING");
    }

    [Fact]
    public void Switch_Should_MatchErrorSubtype_When_ArmComesBeforeError()
    {
        Result<string> result = new UserNotFound(7);

        var description = result switch
        {
            string name => name,
            UserNotFound e => $"no user {e.UserId}",
            Error e => e.Code,
        };

        description.Should().Be("no user 7");
    }

    [Fact]
    public void Switch_Should_MatchError_When_ResultIsDefault()
    {
        Result<int> result = default;

        var description = Describe(result);

        description.Should().Be("error General.Uninitialized");
    }

    [Fact]
    public void Switch_Should_MatchNoValue_When_ResultIsSuccess()
    {
        var result = Result.Success();

        var done = result switch
        {
            NoValue => true,
            Error => false,
        };

        done.Should().BeTrue();
    }

    [Fact]
    public void Switch_Should_MatchNull_When_SuccessHoldsNull()
    {
        Result<string?> result = (string?)null;

        var description = result switch
        {
            string text => text,
            Error e => e.Code,
            null => "empty",
        };

        description.Should().Be("empty");
    }

    [Fact]
    public void TypePattern_Should_ExtractValue_When_ResultIsSuccess()
    {
        Result<int> result = 5;

        var doubled = result is int value ? value * 2 : 0;

        doubled.Should().Be(10);
    }

    [Fact]
    public void TypePattern_Should_NotMatchError_When_ResultIsSuccess()
    {
        Result<int> result = 5;

        var isError = result is Error;

        isError.Should().BeFalse();
    }

    [Fact]
    public void PropertyPattern_Should_ApplyToResult_When_NoTypeIsGiven()
    {
        Result<int> result = 5;

        var matched = result is { IsSuccess: true };

        matched.Should().BeTrue();
    }

    [Fact]
    public void NullPattern_Should_Match_When_SuccessHoldsNull()
    {
        Result<string?> result = (string?)null;

        var matched = result is null;

        matched.Should().BeTrue();
    }
}
#endif
