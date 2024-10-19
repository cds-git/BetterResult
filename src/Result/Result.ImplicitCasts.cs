namespace BetterResult;

public readonly partial record struct Result
{
    /// <summary>
    /// Creates a <see cref="Result"/>.
    /// </summary>
    public static implicit operator Result(bool isSuccess) => new(isSuccess);

    /// <summary>
    /// Creates a <see cref="Result"/> from an error.
    /// </summary>
    public static implicit operator Result(Error error) => new(error);
}

public readonly partial record struct Result<TValue>
{
    /// <summary>
    /// Creates a <see cref="Result{TValue}"/> from a value.
    /// </summary>
    public static implicit operator Result<TValue>(TValue value) => new(value);

    /// <summary>
    /// Creates a <see cref="Result{TValue}"/> from an error.
    /// </summary>
    public static implicit operator Result<TValue>(Error error) => new(error);
}