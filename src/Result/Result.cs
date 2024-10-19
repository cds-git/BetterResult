namespace BetterResult;

/// <summary>
/// A discriminated union of an error or void.
/// </summary>
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
            throw new InvalidOperationException($"{nameof(isSuccess)} can only have value TRUE here. Else use contructor with Error!");
        }

        _isSuccess = true;
        _error = null;
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