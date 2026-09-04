namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Runs <paramref name="onSuccess"/> with the value on a success, then returns the result unchanged.
    /// Use it for logging, caching, and other side effects mid-pipeline.
    /// </summary>
    public Result<T> Tap(Action<T> onSuccess)
    {
        if (IsSuccess) onSuccess(Value);
        return this;
    }

    /// <inheritdoc cref="Tap(Action{T})"/>
    public async Task<Result<T>> TapAsync(Func<T, Task> onSuccess)
    {
        if (IsSuccess) await onSuccess(Value).ConfigureAwait(false);
        return this;
    }
}

/// <summary>
/// <see cref="Result{T}.Tap"/> for results that are still inside a task.
/// </summary>
public static class TapExtensions
{
    /// <inheritdoc cref="Result{T}.Tap(Action{T})"/>
    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> result, Action<T> onSuccess) =>
        (await result.ConfigureAwait(false)).Tap(onSuccess);

    /// <inheritdoc cref="Result{T}.Tap(Action{T})"/>
    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> result, Func<T, Task> onSuccess) =>
        await (await result.ConfigureAwait(false)).TapAsync(onSuccess).ConfigureAwait(false);
}
