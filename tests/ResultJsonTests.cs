#if NET11_0_OR_GREATER
using System.Text.Json;

namespace BetterResult.Tests;

public class ResultJsonTests
{
    private sealed record Envelope(Result<int> Outcome);

    [Fact]
    public void Serialize_Should_ThrowNotSupported_When_ResultIsSuccess()
    {
        Result<int> result = 42;

        var act = () => JsonSerializer.Serialize(result);

        act.Should().Throw<NotSupportedException>().WithMessage("*Result<Int32> is not serialized*Match*");
    }

    [Fact]
    public void Serialize_Should_ThrowNotSupported_When_ResultIsFailure()
    {
        Result<int> result = Error.NotFound();

        var act = () => JsonSerializer.Serialize(result);

        act.Should().Throw<NotSupportedException>().WithMessage("*is not serialized*");
    }

    [Fact]
    public void Serialize_Should_ThrowNotSupported_When_ResultIsProperty()
    {
        var envelope = new Envelope(42);

        var act = () => JsonSerializer.Serialize(envelope);

        act.Should().Throw<NotSupportedException>().WithMessage("*is not serialized*");
    }

    [Fact]
    public void Deserialize_Should_ThrowNotSupported()
    {
        var act = () => JsonSerializer.Deserialize<Result<int>>("42");

        act.Should().Throw<NotSupportedException>().WithMessage("*cannot be deserialized*");
    }

    [Fact]
    public void TypeInfo_Should_NotBeUnion_So_ContractBuildingSucceeds()
    {
        var info = JsonSerializerOptions.Default.GetTypeInfo(typeof(Result<string>));

        info.Kind.Should().Be(System.Text.Json.Serialization.Metadata.JsonTypeInfoKind.None);
    }

    [Fact]
    public void Serialize_Should_WriteError_When_ErrorIsSerializedDirectly()
    {
        var json = JsonSerializer.Serialize(Error.NotFound("X", "x"));

        json.Should().Contain("\"Code\":\"X\"");
    }
}
#endif
