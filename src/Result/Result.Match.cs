namespace BetterResult;

public readonly partial record struct Result<TValue>
{
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<Error, TResult> failure)
    {
        return IsSuccess ? success(Value) : failure(Error);
    }

    public async Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> success, Func<Error, Task<TResult>> failure)
    {
        if (IsSuccess)
        {
            return await success(Value).ConfigureAwait(false);
        }

        return await failure(Error).ConfigureAwait(false);
    }
}