namespace BetterResult;

public partial record Result
{
    /// <summary>
    /// Transforms a failure result by applying the map function to the error.
    /// If the current result is successful, it is returned unchanged.
    /// </summary>
    /// <param name="map">The function to apply to the error if the result is a failure.</param>
    /// <returns>The result of the map function if failed, or the current successful result unchanged.</returns>
    public Result MapError(Func<Error, Result> map) =>
        IsSuccess ? Success() : map(Error);

    /// <summary>
    /// Asynchronously transforms a failure result by applying the map function to the error.
    /// If the current result is successful, it is returned unchanged.
    /// </summary>
    /// <param name="mapAsync">The asynchronous function to apply to the error if the result is a failure.</param>
    /// <returns>A task containing the result of the map function if failed, or the current successful result unchanged.</returns>
    public async Task<Result> MapErrorAsync(Func<Error, Task<Result>> mapAsync) =>
        IsSuccess ? Success() : await mapAsync(Error).ConfigureAwait(false);
}

public partial record Result<T>
{
    /// <summary>
    /// Transforms a failure result by applying the map function to the error.
    /// If the current result is successful, the value is preserved unchanged.
    /// </summary>
    /// <param name="map">The function to apply to the error if the result is a failure.</param>
    /// <returns>The result of the map function if failed, or the current successful result unchanged.</returns>
    public Result<T> MapError(Func<Error, Result<T>> map) =>
        IsSuccess ? Value : map(Error);

    /// <summary>
    /// Asynchronously transforms a failure result by applying the map function to the error.
    /// If the current result is successful, the value is preserved unchanged.
    /// </summary>
    /// <param name="mapAsync">The asynchronous function to apply to the error if the result is a failure.</param>
    /// <returns>A task containing the result of the map function if failed, or the current successful result unchanged.</returns>
    public async Task<Result<T>> MapErrorAsync(Func<Error, Task<Result<T>>> mapAsync) =>
        IsSuccess ? Value : await mapAsync(Error).ConfigureAwait(false);
}

/// <summary>
/// Provides extension methods for asynchronously mapping error operations on Task-wrapped Result instances.
/// </summary>
public static class MapErrorExtensions
{
    /// <summary>
    /// Asynchronously maps a synchronous error transformation function on a Task-wrapped void Result.
    /// </summary>
    /// <param name="result">The task containing the result to map errors on.</param>
    /// <param name="map">The function to apply to the error if the result is a failure.</param>
    /// <returns>A task containing the result of the error map function if failed, or the original successful result unchanged.</returns>
    public static async Task<Result> MapErrorAsync(
        this Task<Result> result, Func<Error, Result> map) =>
            (await result.ConfigureAwait(false)).MapError(map);

    /// <summary>
    /// Asynchronously maps an asynchronous error transformation function on a Task-wrapped void Result.
    /// </summary>
    /// <param name="result">The task containing the result to map errors on.</param>
    /// <param name="mapAsync">The asynchronous function to apply to the error if the result is a failure.</param>
    /// <returns>A task containing the result of the error map function if failed, or the original successful result unchanged.</returns>
    public static async Task<Result> MapErrorAsync(
        this Task<Result> result, Func<Error, Task<Result>> mapAsync) =>
            await (await result.ConfigureAwait(false)).MapErrorAsync(mapAsync).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously maps a synchronous error transformation function on a Task-wrapped Result with value preservation.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to map errors on.</param>
    /// <param name="map">The function to apply to the error if the result is a failure.</param>
    /// <returns>A task containing the result of the error map function if failed, or the original successful result unchanged.</returns>
    public static async Task<Result<T>> MapErrorAsync<T>(
        this Task<Result<T>> result, Func<Error, Result<T>> map) =>
            (await result.ConfigureAwait(false)).MapError(map);

    /// <summary>
    /// Asynchronously maps an asynchronous error transformation function on a Task-wrapped Result with value preservation.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to map errors on.</param>
    /// <param name="mapAsync">The asynchronous function to apply to the error if the result is a failure.</param>
    /// <returns>A task containing the result of the error map function if failed, or the original successful result unchanged.</returns>
    public static async Task<Result<T>> MapErrorAsync<T>(
        this Task<Result<T>> result, Func<Error, Task<Result<T>>> mapAsync) =>
            await (await result.ConfigureAwait(false)).MapErrorAsync(mapAsync).ConfigureAwait(false);
}