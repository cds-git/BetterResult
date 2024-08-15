namespace BetterResult;

public readonly partial record struct Result<TValue>
{
    private readonly TValue? _value;
    private readonly Error _error;
    private readonly bool _isSuccess;

    private Result(TValue value)
    {
        if (value is null)
        {
            ArgumentNullException.ThrowIfNull(value);
        }

        _isSuccess = true;
        _value = value;
        _error = default;
    }

    private Result(Error error)
    {
        _isSuccess = false;
        _value = default;
        _error = error;
    }

    public bool IsSuccess => _isSuccess;

    public bool IsFailure => !_isSuccess;

    public Error Error => _error;

    public TValue Value
    {
        get
        {
            if (_value is null)
            {
                throw new InvalidOperationException();
            }

            return _value;
        }
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public static Result<TValue> Failure(Error error) => new(error);
}