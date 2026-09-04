namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Replaces the error with a new one. The result stays a failure; a success passes through untouched.
    /// </summary>
    /// <remarks>
    /// <c>MapError</c> translates errors, it does not recover from them. Use <c>Recover</c> to turn a
    /// failure back into a success.
    /// </remarks>
    public Result<T> MapError(Func<Error, Error> map) =>
        IsSuccess ? this : map(Error);

    /// <inheritdoc cref="MapError(Func{Error, Error})"/>
    public async Task<Result<T>> MapErrorAsync(Func<Error, Task<Error>> mapAsync) =>
        IsSuccess ? this : await mapAsync(Error).ConfigureAwait(false);
}

/// <summary>
/// <see cref="Result{T}.MapError"/> for results that are still inside a task.
/// </summary>
public static class MapErrorExtensions
{
    /// <inheritdoc cref="Result{T}.MapError(Func{Error, Error})"/>
    public static async Task<Result<T>> MapErrorAsync<T>(
        this Task<Result<T>> result, Func<Error, Error> map) =>
            (await result.ConfigureAwait(false)).MapError(map);

    /// <inheritdoc cref="Result{T}.MapError(Func{Error, Error})"/>
    public static async Task<Result<T>> MapErrorAsync<T>(
        this Task<Result<T>> result, Func<Error, Task<Error>> mapAsync) =>
            await (await result.ConfigureAwait(false)).MapErrorAsync(mapAsync).ConfigureAwait(false);
}
