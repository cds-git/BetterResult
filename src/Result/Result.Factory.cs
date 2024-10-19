namespace BetterResult;

public readonly partial record struct Result
{
    /// <summary>
    /// Creates a successful result with no value.
    /// </summary>
    /// <returns>A successful <see cref="Result"/> instance.</returns>
    public static Result Success() => new(true);

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <param name="error">The <see cref="Error"/> associated with the failure.</param>
    /// <returns>A failed <see cref="Result"/> instance containing the provided error.</returns>
    public static Result Failure(Error error) => new(error);

    /// <summary>
    /// Creates a successful result with the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the successful result.</typeparam>
    /// <param name="value">The value of the successful result.</param>
    /// <returns>A successful <see cref="Result{TValue}"/> instance containing the provided value.</returns>
    public static Result<TValue> Success<TValue>(TValue value) => Result<TValue>.Success(value);

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <typeparam name="TValue">The type of the value in the result.</typeparam>
    /// <param name="error">The <see cref="Error"/> associated with the failure.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> instance containing the provided error.</returns>
    public static Result<TValue> Failure<TValue>(Error error) => Result<TValue>.Failure(error);
}

public readonly partial record struct Result<TValue>
{
    /// <summary>
    /// Creates a successful result containing the specified value.
    /// </summary>
    /// <param name="value">The value of the successful result.</param>
    /// <returns>A successful <see cref="Result{TValue}"/> instance containing the provided value.</returns>
    public static Result<TValue> Success(TValue value) => new(value);

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <param name="error">The <see cref="Error"/> associated with the failure.</param>
    /// <returns>A failed <see cref="Result{TValue}"/> instance containing the provided error.</returns>
    public static Result<TValue> Failure(Error error) => new(error);
}
