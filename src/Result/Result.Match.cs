namespace BetterResult;

public readonly partial record struct Result
{
    /// <summary>
    /// Executes the appropriate function based on the state of the <see cref="Result"/>.
    /// If the state is success, the provided function <paramref name="onSuccess"/> is executed and its result is returned.
    /// If the state is a failure, the provided function <paramref name="onFailure"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="onSuccess">The function to execute if the state is success.</param>
    /// <param name="onFailure">The function to execute if the state is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    public TResult Match<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        if (IsFailure)
        {
            return onFailure(Error);
        }

        return onSuccess();
    }

    /// <summary>
    /// Asynchronously executes the appropriate function based on the state of the <see cref="Result"/>.
    /// If the state is success, the provided function <paramref name="onSuccess"/> is executed asynchronously and its result is returned.
    /// If the state is a failure, the provided function <paramref name="onFailure"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="onSuccess">The asynchronous function to execute if the state is success.</param>
    /// <param name="onFailure">The asynchronous function to execute if the state is a failure.</param>
    /// <returns>A task representing the asynchronous operation that yields the result of the executed function.</returns>
    public async Task<TResult> MatchAsync<TResult>(Func<Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFailure)
    {
        if (IsFailure)
        {
            return await onFailure(Error).ConfigureAwait(false);
        }

        return await onSuccess().ConfigureAwait(false);
    }
}

public readonly partial record struct Result<TValue>
{
    /// <summary>
    /// Executes the appropriate function based on the state of the <see cref="Result{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onSuccess"/> is executed and its result is returned.
    /// If the state is an error, the provided function <paramref name="onFailure"/> is executed and its result is returned.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="onSuccess">The function to execute if the state is a value.</param>
    /// <param name="onFailure">The function to execute if the state is an error.</param>
    /// <returns>The result of the executed function.</returns>
    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        if (IsFailure)
        {
            return onFailure(Error);
        }

        return onSuccess(Value);
    }
    
    /// <summary>
    /// Asynchronously executes the appropriate function based on the state of the <see cref="Result{TValue}"/>.
    /// If the state is a value, the provided function <paramref name="onSuccess"/> is executed asynchronously and its result is returned.
    /// If the state is an error, the provided function <paramref name="onFailure"/> is executed asynchronously and its result is returned.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="onSuccess">The asynchronous function to execute if the state is a value.</param>
    /// <param name="onFailure">The asynchronous function to execute if the state is an error.</param>
    /// <returns>A task representing the asynchronous operation that yields the result of the executed function.</returns>
    public async Task<TResult> MatchAsync<TResult>(Func<TValue, Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFailure)
    {
        if (IsFailure)
        {
            return await onFailure(Error).ConfigureAwait(false);
        }

        return await onSuccess(Value).ConfigureAwait(false);
    }
}