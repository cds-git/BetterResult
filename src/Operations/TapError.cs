namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Runs <paramref name="onFailure"/> with the error on a failure, then returns the result unchanged.
    /// Use it for error logging, alerts, and cleanup without swallowing the error.
    /// </summary>
    public Result<T> TapError(Action<Error> onFailure)
    {
        if (IsFailure) onFailure(Error);
        return this;
    }

    /// <inheritdoc cref="TapError(Action{Error})"/>
    public async Task<Result<T>> TapErrorAsync(Func<Error, Task> onFailure)
    {
        if (IsFailure) await onFailure(Error).ConfigureAwait(false);
        return this;
    }
}

/// <summary>
/// <see cref="Result{T}.TapError"/> for results that are still inside a task.
/// </summary>
public static class TapErrorExtensions
{
    /// <inheritdoc cref="Result{T}.TapError(Action{Error})"/>
    public static async Task<Result<T>> TapErrorAsync<T>(this Task<Result<T>> result, Action<Error> onFailure) =>
        (await result.ConfigureAwait(false)).TapError(onFailure);

    /// <inheritdoc cref="Result{T}.TapError(Action{Error})"/>
    public static async Task<Result<T>> TapErrorAsync<T>(this Task<Result<T>> result, Func<Error, Task> onFailure) =>
        await (await result.ConfigureAwait(false)).TapErrorAsync(onFailure).ConfigureAwait(false);
}
