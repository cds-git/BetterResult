namespace BetterResult;

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

    public bool IsSuccess => _isSuccess;

    public bool IsFailure => !_isSuccess;

    public Error Error => IsFailure ? (Error)_error! : throw new InvalidOperationException();

    public TValue Value
    {
        get
        {
            if (IsFailure)
            {
                throw new InvalidOperationException("Cannot access the value when result is of type failure");
            }

            return _value!;
        }
    }
}