namespace BetterResult;

public readonly partial record struct Result<TValue>
{
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