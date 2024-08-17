namespace BetterResult;

public readonly partial record struct Result<TValue>
{
    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        if (IsFailure)
        {
            return onFailure(Error);
        }

        return onSuccess(Value);
    }

    public async Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFailure)
    {
        if (IsFailure)
        {
            return await onFailure(Error).ConfigureAwait(false);
        }

        return await onSuccess(Value).ConfigureAwait(false);
    }
}