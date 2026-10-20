namespace BetterResult.Tests;

public class CustomErrorTests
{
    private sealed record UserNotFound(int UserId)
        : Error(ErrorType.NotFound, "USER_NOT_FOUND", $"User {UserId} was not found");

    private sealed record EmailTaken(string Email)
        : Error(ErrorType.Conflict, "EMAIL_TAKEN", $"{Email} is already registered");

    [Fact]
    public void CustomError_Should_CarryTypeCodeMessageAndData()
    {
        var error = new UserNotFound(42);

        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("USER_NOT_FOUND");
        error.Message.Should().Be("User 42 was not found");
        error.UserId.Should().Be(42);
    }

    [Fact]
    public void ImplicitConversion_Should_CreateFailure_When_CustomErrorIsReturned()
    {
        static Result<string> GetName(int id) => new UserNotFound(id);

        var result = GetName(7);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UserNotFound>().Which.UserId.Should().Be(7);
    }

    [Fact]
    public void Error_Should_MatchOnSubtype_When_Switching()
    {
        Result<int> result = new EmailTaken("a@b.c");

        var message = result.Error switch
        {
            UserNotFound e => $"no user {e.UserId}",
            EmailTaken e => $"taken {e.Email}",
            _ => "other",
        };

        message.Should().Be("taken a@b.c");
    }

    [Fact]
    public void Recover_Should_MatchOnSubtype_When_PredicateTestsType()
    {
        Result<string> result = new UserNotFound(1);

        var recovered = result.Recover(e => e is UserNotFound, _ => "guest");

        recovered.Value.Should().Be("guest");
    }

    [Fact]
    public void WithExtensions_Should_KeepSubtypeAndData()
    {
        Error error = new UserNotFound(42);

        var changed = error
            .WithMessage("Checkout failed")
            .WithCode("CHECKOUT_USER_MISSING")
            .WithType(ErrorType.Validation)
            .WithMetadata("OrderId", 9);

        var typed = changed.Should().BeOfType<UserNotFound>().Subject;
        typed.UserId.Should().Be(42);
        typed.Code.Should().Be("CHECKOUT_USER_MISSING");
        typed.Message.Should().Be("Checkout failed: User 42 was not found");
        typed.GetMetadata<int>("OrderId").Should().Be(9);
    }

    [Fact]
    public void With_Should_KeepSubtype_When_UsedDirectly()
    {
        var error = new UserNotFound(42) with { Code = "RECODED" };

        error.UserId.Should().Be(42);
        error.Code.Should().Be("RECODED");
    }

    [Fact]
    public void With_Should_Throw_When_CodeIsSetToNull()
    {
        var act = () => new UserNotFound(42) with { Code = null! };

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Equals_Should_CompareCustomData()
    {
        new UserNotFound(1).Should().Be(new UserNotFound(1));
        new UserNotFound(1).GetHashCode().Should().Be(new UserNotFound(1).GetHashCode());
        new UserNotFound(1).Should().NotBe(new UserNotFound(2));
    }

    [Fact]
    public void Equals_Should_BeFalse_When_SubtypeDiffersFromPlainError()
    {
        Error plain = Error.NotFound("USER_NOT_FOUND", "User 42 was not found");
        Error typed = new UserNotFound(42);

        plain.Should().NotBe(typed);
        typed.Should().NotBe(plain);
    }

    [Fact]
    public void Combine_Should_KeepSubtypes_When_CombiningCustomErrors()
    {
        var combined = new UserNotFound(1).Combine(new EmailTaken("a@b.c"));

        combined.Should().BeOfType<ErrorList>().Which.Errors
            .Should().SatisfyRespectively(
                first => first.Should().BeOfType<UserNotFound>(),
                second => second.Should().BeOfType<EmailTaken>());
    }
}
