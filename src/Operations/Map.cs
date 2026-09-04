namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Transforms the value with <paramref name="map"/>. A failure passes through untouched and
    /// <paramref name="map"/> never runs.
    /// </summary>
    public Result<U> Map<U>(Func<T, U> map) =>
        IsSuccess ? map(Value) : Error;

    /// <inheritdoc cref="Map{U}(Func{T, U})"/>
    public async Task<Result<U>> MapAsync<U>(Func<T, Task<U>> mapAsync) =>
        IsSuccess ? await mapAsync(Value).ConfigureAwait(false) : Error;
}

/// <summary>
/// <see cref="Result{T}.Map{U}"/> for results that are still inside a task.
/// </summary>
public static class MapExtensions
{
    /// <inheritdoc cref="Result{T}.Map{U}(Func{T, U})"/>
    public static async Task<Result<U>> MapAsync<T, U>(
        this Task<Result<T>> result, Func<T, U> map) =>
            (await result.ConfigureAwait(false)).Map(map);

    /// <inheritdoc cref="Result{T}.Map{U}(Func{T, U})"/>
    public static async Task<Result<U>> MapAsync<T, U>(
        this Task<Result<T>> result, Func<T, Task<U>> mapAsync) =>
            await (await result.ConfigureAwait(false)).MapAsync(mapAsync).ConfigureAwait(false);
}
