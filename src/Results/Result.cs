namespace BetterResult;

public class Result<TValue>
{
    private readonly TValue? _value;
    private readonly Error _error;

    private readonly bool _isSuccess;

    private Result(TValue value)
    {
        _isSuccess = true;
        _value = value;
        _error = Error.None;
    }

    private Result(Error error)
    {
        _isSuccess = false;
        _value = default;
        _error = error;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public static Result<TValue> Failure(Error error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => new(error);

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<Error, TResult> failure)
    {
        return _isSuccess ? success(_value!) : failure(_error);
    }

    public async Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<Error, Task<TResult>> failure)
    {
        if (_isSuccess)
        {
            return await success(_value!).ConfigureAwait(false);
        }

        return await failure(_error).ConfigureAwait(false);
    }
}