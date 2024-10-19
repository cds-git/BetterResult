namespace BetterResult;

/// <summary>
/// A discriminated union of a value or an error.
/// </summary>
/// <typeparam name="TValue">The type of the underlying <see cref="Value"/>.</typeparam>
public readonly partial record struct Result<TValue>
{
    private readonly TValue? _value;
    private readonly Error? _error;
    private readonly bool _isSuccess;

    private Result(TValue value)
    {
        if (value is null)
        {
            ArgumentNullException.ThrowIfNull(value);
        }

        _isSuccess = true;
        _value = value;
        _error = null;
    }

    private Result(Error error)
    {
        _isSuccess = false;
        _value = default;
        _error = error;
    }

    /// <summary>
    /// Gets a value indicating whether the state is success.
    /// </summary>
    public bool IsSuccess => _isSuccess;

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    public bool IsFailure => !_isSuccess;

    /// <summary>
    /// Gets the error.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no errors are present.</exception>
    public Error Error => IsFailure ? (Error)_error! 
        : throw new InvalidOperationException("Cannot access the error when the result is of type success. Check IsFailure before accessing Error!");

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no value is present.</exception>
    public TValue Value
    {
        get
        {
            if (IsFailure)
            {
                throw new InvalidOperationException("Cannot access the value when result is of type failure. Check IsFailure before accessing value!");
            }

            return _value!;
        }
    }
}