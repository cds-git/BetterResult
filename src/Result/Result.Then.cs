namespace BetterResult;

public readonly partial record struct Result
{
    /// <summary>
    /// Chains the current <see cref="Result"/> with the next operation by invoking the provided function <paramref name="onSuccess"/>
    /// only if the current result represents success. If the current result is a failure, the failure is propagated without executing <paramref name="onSuccess"/>.
    /// </summary>
    /// <param name="onSuccess">The function to invoke if the current result is a success.</param>
    /// <returns>
    /// A new <see cref="Result"/> that is either the propagated failure or the result of executing <paramref name="onSuccess"/>.
    /// </returns>
    public Result Then(Func<Result> onSuccess)
    {
        if (IsFailure)
        {
            return Error;
        }

        return onSuccess();
    }

    /// <summary>
    /// Asynchronously chains the current <see cref="Result"/> with the next operation by invoking the provided asynchronous function <paramref name="onSuccess"/>
    /// only if the current result represents success. If the current result is a failure, the failure is propagated without executing <paramref name="onSuccess"/>.
    /// </summary>
    /// <param name="onSuccess">The asynchronous function to invoke if the current result is a success.</param>
    /// <returns>
    /// A task representing the asynchronous operation that yields a new <see cref="Result"/> which is either the propagated failure or the result of executing <paramref name="onSuccess"/>.
    /// </returns>
    public async Task<Result> ThenAsync(Func<Task<Result>> onSuccess)
    {
        if (IsFailure)
        {
            return Error;
        }

        return await onSuccess().ConfigureAwait(false);
    }
}

public readonly partial record struct Result<TValue>
{
    /// <summary>
    /// Chains the current <see cref="Result{TValue}"/> with the next operation by invoking the provided function <paramref name="onSuccess"/>
    /// only if the current result represents success. If the current result is a failure, the failure is propagated without executing <paramref name="onSuccess"/>.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the next value.</typeparam>
    /// <param name="onSuccess">The function to invoke if the current result is a success.</param>
    /// <returns>
    /// A new <see cref="Result{TNextValue}"/> that contains the transformed success value if the operation succeeds, or the propagated failure.
    /// </returns>
    public Result<TNextValue> Then<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess)
    {
        if (IsFailure)
        {
            return Error;
        }

        return onSuccess(Value);
    }

    /// <summary>
    /// Asynchronously chains the current <see cref="Result{TValue}"/> with the next operation by invoking the provided asynchronous function <paramref name="onValue"/>
    /// only if the current result represents success. If the current result is a failure, the failure is propagated without executing <paramref name="onValue"/>.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the next value.</typeparam>
    /// <param name="onValue">The asynchronous function to invoke if the current result is a success.</param>
    /// <returns>
    /// A task representing the asynchronous operation that yields a new <see cref="Result{TNextValue}"/> containing either the transformed success value or the propagated failure.
    /// </returns>
    public async Task<Result<TNextValue>> ThenAsync<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onValue)
    {
        if (IsFailure)
        {
            return Error;
        }

        return await onValue(Value).ConfigureAwait(false);
    }
}
