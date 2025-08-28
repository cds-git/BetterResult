namespace BetterResult;

public partial record Result
{
    /// <summary>
    /// Transforms a successful void result into a result with a value by applying the map function.
    /// If the current result is a failure, the error is propagated without executing the map function.
    /// </summary>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="map">The function to execute if the current result is successful.</param>
    /// <returns>A result containing the mapped value if successful, or the current error if failed.</returns>
    public Result<U> Map<U>(Func<U> map) =>
        IsSuccess ? map() : Error;

    /// <summary>
    /// Asynchronously transforms a successful void result into a result with a value by applying the map function.
    /// If the current result is a failure, the error is propagated without executing the map function.
    /// </summary>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="mapAsync">The asynchronous function to execute if the current result is successful.</param>
    /// <returns>A task containing a result with the mapped value if successful, or the current error if failed.</returns>
    public async Task<Result<U>> MapAsync<U>(Func<Task<U>> mapAsync) =>
        IsSuccess ? await mapAsync().ConfigureAwait(false) : Error;
}

public partial record Result<T>
{
    /// <summary>
    /// Transforms the value in a successful result by applying the map function.
    /// If the current result is a failure, the error is propagated without executing the map function.
    /// </summary>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="map">The function to apply to the current value if the result is successful.</param>
    /// <returns>A result containing the mapped value if successful, or the current error if failed.</returns>
    public Result<U> Map<U>(Func<T, U> map) =>
        IsSuccess ? map(Value) : Error;

    /// <summary>
    /// Asynchronously transforms the value in a successful result by applying the map function.
    /// If the current result is a failure, the error is propagated without executing the map function.
    /// </summary>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="mapAsync">The asynchronous function to apply to the current value if the result is successful.</param>
    /// <returns>A task containing a result with the mapped value if successful, or the current error if failed.</returns>
    public async Task<Result<U>> MapAsync<U>(Func<T, Task<U>> mapAsync) =>
        IsSuccess ? await mapAsync(Value).ConfigureAwait(false) : Error;
}

/// <summary>
/// Provides extension methods for asynchronously mapping operations on Task-wrapped Result instances.
/// </summary>
public static class MapExtensions
{
    /// <summary>
    /// Asynchronously maps a synchronous function on a Task-wrapped void Result.
    /// </summary>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="result">The task containing the result to map on.</param>
    /// <param name="map">The function to execute if the result is successful.</param>
    /// <returns>A task containing a result with the mapped value if successful, or the original error if failed.</returns>
    public static async Task<Result<U>> MapAsync<U>(this Task<Result> result, Func<U> map) =>
        (await result.ConfigureAwait(false)).Map(map);

    /// <summary>
    /// Asynchronously maps an asynchronous function on a Task-wrapped void Result.
    /// </summary>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="result">The task containing the result to map on.</param>
    /// <param name="mapAsync">The asynchronous function to execute if the result is successful.</param>
    /// <returns>A task containing a result with the mapped value if successful, or the original error if failed.</returns>
    public static async Task<Result<U>> MapAsync<U>(this Task<Result> result, Func<Task<U>> mapAsync) =>
        await (await result.ConfigureAwait(false)).MapAsync(mapAsync).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously maps a synchronous function on a Task-wrapped Result with value transformation.
    /// </summary>
    /// <typeparam name="T">The type of the value in the input result.</typeparam>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="result">The task containing the result to map on.</param>
    /// <param name="map">The function to apply to the current value if the result is successful.</param>
    /// <returns>A task containing a result with the mapped value if successful, or the original error if failed.</returns>
    public static async Task<Result<U>> MapAsync<T, U>(
        this Task<Result<T>> result, Func<T, U> map) =>
            (await result.ConfigureAwait(false)).Map(map);

    /// <summary>
    /// Asynchronously maps an asynchronous function on a Task-wrapped Result with value transformation.
    /// </summary>
    /// <typeparam name="T">The type of the value in the input result.</typeparam>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="result">The task containing the result to map on.</param>
    /// <param name="mapAsync">The asynchronous function to apply to the current value if the result is successful.</param>
    /// <returns>A task containing a result with the mapped value if successful, or the original error if failed.</returns>
    public static async Task<Result<U>> MapAsync<T, U>(
        this Task<Result<T>> result, Func<T, Task<U>> mapAsync) =>
            await (await result.ConfigureAwait(false)).MapAsync(mapAsync).ConfigureAwait(false);
}