namespace BetterResult;

public static partial class ResultExtensions
{
    // ── Result<T> Map overloads ─────────────────────────

    /// <summary>
    /// Transforms a successful <see cref="Result{T1}"/> value into <typeparamref name="T2"/>,
    /// propagating the original error if not successful.
    /// </summary>
    /// <typeparam name="T1">Type wrapped by the source <see cref="Result{T1}"/>.</typeparam>
    /// <typeparam name="T2">Type wrapped by the resulting <see cref="Result{T2}"/>.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="map">Function to transform the value.</param>
    /// <returns>
    /// A successful <see cref="Result{T2}"/> containing the mapped value if <paramref name="result"/> is success;
    /// otherwise a failed <see cref="Result{T2}"/> with the original error.
    /// </returns>
    public static Result<T2> Map<T1, T2>(
        this Result<T1> result, Func<T1, T2> map) =>
            result.IsSuccess ? map(result.Value) : result.Error;

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/> then applies <see cref="Map{T1,T2}"/>.
    /// </summary>
    public static async Task<Result<T2>> MapAsync<T1, T2>(
        this Task<Result<T1>> result, Func<T1, T2> map) =>
            (await result.ConfigureAwait(false)).Map(map);

    /// <summary>
    /// Transforms a successful <see cref="Result{T1}"/> to <typeparamref name="T2"/> via an async mapper.
    /// </summary>
    public static async Task<Result<T2>> MapAsync<T1, T2>(
        this Result<T1> result, Func<T1, Task<T2>> mapAsync) =>
            result.IsSuccess ? await mapAsync(result.Value).ConfigureAwait(false) : result.Error;

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/> and then applies an async mapper.
    /// </summary>
    public static async Task<Result<T2>> MapAsync<T1, T2>(
        this Task<Result<T1>> result, Func<T1, Task<T2>> mapAsync) =>
            await (await result.ConfigureAwait(false)).MapAsync(mapAsync).ConfigureAwait(false);


    // ── Result Map overloads ─────────────────────────

    /// <summary>
    /// Projects a successful <see cref="Result"/> into a <see cref="Result{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type wrapped by the resulting <see cref="Result{T}"/>.</typeparam>
    /// <param name="result">The source non-generic result.</param>
    /// <param name="map">Function to produce a value on success.</param>
    /// <returns>
    /// A successful <see cref="Result{T}"/> containing the mapped value if <paramref name="result"/> is success;
    /// otherwise a failed <see cref="Result{T}"/> with the original error.
    /// </returns>
    public static Result<T> Map<T>(this Result result, Func<T> map) =>
        result.IsSuccess ? map() : result.Error;

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/> and then projects it into <see cref="Result{T}"/>.
    /// </summary>
    public static async Task<Result<T>> MapAsync<T>(this Task<Result> result, Func<T> map) =>
        (await result.ConfigureAwait(false)).Map(map);

    /// <summary>
    /// Projects a successful <see cref="Result"/> into <see cref="Result{T}"/> via an async mapper.
    /// </summary>
    public static async Task<Result<T>> MapAsync<T>(this Result result, Func<Task<T>> mapAsync) =>
        result.IsSuccess ? await mapAsync().ConfigureAwait(false) : result.Error;

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/> and then applies an async mapper into <see cref="Result{T}"/>.
    /// </summary>
    public static async Task<Result<T>> MapAsync<T>(this Task<Result> result, Func<Task<T>> mapAsync) =>
        await (await result.ConfigureAwait(false)).MapAsync(mapAsync).ConfigureAwait(false);
}