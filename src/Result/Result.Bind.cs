namespace BetterResult;

public readonly partial record struct Result
{
    /// <summary>
    /// Transforms the current <see cref="Result"/> into a new <see cref="Result"/> 
    /// by applying the provided function <paramref name="onSuccess"/> if the result represents success.
    /// If the result is a failure, the failure is propagated without invoking <paramref name="onSuccess"/>.
    /// </summary>
    /// <param name="onSuccess">The function to apply if the current result is a success.</param>
    /// <returns>A new <see cref="Result"/> representing either a propagated failure or a success after executing <paramref name="onSuccess"/>.</returns>
    public Result Bind(Func<Result> onSuccess)
    {
        if (IsFailure)
        {
            return Error;
        }

        return onSuccess();
    }

    /// <summary>
    /// Asynchronously transforms the current <see cref="Result"/> into a new <see cref="Result"/> 
    /// by applying the provided asynchronous function <paramref name="onSuccess"/> if the result represents success.
    /// If the result is a failure, the failure is propagated without invoking <paramref name="onSuccess"/>.
    /// </summary>
    /// <param name="onSuccess">The asynchronous function to apply if the current result is a success.</param>
    /// <returns>A task representing the asynchronous operation that yields a new <see cref="Result"/> 
    /// representing either a propagated failure or a success after executing <paramref name="onSuccess"/>.</returns>
    public async Task<Result> BindAsync(Func<Task<Result>> onSuccess)
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
    /// Transforms the current <see cref="Result{TValue}"/> into a new <see cref="Result{TNextValue}"/> 
    /// by applying the provided function <paramref name="onSuccess"/> if the result represents success.
    /// If the result is a failure, the failure is propagated without invoking <paramref name="onSuccess"/>.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the next value.</typeparam>
    /// <param name="onSuccess">The function to apply if the current result is a success.</param>
    /// <returns>A new <see cref="Result{TNextValue}"/> representing either the transformed success value 
    /// or the propagated failure.</returns>
    public Result<TNextValue> Bind<TNextValue>(Func<TValue, Result<TNextValue>> onSuccess)
    {
        if (IsFailure)
        {
            return Error;
        }

        return onSuccess(Value);
    }

    /// <summary>
    /// Asynchronously transforms the current <see cref="Result{TValue}"/> into a new <see cref="Result{TNextValue}"/> 
    /// by applying the provided asynchronous function <paramref name="onValue"/> if the result represents success.
    /// If the result is a failure, the failure is propagated without invoking <paramref name="onValue"/>.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the next value.</typeparam>
    /// <param name="onValue">The asynchronous function to apply if the current result is a success.</param>
    /// <returns>A task representing the asynchronous operation that yields a new <see cref="Result{TNextValue}"/> 
    /// representing either the transformed success value or the propagated failure.</returns>
    public async Task<Result<TNextValue>> BindAsync<TNextValue>(Func<TValue, Task<Result<TNextValue>>> onValue)
    {
        if (IsFailure)
        {
            return Error;
        }

        return await onValue(Value).ConfigureAwait(false);
    }
}