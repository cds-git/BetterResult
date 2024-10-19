namespace BetterResult;

public readonly partial record struct Result
{
    /// <summary>
    /// Attempts to execute the provided function <paramref name="onSuccess"/> if the result is a success. 
    /// If an exception occurs during execution, the provided <paramref name="error"/> is returned as a failure.
    /// </summary>
    /// <param name="onSuccess">The function to apply if the current result is a success.</param>
    /// <param name="error">The error to return if an exception is thrown during the operation.</param>
    /// <returns>A new <see cref="Result"/> representing either a propagated failure, a success after executing <paramref name="onSuccess"/>, 
    /// or a failure due to the exception.</returns>
    public Result TryCatch(Action onSuccess, Error error)
    {
        try
        {
            if (IsFailure)
            {
                return Error;
            }

            onSuccess();

            return Result.Success();
        }
        catch
        {
            return Result.Failure(error);
        }
    }

    /// <summary>
    /// Asynchronously attempts to execute the provided function <paramref name="onSuccess"/> if the result is a success. 
    /// If an exception occurs during execution, the provided <paramref name="error"/> is returned as a failure.
    /// </summary>
    /// <param name="onSuccess">The asynchronous function to apply if the current result is a success.</param>
    /// <param name="error">The error to return if an exception is thrown during the operation.</param>
    /// <returns>A task representing the asynchronous operation that yields a new <see cref="Result"/> 
    /// representing either a propagated failure, a success after executing <paramref name="onSuccess"/>, 
    /// or a failure due to the exception.</returns>
    public async Task<Result> TryCatchAsync(Func<Task> onSuccess, Error error)
    {
        try
        {
            if (IsFailure)
            {
                return Error;
            }

            await onSuccess().ConfigureAwait(false);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(error);
        }
    }
}

public readonly partial record struct Result<TValue>
{
    /// <summary>
    /// Attempts to transform the current <see cref="Result{TValue}"/> into a new <see cref="Result{TNextValue}"/> 
    /// by applying the provided function <paramref name="onSuccess"/> if the result represents success. 
    /// If an exception occurs during the transformation, the provided <paramref name="error"/> is returned as a failure.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the next value.</typeparam>
    /// <param name="onSuccess">The function to apply if the current result is a success.</param>
    /// <param name="error">The error to return if an exception is thrown during the transformation.</param>
    /// <returns>A new <see cref="Result{TNextValue}"/> representing either the transformed success value, 
    /// the propagated failure, or the failure caused by the exception.</returns>
    public Result<TNextValue> TryCatch<TNextValue>(Func<TValue, TNextValue> onSuccess, Error error)
    {
        try
        {
            if (IsFailure)
            {
                return Error;
            }

            return onSuccess(Value);
        }
        catch
        {
            return Result<TNextValue>.Failure(error);
        }
    }

    /// <summary>
    /// Asynchronously attempts to transform the current <see cref="Result{TValue}"/> into a new <see cref="Result{TNextValue}"/> 
    /// by applying the provided asynchronous function <paramref name="onSuccess"/> if the result represents success. 
    /// If an exception occurs during the transformation, the provided <paramref name="error"/> is returned as a failure.
    /// </summary>
    /// <typeparam name="TNextValue">The type of the next value.</typeparam>
    /// <param name="onSuccess">The asynchronous function to apply if the current result is a success.</param>
    /// <param name="error">The error to return if an exception is thrown during the transformation.</param>
    /// <returns>A task representing the asynchronous operation that yields a new <see cref="Result{TNextValue}"/> 
    /// representing either the transformed success value, the propagated failure, or the failure caused by the exception.</returns>
    public async Task<Result<TNextValue>> TryCatchAsync<TNextValue>(Func<TValue, Task<TNextValue>> onSuccess, Error error)
    {
        try
        {
            if (IsFailure)
            {
                return Error;
            }

            return await onSuccess(Value).ConfigureAwait(false);
        }
        catch
        {
            return Result<TNextValue>.Failure(error);
        }
    }
}
