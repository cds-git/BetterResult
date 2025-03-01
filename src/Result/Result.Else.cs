namespace BetterResult;

public readonly partial record struct Result
{
    /// <summary>
    /// Chains the current <see cref="Result"/> with an alternative operation by invoking the provided function <paramref name="onFailure"/>
    /// only if the current result represents a failure. If the current result is successful, the original result is returned unchanged.
    /// </summary>
    /// <param name="onFailure">The function to invoke if the current result represents a failure.</param>
    /// <returns>
    /// A new <see cref="Result"/> that is either the unchanged successful result or the result of executing <paramref name="onFailure"/>.
    /// </returns>
    public Result Else(Func<Error, Result> onFailure)
    {
        if (IsSuccess)
        {
            return this;
        }

        return onFailure(Error);
    }

    /// <summary>
    /// Asynchronously chains the current <see cref="Result"/> with an alternative operation by invoking the provided asynchronous function <paramref name="onFailure"/>
    /// only if the current result represents a failure. If the current result is successful, the original result is returned unchanged.
    /// </summary>
    /// <param name="onFailure">The asynchronous function to invoke if the current result represents a failure.</param>
    /// <returns>
    /// A task representing the asynchronous operation that yields a new <see cref="Result"/> that is either the unchanged successful result or the result of executing <paramref name="onFailure"/>.
    /// </returns>
    public async Task<Result> ElseAsync(Func<Error, Task<Result>> onFailure)
    {
        if (IsSuccess)
        {
            return this;
        }

        return await onFailure(Error).ConfigureAwait(false);
    }
}

public readonly partial record struct Result<TValue>
{
    /// <summary>
    /// Chains the current <see cref="Result{TValue}"/> with an alternative operation by invoking the provided function <paramref name="onFailure"/>
    /// only if the current result represents a failure. If the current result is successful, the original result is returned unchanged.
    /// </summary>
    /// <param name="onFailure">The function to invoke if the current result represents a failure.</param>
    /// <returns>
    /// A new <see cref="Result{TValue}"/> that is either the unchanged successful result or the result of executing <paramref name="onFailure"/>.
    /// </returns>
    public Result<TValue> Else(Func<Error, Result<TValue>> onFailure)
    {
        if (IsSuccess)
        {
            return this;
        }

        return onFailure(Error);
    }

    /// <summary>
    /// Asynchronously chains the current <see cref="Result{TValue}"/> with an alternative operation by invoking the provided asynchronous function <paramref name="onFailure"/>
    /// only if the current result represents a failure. If the current result is successful, the original result is returned unchanged.
    /// </summary>
    /// <param name="onFailure">The asynchronous function to invoke if the current result represents a failure.</param>
    /// <returns>
    /// A task representing the asynchronous operation that yields a new <see cref="Result{TValue}"/> that is either the unchanged successful result or the result of executing <paramref name="onFailure"/>.
    /// </returns>
    public async Task<Result<TValue>> ElseAsync(Func<Error, Task<Result<TValue>>> onFailure)
    {
        if (IsSuccess)
        {
            return this;
        }

        return await onFailure(Error).ConfigureAwait(false);
    }
}
