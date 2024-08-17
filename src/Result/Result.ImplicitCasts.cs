namespace BetterResult;

public readonly partial record struct Result
{
    public static implicit operator Result(bool isSuccess) => new(isSuccess);
    public static implicit operator Result(Error error) => new(error);
}

public readonly partial record struct Result<TValue>
{
    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => new(error);
}