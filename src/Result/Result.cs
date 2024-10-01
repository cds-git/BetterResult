namespace BetterResult;

public readonly partial record struct Result
{
    private readonly bool _isSuccess;
    private readonly Error? _error;

    private Result(Error error)
    {
        _isSuccess = false;
        _error = error;
    }

    private Result(bool isSuccess)
    {
        if (isSuccess is false)
        {
            throw new InvalidOperationException();
        }

        _isSuccess = true;
        _error = null;
    }

    public bool IsSuccess => _isSuccess;
    public bool IsFailure => !_isSuccess;

    public Error Error
    {
        get
        {
            if (_error is null)
            {
                throw new InvalidOperationException("Cannot access Error when result is of success");
            }

            return (Error)_error;
        }
    }
}