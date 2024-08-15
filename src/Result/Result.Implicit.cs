namespace BetterResult;

public readonly partial record struct Result<TValue>
{
    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => new(error);
}