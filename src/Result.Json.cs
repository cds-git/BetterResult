#if NET11_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BetterResult;

[JsonConverter(typeof(ResultJsonConverter<>))]
public readonly partial record struct Result<T>;

/// <summary>
/// Refuses to serialize a result, with a message that says what to do instead. Without it, .NET 11
/// treats <see cref="Result{T}"/> as a union it cannot read the cases of, and fails while building
/// every endpoint rather than the one that returned a result. An open generic converter in the
/// attribute needs .NET 11, and keeps <c>MakeGenericType</c> out of the AOT path.
/// </summary>
internal sealed class ResultJsonConverter<T> : JsonConverter<Result<T>>
{
    public override Result<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        throw new NotSupportedException(
            $"{Name} cannot be deserialized. Read into your own type and turn that into a result.");

    public override void Write(Utf8JsonWriter writer, Result<T> value, JsonSerializerOptions options) =>
        throw new NotSupportedException(
            $"{Name} is not serialized. Use Match or a switch to turn it into a response type first.");

    private static string Name => $"Result<{typeof(T).Name}>";
}
#endif
