namespace BetterResult;

public readonly partial record struct Result<TValue>
{
    /// <summary>
    /// Executes a side effect action on the current <see cref="Result{TValue}"/> if it represents a success, 
    /// without modifying the result. If the result is a failure, the action is not invoked.
    /// </summary>
    /// <param name="onSuccess">The action to execute if the current result is a success.</param>
    /// <returns>The original <see cref="Result{TValue}"/>, preserving its value or error state.</returns>
    public Result<TValue> Tap(Action<TValue> onSuccess)
    {
        if (IsFailure)
        {
            return Error;
        }

        onSuccess(Value);

        return this;
    }

    /// <summary>
    /// Asynchronously executes a side effect action on the current <see cref="Result{TValue}"/> if it represents a success, 
    /// without modifying the result. If the result is a failure, the action is not invoked.
    /// </summary>
    /// <param name="onSuccess">The asynchronous action to execute if the current result is a success.</param>
    /// <returns>A task representing the asynchronous operation, which yields the original <see cref="Result{TValue}"/>, 
    /// preserving its value or error state.</returns>
    public async Task<Result<TValue>> TapAsync(Func<TValue, Task> onSuccess)
    {
        if (IsFailure)
        {
            return Error;
        }

        await onSuccess(Value).ConfigureAwait(false);

        return this;
    }
}
