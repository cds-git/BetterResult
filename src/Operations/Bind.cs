namespace BetterResult;

public static partial class ResultExtensions
{
    // ── Result<T> Bind overloads ───────────────────────────────────

    /// <summary>
    /// Chains a successful <see cref="Result{T1}"/> into a <see cref="Result{T2}"/>.
    /// If <paramref name="result"/> is a success, invokes <paramref name="bind"/> with its value; otherwise propagates the original error.
    /// </summary>
    /// <typeparam name="T1">Type wrapped by the source <see cref="Result{T1}"/>.</typeparam>
    /// <typeparam name="T2">Type wrapped by the resulting <see cref="Result{T2}"/>.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="bind">Function to produce the next result.</param>
    /// <returns>
    /// The <see cref="Result{T2}"/> returned by <paramref name="bind"/> if <paramref name="result"/> is successful;
    /// otherwise a failed <see cref="Result{T2}"/> carrying the original error.
    /// </returns>
    public static Result<T2> Bind<T1, T2>(
        this Result<T1> result, Func<T1, Result<T2>> bind) =>
            result.IsSuccess ? bind(result.Value) : result.Error;

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/> and then chains it into a <see cref="Result{T2}"/>.
    /// </summary>
    /// <typeparam name="T1">Type wrapped by the source <see cref="Result{T1}"/>.</typeparam>
    /// <typeparam name="T2">Type wrapped by the resulting <see cref="Result{T2}"/>.</typeparam>
    /// <param name="result">A task that produces the source result.</param>
    /// <param name="bind">Function to produce the next result.</param>
    /// <returns>A task that produces the chained <see cref="Result{T2}"/>.</returns>
    public static async Task<Result<T2>> BindAsync<T1, T2>(
        this Task<Result<T1>> result, Func<T1, Result<T2>> bind) =>
            (await result.ConfigureAwait(false)).Bind(bind);

    /// <summary>
    /// Chains a successful <see cref="Result{T1}"/> into a <see cref="Result{T2}"/> via an async binder.
    /// </summary>
    /// <typeparam name="T1">Type wrapped by the source <see cref="Result{T1}"/>.</typeparam>
    /// <typeparam name="T2">Type wrapped by the resulting <see cref="Result{T2}"/>.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="bindAsync">Async function to produce the next result.</param>
    /// <returns>A task that produces the chained <see cref="Result{T2}"/>.</returns>
    public static async Task<Result<T2>> BindAsync<T1, T2>(
        this Result<T1> result, Func<T1, Task<Result<T2>>> bindAsync) =>
            result.IsSuccess ? await bindAsync(result.Value).ConfigureAwait(false) : result.Error;

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/> and then chains it via an async binder into a <see cref="Result{T2}"/>.
    /// </summary>
    /// <typeparam name="T1">Type wrapped by the source <see cref="Result{T1}"/>.</typeparam>
    /// <typeparam name="T2">Type wrapped by the resulting <see cref="Result{T2}"/>.</typeparam>
    /// <param name="result">A task that produces the source result.</param>
    /// <param name="bindAsync">Async function to produce the next result.</param>
    /// <returns>A task that produces the chained <see cref="Result{T2}"/>.</returns>
    public static async Task<Result<T2>> BindAsync<T1, T2>(
        this Task<Result<T1>> result, Func<T1, Task<Result<T2>>> bindAsync) =>
            await (await result.ConfigureAwait(false)).BindAsync(bindAsync).ConfigureAwait(false);

    // ── Result Bind overloads ───────────────────────────────────

    /// <summary>
    /// Chains a successful <see cref="Result"/> into another <see cref="Result"/>.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="bind">Function to produce the next result.</param>
    /// <returns>
    /// The <see cref="Result"/> returned by <paramref name="bind"/> if <paramref name="result"/> is successful;
    /// otherwise the original failed <see cref="Result"/>.
    /// </returns>
    public static Result Bind(
        this Result result, Func<Result> bind) =>
            result.IsSuccess ? bind() : result;

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/> and then chains it into another <see cref="Result"/>.
    /// </summary>
    /// <param name="result">A task that produces the source result.</param>
    /// <param name="bind">Function to produce the next result.</param>
    /// <returns>A task that produces the chained <see cref="Result"/>.</returns>
    public static async Task<Result> BindAsync(
        this Task<Result> result, Func<Result> bind) =>
            (await result.ConfigureAwait(false)).Bind(bind);

    /// <summary>
    /// Chains a successful <see cref="Result"/> into another <see cref="Result"/> via an async binder.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="bindAsync">Async function to produce the next result.</param>
    /// <returns>A task that produces the chained <see cref="Result"/>.</returns>
    public static async Task<Result> BindAsync(
        this Result result, Func<Task<Result>> bindAsync) =>
            result.IsSuccess ? await bindAsync().ConfigureAwait(false) : result;

    /// <summary>
    /// Awaits a <see cref="Task{Result}"/> and then chains it via an async binder into another <see cref="Result"/>.
    /// </summary>
    /// <param name="result">A task that produces the source result.</param>
    /// <param name="bindAsync">Async function to produce the next result.</param>
    /// <returns>A task that produces the chained <see cref="Result"/>.</returns>
    public static async Task<Result> BindAsync(
        this Task<Result> result, Func<Task<Result>> bindAsync) =>
            await (await result.ConfigureAwait(false)).BindAsync(bindAsync).ConfigureAwait(false);
}