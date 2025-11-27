namespace BetterResult;

/// <summary>
/// A discriminated union of a value or an error.
/// </summary>
/// <typeparam name="T">The type of the underlying <see cref="Value"/>.</typeparam>
public partial record Result<T>
{
    private readonly T? _value;
    private readonly Error? _error;

    private Result(T? value)
    {
        IsFailure = false;
        _value = value;
        _error = null;
    }

    private Result(Error error)
    {
        IsFailure = true;
        _value = default;
        _error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the state is success.
    /// </summary>
    public bool IsSuccess => !IsFailure;

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    public bool IsFailure { get; }

    /// <summary>
    /// Gets the error.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no errors are present.</exception>
    public Error Error => IsFailure
        ? (Error)_error!
        : throw new InvalidOperationException("Cannot access the error when the result is of type success. Check IsFailure before accessing Error!");

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no value is present.</exception>
    public T Value => IsFailure
        ? throw new InvalidOperationException("Cannot access the value when result is of type failure. Check IsFailure before accessing value!")
        : _value!;

    /// <summary>
    /// Creates a <see cref="Result{T}"/> from a value.
    /// </summary>
    public static implicit operator Result<T>(T value) => new(value);

    /// <summary>
    /// Creates a <see cref="Result{T}"/> from an error.
    /// </summary>
    public static implicit operator Result<T>(Error error) => new(error);

    /// <summary>
    /// Creates a successful result containing the specified value.
    /// </summary>
    /// <param name="value">The value of the successful result.</param>
    /// <returns>A successful <see cref="Result{T}"/> instance containing the provided value.</returns>
    public static Result<T> Success(T value) => new(value);

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <param name="error">The <see cref="Error"/> associated with the failure.</param>
    /// <returns>A failed <see cref="Result{T}"/> instance containing the provided error.</returns>
    public static Result<T> Failure(Error error) => new(error);
}

/// <summary>
/// Represents the absence of a meaningful value.
/// Use with <see cref="Result{T}"/> for operations that don't return a value.
/// </summary>
/// <example>
/// <code>
/// Result&lt;NoValue&gt; DeleteUser(int id)
/// {
///     if (id &lt;= 0) 
///         return Error.Validation("INVALID_ID", "ID must be positive");
///     
///     _repository.Delete(id);
///     return NoValue.Instance;
/// }
/// </code>
/// </example>
public readonly record struct NoValue
{
    /// <summary>
    /// Gets the singleton instance of <see cref="NoValue"/>.
    /// </summary>
    public static NoValue Instance => default;
}