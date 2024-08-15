namespace BetterResult;

public readonly partial record struct Result<TValue>
{
    private readonly TValue? _value;
    private readonly Error? _error;

    private Result(TValue value)
    {
        if (value is null)
        {
            ArgumentNullException.ThrowIfNull(value);
        }

        _value = value;
        _error = null;
    }

    private Result(Error error)
    {
        if (error is null)
        {
            ArgumentNullException.ThrowIfNull(error);
        }

        _value = default;
        _error = error;
    }

    public bool IsSuccess => !IsFailure;

    public bool IsFailure => _error is not null;

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

    public Error Error
    {
        get
        {
            if (_error is null)
            {
                throw new InvalidOperationException();
            }

            return _error;
        }
    }


    public static Result<TValue> Success(TValue value) => new(value);
    public static Result<TValue> Failure(Error error) => new(error);
}