namespace BetterResult;

public static partial class ResultExtensions
{
    // ── Result<T> MapError overloads ───────────────────────────────────

    /// <summary>
    /// Transforms a failed <see cref="Result{T}"/> by mapping its <see cref="Error"/> into another <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type wrapped by the <see cref="Result{T}"/>.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="map">Function to recover from an error.</param>
    /// <returns>
    /// The original successful result, or the <see cref="Result{T}"/> returned by <paramref name="map"/>.
    /// </returns>
    public static Result<T> MapError<T>(
        this Result<T> result, Func<Error, Result<T>> map) =>
            result.IsSuccess ? result.Value : map(result.Error);

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/> then applies <see cref="MapError{T}"/>.
    /// </summary>
    public static async Task<Result<T>> MapErrorAsync<T>(
        this Task<Result<T>> result, Func<Error, Result<T>> map) =>
            (await result.ConfigureAwait(false)).MapError(map);

    /// <summary>
    /// Transforms a failed <see cref="Result{T}"/> via an async mapper.
    /// </summary>
    public static async Task<Result<T>> MapErrorAsync<T>(
        this Result<T> result, Func<Error, Task<Result<T>>> mapAsync) =>
            result.IsSuccess ? result.Value : await mapAsync(result.Error).ConfigureAwait(false);

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/> then applies an async error mapper.
    /// </summary>
    public static async Task<Result<T>> MapErrorAsync<T>(
        this Task<Result<T>> result, Func<Error, Task<Result<T>>> mapAsync) =>
            await (await result.ConfigureAwait(false)).MapErrorAsync(mapAsync).ConfigureAwait(false);

    // ── Result MapError overloads ───────────────────────────────────

    /// <summary>
    /// Transforms a failed <see cref="Result"/> by mapping its <see cref="Error"/> into another <see cref="Result"/>.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="map">Function to recover from an error.</param>
    /// <returns>
    /// The original successful result, or the <see cref="Result"/> returned by <paramref name="map"/>.
    /// </returns>
    public static Result MapError(
        this Result result, Func<Error, Result> map) =>
            result.IsSuccess ? result : map(result.Error);

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/> then applies <see cref="MapError"/>.
    /// </summary>
    public static async Task<Result> MapErrorAsync(
        this Task<Result> result, Func<Error, Result> map) =>
            (await result.ConfigureAwait(false)).MapError(map);

    /// <summary>
    /// Transforms a failed <see cref="Result"/> via an async mapper.
    /// </summary>
    public static async Task<Result> MapErrorAsync(
        this Result result, Func<Error, Task<Result>> mapAsync) =>
            result.IsSuccess ? result : await mapAsync(result.Error).ConfigureAwait(false);

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/> then applies an async error mapper.
    /// </summary>
    public static async Task<Result> MapErrorAsync(
        this Task<Result> result, Func<Error, Task<Result>> mapAsync) =>
            await (await result.ConfigureAwait(false)).MapErrorAsync(mapAsync).ConfigureAwait(false);
}