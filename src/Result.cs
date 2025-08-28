namespace BetterResult;

/// <summary>
/// A discriminated union of an error or void.
/// </summary>
public partial record Result
{
    private readonly Error? _error;

    private Result(Error error)
    {
        IsSuccess = false;
        _error = error;
    }

    private Result(bool isSuccess)
    {
        if (isSuccess is false)
        {
            throw new InvalidOperationException($"{nameof(isSuccess)} can only have value TRUE here. Else use contructor with Error!");
        }

        IsSuccess = true;
        _error = null;
    }

    /// <summary>
    /// Gets a value indicating whether the state is success.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no errors are present.</exception>
    public Error Error => _error is null
        ? throw new InvalidOperationException("Cannot access Error when result is of success")
        : (Error)_error;

    /// <summary>
    /// Creates a <see cref="Result"/>.
    /// </summary>
    public static implicit operator Result(bool isSuccess) => new(isSuccess);

    /// <summary>
    /// Creates a <see cref="Result"/> from an error.
    /// </summary>
    public static implicit operator Result(Error error) => new(error);

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
    /// <typeparam name="T">The type of the value in the successful result.</typeparam>
    /// <param name="value">The value of the successful result.</param>
    /// <returns>A successful <see cref="Result{T}"/> instance containing the provided value.</returns>
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="error">The <see cref="Error"/> associated with the failure.</param>
    /// <returns>A failed <see cref="Result{T}"/> instance containing the provided error.</returns>
    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
}

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