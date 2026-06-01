namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Transforms a failure result by mapping its <see cref="Error"/> to a new <see cref="Error"/>.
    /// The result stays a failure; if the current result is successful, the value is preserved unchanged.
    /// </summary>
    /// <remarks>
    /// <c>MapError</c> is for error <em>translation</em> only — it cannot turn a failure into a success.
    /// To recover from failures (conditionally, by <see cref="ErrorType"/> or predicate), use
    /// <c>Recover</c>. To change the value type while recovering, transform the success rail with
    /// <c>Map</c>/<c>Bind</c> and then call <c>Recover</c>.
    /// </remarks>
    /// <param name="map">The function to apply to the error if the result is a failure.</param>
    /// <returns>A failure result with the mapped error, or the current successful result unchanged.</returns>
    public Result<T> MapError(Func<Error, Error> map) =>
        IsSuccess ? this : map(Error);

    /// <summary>
    /// Asynchronously transforms a failure result by mapping its <see cref="Error"/> to a new <see cref="Error"/>.
    /// The result stays a failure; if the current result is successful, the value is preserved unchanged.
    /// </summary>
    /// <param name="mapAsync">The asynchronous function to apply to the error if the result is a failure.</param>
    /// <returns>A task containing a failure result with the mapped error, or the current successful result unchanged.</returns>
    public async Task<Result<T>> MapErrorAsync(Func<Error, Task<Error>> mapAsync) =>
        IsSuccess ? this : await mapAsync(Error).ConfigureAwait(false);
}

/// <summary>
/// Provides extension methods for asynchronously mapping error operations on Task-wrapped Result instances.
/// </summary>
public static class MapErrorExtensions
{
    /// <summary>
    /// Asynchronously maps a Task-wrapped Result's error to a new <see cref="Error"/> with value preservation.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to map errors on.</param>
    /// <param name="map">The function to apply to the error if the result is a failure.</param>
    /// <returns>A task containing a failure result with the mapped error, or the original successful result unchanged.</returns>
    public static async Task<Result<T>> MapErrorAsync<T>(
        this Task<Result<T>> result, Func<Error, Error> map) =>
            (await result.ConfigureAwait(false)).MapError(map);

    /// <summary>
    /// Asynchronously maps a Task-wrapped Result's error to a new <see cref="Error"/> via an async function, with value preservation.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to map errors on.</param>
    /// <param name="mapAsync">The asynchronous function to apply to the error if the result is a failure.</param>
    /// <returns>A task containing a failure result with the mapped error, or the original successful result unchanged.</returns>
    public static async Task<Result<T>> MapErrorAsync<T>(
        this Task<Result<T>> result, Func<Error, Task<Error>> mapAsync) =>
            await (await result.ConfigureAwait(false)).MapErrorAsync(mapAsync).ConfigureAwait(false);
}