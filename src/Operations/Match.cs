namespace BetterResult;

public static partial class ResultExtensions
{
    /// <summary>
    /// Maps a successful <see cref="Result{TValue}"/> to <typeparamref name="TResult"/>,
    /// or maps its <see cref="Error"/> on failure.
    /// </summary>
    /// <typeparam name="TValue">Type wrapped by the source <see cref="Result{TValue}"/>.</typeparam>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">The source <see cref="Result{TValue}"/> to match.</param>
    /// <param name="mapValue">Function to map the successful value.</param>
    /// <param name="mapError">Function to map the <see cref="Error"/> on failure.</param>
    /// <returns>
    /// The result of <paramref name="mapValue"/> if <paramref name="result"/> is successful;
    /// otherwise the result of <paramref name="mapError"/>.
    /// </returns>
    public static TResult Match<TValue, TResult>(
        this Result<TValue> result, Func<TValue, TResult> mapValue, Func<Error, TResult> mapError) =>
            result.IsSuccess ? mapValue(result.Value) : mapError(result.Error);

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/>, then applies <see cref="Match{TValue,TResult}"/>.
    /// </summary>
    /// <typeparam name="TValue">Type wrapped by the source <see cref="Result{TValue}"/>.</typeparam>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">Task producing the source <see cref="Result{TValue}"/>.</param>
    /// <param name="mapValue">Function to map the successful value.</param>
    /// <param name="mapError">Function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.</returns>
    public static async Task<TResult> MatchAsync<TValue, TResult>(
        this Task<Result<TValue>> result, Func<TValue, TResult> mapValue, Func<Error, TResult> mapError) =>
            (await result.ConfigureAwait(false)).Match(mapValue, mapError);

    /// <summary>
    /// Maps using an async <paramref name="mapValueAsync"/>, or a sync <paramref name="mapError"/> on failure.
    /// </summary>
    /// <typeparam name="TValue">Type wrapped by the source <see cref="Result{TValue}"/>.</typeparam>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">The source <see cref="Result{TValue}"/> to match.</param>
    /// <param name="mapValueAsync">Async function to map the successful value.</param>
    /// <param name="mapError">Function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.</returns>
    public static async Task<TResult> MatchAsync<TValue, TResult>(
        this Result<TValue> result, Func<TValue, Task<TResult>> mapValueAsync, Func<Error, TResult> mapError) =>
            result.IsSuccess ? await mapValueAsync(result.Value).ConfigureAwait(false) : mapError(result.Error);

    /// <summary>
    /// Maps using a sync <paramref name="mapValue"/>, or an async <paramref name="mapErrorAsync"/> on failure.
    /// </summary>
    /// <typeparam name="TValue">Type wrapped by the source <see cref="Result{TValue}"/>.</typeparam>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">The source <see cref="Result{TValue}"/> to match.</param>
    /// <param name="mapValue">Function to map the successful value.</param>
    /// <param name="mapErrorAsync">Async function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.</returns>
    public static async Task<TResult> MatchAsync<TValue, TResult>(
        this Result<TValue> result, Func<TValue, TResult> mapValue, Func<Error, Task<TResult>> mapErrorAsync) =>
            result.IsSuccess ? mapValue(result.Value) : await mapErrorAsync(result.Error).ConfigureAwait(false);

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/>, then applies an async <paramref name="mapValueAsync"/> and sync <paramref name="mapError"/>.
    /// </summary>
    /// <typeparam name="TValue">Type wrapped by the source <see cref="Result{TValue}"/>.</typeparam>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">Task producing the source <see cref="Result{TValue}"/>.</param>
    /// <param name="mapValueAsync">Async function to map the successful value.</param>
    /// <param name="mapError">Function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.</returns>
    public static async Task<TResult> MatchAsync<TValue, TResult>(
        this Task<Result<TValue>> result, Func<TValue, Task<TResult>> mapValueAsync, Func<Error, TResult> mapError) =>
            await (await result.ConfigureAwait(false)).MatchAsync(mapValueAsync, mapError).ConfigureAwait(false);

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/>, then applies sync <paramref name="mapValue"/> and async <paramref name="mapErrorAsync"/>.
    /// </summary>
    /// <typeparam name="TValue">Type wrapped by the source <see cref="Result{TValue}"/>.</typeparam>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">Task producing the source <see cref="Result{TValue}"/>.</param>
    /// <param name="mapValue">Function to map the successful value.</param>
    /// <param name="mapErrorAsync">Async function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.
    /// </returns>
    public static async Task<TResult> MatchAsync<TValue, TResult>(
        this Task<Result<TValue>> result, Func<TValue, TResult> mapValue, Func<Error, Task<TResult>> mapErrorAsync) =>
            await (await result.ConfigureAwait(false)).MatchAsync(mapValue, mapErrorAsync).ConfigureAwait(false);

    /// <summary>
    /// Maps using async <paramref name="mapValueAsync"/> and async <paramref name="mapErrorAsync"/>.
    /// </summary>
    /// <typeparam name="TValue">Type wrapped by the source <see cref="Result{TValue}"/>.</typeparam>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">The source <see cref="Result{TValue}"/> to match.</param>
    /// <param name="mapValueAsync">Async function to map the successful value.</param>
    /// <param name="mapErrorAsync">Async function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.</returns>
    public static async Task<TResult> MatchAsync<TValue, TResult>(
        this Result<TValue> result, Func<TValue, Task<TResult>> mapValueAsync, Func<Error, Task<TResult>> mapErrorAsync) =>
            result.IsSuccess ? await mapValueAsync(result.Value).ConfigureAwait(false) : await mapErrorAsync(result.Error).ConfigureAwait(false);

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/>, then applies async <paramref name="mapValueAsync"/> and async <paramref name="mapErrorAsync"/>.
    /// </summary>
    /// <typeparam name="TValue">Type wrapped by the source <see cref="Result{TValue}"/>.</typeparam>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">Task producing the source <see cref="Result{TValue}"/>.</param>
    /// <param name="mapValueAsync">Async function to map the successful value.</param>
    /// <param name="mapErrorAsync">Async function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.</returns>
    public static async Task<TResult> MatchAsync<TValue, TResult>(
        this Task<Result<TValue>> result, Func<TValue, Task<TResult>> mapValueAsync, Func<Error, Task<TResult>> mapErrorAsync) =>
            await (await result.ConfigureAwait(false)).MatchAsync(mapValueAsync, mapErrorAsync).ConfigureAwait(false);


