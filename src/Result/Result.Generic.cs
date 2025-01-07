namespace BetterResult;

/// <summary>
/// A discriminated union of a value or an error.
/// </summary>
/// <typeparam name="TValue">The type of the underlying <see cref="Value"/>.</typeparam>
public readonly partial record struct Result<TValue>
{
    private readonly TValue? _value;
    private readonly Error? _error;

    private Result(TValue? value)
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
    public TValue Value => IsFailure 
        ? throw new InvalidOperationException("Cannot access the value when result is of type failure. Check IsFailure before accessing value!")
        : _value!;
}