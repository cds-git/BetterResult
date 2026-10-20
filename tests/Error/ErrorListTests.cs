namespace BetterResult.Tests;

public class ErrorListTests
{
    [Fact]
    public void Combine_Should_ReturnErrorList_When_CombiningTwoErrors()
    {
        var name = Error.Validation("NAME_REQUIRED", "Name is required");
        var email = Error.Validation("EMAIL_INVALID", "Email is invalid");

        var combined = name.Combine(email);

        var list = combined.Should().BeOfType<ErrorList>().Subject;
        list.Code.Should().Be("General.Multiple");
        list.Errors.Should().Equal(name, email);
    }

    [Fact]
    public void Combine_Should_ReturnErrorList_When_CalledOnCollection()
    {
        IReadOnlyList<Error> errors = [Error.Validation("A", "a"), Error.Validation("B", "b"), Error.Validation("C", "c")];

        var combined = errors.Combine();

        combined.Should().BeOfType<ErrorList>().Which.Errors.Should().Equal(errors);
    }

    [Fact]
    public void Combine_Should_KeepSharedType_When_AllErrorsHaveSameType()
    {
        var combined = Error.Validation("A", "a").Combine(Error.Validation("B", "b"));

        combined.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public void Combine_Should_UseFailureType_When_TypesDiffer()
    {
        var combined = Error.Validation("A", "a").Combine(Error.NotFound("B", "b"));

        combined.Type.Should().Be(ErrorType.Failure);
    }

    [Fact]
    public void Combine_Should_ReturnErrorUnchanged_When_CollectionHasOne()
    {
        var error = Error.NotFound("E404", "msg").WithMetadata("UserId", 42);

        var combined = new[] { error }.Combine();

        combined.Should().BeSameAs(error);
    }

    [Fact]
    public void Combine_Should_KeepRecodedList_When_CollectionHasOne()
    {
        var form = Error.Validation("A", "a").Combine(Error.Validation("B", "b")).WithCode("FORM_INVALID");

        var combined = new[] { form }.Combine();

        combined.Code.Should().Be("FORM_INVALID");
        combined.Should().BeOfType<ErrorList>().Which.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Combine_Should_Flatten_When_CombiningIntoAList()
    {
        var a = Error.Validation("A", "a");
        var b = Error.Validation("B", "b");
        var c = Error.Validation("C", "c");

        var combined = a.Combine(b).Combine(c);

        combined.Should().BeOfType<ErrorList>().Which.Errors.Should().Equal(a, b, c);
    }

    [Fact]
    public void Combine_Should_Throw_When_CollectionIsEmpty()
    {
        var act = () => new List<Error>().Combine();

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Combine_Should_Throw_When_CollectionContainsNull()
    {
        var act = () => new[] { Error.Validation("A", "a"), null! }.Combine();

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void WithExtensions_Should_KeepIndividualErrors()
    {
        var list = Error.Validation("A", "a").Combine(Error.Validation("B", "b"));

        var changed = list
            .WithMessage("Signup failed")
            .WithCode("SIGNUP_INVALID")
            .WithType(ErrorType.Conflict)
            .WithMetadata("Form", "Signup");

        changed.Should().BeOfType<ErrorList>().Which.Errors
            .Should().Equal(((ErrorList)list).Errors);
    }

    [Fact]
    public void Equals_Should_CompareIndividualErrors()
    {
        var a = Error.Validation("A", "a").Combine(Error.Validation("B", "b"));
        var same = Error.Validation("A", "a").Combine(Error.Validation("B", "b"));
        var different = Error.Validation("A", "a").Combine(Error.Validation("C", "b"));

        a.Should().Be(same);
        a.GetHashCode().Should().Be(same.GetHashCode());
        a.Should().NotBe(different);
    }

    [Fact]
    public void Recover_Should_MatchOnSharedType()
    {
        Result<int> result = Error.Validation("A", "a").Combine(Error.Validation("B", "b"));

        var recovered = result.Recover(ErrorType.Validation, 0);

        recovered.IsSuccess.Should().BeTrue();
    }
}