    // Non-generic Result overloads

    /// <summary>
    /// Maps a non-generic <see cref="Result"/> to <typeparamref name="TResult"/>,
    /// or maps its <see cref="Error"/> on failure.
    /// </summary>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">The source <see cref="Result"/> to match.</param>
    /// <param name="mapValue">Function to map success (no value).</param>
    /// <param name="mapError">Function to map the <see cref="Error"/> on failure.</param>
    /// <returns>
    /// The result of <paramref name="mapValue"/> if <paramref name="result"/> is successful;
    /// otherwise the result of <paramref name="mapError"/>.
    /// </returns>
    public static TResult Match<TResult>(
        this Result result, Func<TResult> mapValue, Func<Error, TResult> mapError) =>
            result.IsSuccess ? mapValue() : mapError(result.Error);

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/>, then applies <see cref="Match{TResult}"/>.
    /// </summary>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">Task producing the source <see cref="Result"/>.</param>
    /// <param name="mapValue">Function to map success (no value).</param>
    /// <param name="mapError">Function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.</returns>
    public static async Task<TResult> MatchAsync<TResult>(
        this Task<Result> result, Func<TResult> mapValue, Func<Error, TResult> mapError) =>
            (await result.ConfigureAwait(false)).Match(mapValue, mapError);

    /// <summary>
    /// Maps using an async <paramref name="mapValueAsync"/>, or a sync <paramref name="mapError"/> on failure.
    /// </summary>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">The source <see cref="Result"/> to match.</param>
    /// <param name="mapValueAsync">Async function to map success (no value).</param>
    /// <param name="mapError">Function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.</returns>
    public static async Task<TResult> MatchAsync<TResult>(
        this Result result, Func<Task<TResult>> mapValueAsync, Func<Error, TResult> mapError) =>
            result.IsSuccess ? await mapValueAsync().ConfigureAwait(false) : mapError(result.Error);

    /// <summary>
    /// Maps using a sync <paramref name="mapValue"/>, or an async <paramref name="mapErrorAsync"/> on failure.
    /// </summary>
    /// <summary>
    /// Maps using a sync <paramref name="mapValue"/>, or an async <paramref name="mapErrorAsync"/> on failure.
    /// </summary>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">The source <see cref="Result"/> to match.</param>
    /// <param name="mapValue">Function to map success (no value).</param>
    /// <param name="mapErrorAsync">Async function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.</returns>
    public static async Task<TResult> MatchAsync<TResult>(
        this Result result, Func<TResult> mapValue, Func<Error, Task<TResult>> mapErrorAsync) =>
            result.IsSuccess ? mapValue() : await mapErrorAsync(result.Error).ConfigureAwait(false);

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/>, then applies sync <paramref name="mapValue"/> and async <paramref name="mapErrorAsync"/>.
    /// </summary>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">Task producing the source <see cref="Result"/>.</param>
    /// <param name="mapValue">Function to map success (no value).</param>
    /// <param name="mapErrorAsync">Async function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.</returns>
    public static async Task<TResult> MatchAsync<TResult>(
        this Task<Result> result, Func<TResult> mapValue, Func<Error, Task<TResult>> mapErrorAsync) =>
            await (await result.ConfigureAwait(false)).MatchAsync(mapValue, mapErrorAsync).ConfigureAwait(false);

    /// <summary>
    /// Maps using async <paramref name="mapValueAsync"/>, or async <paramref name="mapErrorAsync"/> on failure.
    /// </summary>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">The source <see cref="Result"/> to match.</param>
    /// <param name="mapValueAsync">Async function to map success (no value).</param>
    /// <param name="mapErrorAsync">Async function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.</returns>
    public static async Task<TResult> MatchAsync<TResult>(
        this Result result, Func<Task<TResult>> mapValueAsync, Func<Error, Task<TResult>> mapErrorAsync) =>
            result.IsSuccess ? await mapValueAsync().ConfigureAwait(false) : await mapErrorAsync(result.Error).ConfigureAwait(false);

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/>, then applies async <paramref name="mapValueAsync"/> and async <paramref name="mapErrorAsync"/>.
    /// </summary>
    /// <typeparam name="TResult">Type returned by the mapping functions.</typeparam>
    /// <param name="result">Task producing the source <see cref="Result"/>.</param>
    /// <param name="mapValueAsync">Async function to map success (no value).</param>
    /// <param name="mapErrorAsync">Async function to map the <see cref="Error"/> on failure.</param>
    /// <returns>A task yielding the mapped <typeparamref name="TResult"/>.</returns>
    public static async Task<TResult> MatchAsync<TResult>(
        this Task<Result> result, Func<Task<TResult>> mapValueAsync, Func<Error, Task<TResult>> mapErrorAsync) =>
            await (await result.ConfigureAwait(false)).MatchAsync(mapValueAsync, mapErrorAsync).ConfigureAwait(false);
}