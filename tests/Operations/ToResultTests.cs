namespace BetterResult.Tests.Operations;

public class ToResultTests
{
    private static Error Missing => Error.NotFound("MISSING", "Missing");

    [Fact]
    public void ToResult_Should_ReturnValue_When_ReferenceIsNotNull()
    {
        string? value = "hello";

        Result<string> result = value.ToResult(Missing);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("hello");
    }

    [Fact]
    public void ToResult_Should_ReturnError_When_ReferenceIsNull()
    {
        string? value = null;

        var result = value.ToResult(Missing);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("MISSING");
    }

    [Fact]
    public void ToResult_Should_ReturnUnwrappedValue_When_NullableStructHasValue()
    {
        int? value = 42;

        Result<int> result = value.ToResult(Missing);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void ToResult_Should_ReturnError_When_NullableStructIsNull()
    {
        int? value = null;

        var result = value.ToResult(Missing);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("MISSING");
    }

    [Fact]
    public async Task ToResultAsync_Should_ReturnValue_When_ReferenceIsNotNull()
    {
        Result<User> result = await FindUserAsync(1).ToResultAsync(Missing);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(1);
    }

    [Fact]
    public async Task ToResultAsync_Should_ReturnError_When_ReferenceIsNull()
    {
        var result = await FindUserAsync(999).ToResultAsync(Missing);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("MISSING");
    }

    [Fact]
    public async Task ToResultAsync_Should_ReturnUnwrappedValue_When_NullableStructHasValue()
    {
        Result<int> result = await FindAgeAsync(1).ToResultAsync(Missing);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(30);
    }

    [Fact]
    public async Task ToResultAsync_Should_ReturnError_When_NullableStructIsNull()
    {
        var result = await FindAgeAsync(999).ToResultAsync(Missing);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("MISSING");
    }

    [Fact]
    public async Task ToResultAsync_Should_JoinPipeline_When_ReferenceIsNotNull()
    {
        var result = await FindUserAsync(1)
            .ToResultAsync(Missing)
            .MapAsync(user => user.Name);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("User1");
    }

    private static async Task<User?> FindUserAsync(int id)
    {
        await Task.Yield();
        return id == 999 ? null : new User(id, $"User{id}");
    }

    private static async Task<int?> FindAgeAsync(int id)
    {
        await Task.Yield();
        return id == 999 ? null : 30;
    }

    private sealed record User(int Id, string Name);
}
