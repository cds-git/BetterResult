namespace BetterResult;

/// <summary>
/// Factory methods for creating results.
/// </summary>
public static partial class Result
{
    /// <summary>
    /// Creates a successful result with no value. Same as Result&lt;NoValue&gt;.Success(NoValue.Instance).
    /// </summary>
    public static Result<NoValue> Success() => NoValue.Instance;

    /// <summary>
    /// Creates a successful result holding <paramref name="value"/>.
    /// </summary>
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);

    /// <summary>
    /// Creates a failed result holding <paramref name="error"/>.
    /// </summary>
    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
}
