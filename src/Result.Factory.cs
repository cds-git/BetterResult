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

    /// <summary>
    /// Succeeds with <paramref name="value"/> when <paramref name="condition"/> is true, otherwise fails with <paramref name="error"/>.
    /// </summary>
    public static Result<T> SuccessIf<T>(bool condition, T value, Error error) =>
        condition ? Result<T>.Success(value) : Result<T>.Failure(error);

    /// <summary>
    /// Succeeds when <paramref name="condition"/> is true, otherwise fails with <paramref name="error"/>.
    /// </summary>
    public static Result<NoValue> SuccessIf(bool condition, Error error) =>
        SuccessIf(condition, NoValue.Instance, error);

    /// <summary>
    /// Fails with <paramref name="error"/> when <paramref name="condition"/> is true, otherwise succeeds with <paramref name="value"/>.
    /// </summary>
    public static Result<T> FailureIf<T>(bool condition, T value, Error error) =>
        SuccessIf(!condition, value, error);

    /// <summary>
    /// Fails with <paramref name="error"/> when <paramref name="condition"/> is true, otherwise succeeds.
    /// </summary>
    public static Result<NoValue> FailureIf(bool condition, Error error) =>
        SuccessIf(!condition, NoValue.Instance, error);
}
