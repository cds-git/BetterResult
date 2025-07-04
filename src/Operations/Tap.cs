namespace BetterResult;

public static partial class ResultExtensions
{
    // ── Result<T> Tap overloads ───────────────────────────────────

    /// <summary>
    /// Performs the specified <paramref name="onSuccess"/> action if the <paramref name="result"/> is successful,
    /// then returns the original result unchanged.
    /// </summary>
    /// <typeparam name="T">Type of the result's value.</typeparam>
    /// <param name="result">The <see cref="Result{T}"/> to inspect.</param>
    /// <param name="onSuccess">Action to perform on the successful value.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static Result<T> Tap<T>(this Result<T> result, Action<T> onSuccess)
    {
        if (result.IsSuccess) onSuccess(result.Value);
        return result;
    }

    /// <summary>
    /// Asynchronously performs the specified <paramref name="onSuccess"/> function if the <paramref name="result"/> is successful,
    /// then returns the original result unchanged.
    /// </summary>
    /// <typeparam name="T">Type of the result's value.</typeparam>
    /// <param name="result">The <see cref="Result{T}"/> to inspect.</param>
    /// <param name="onSuccess">Async function to perform on the successful value.</param>
    /// <returns>A task that completes with the original <paramref name="result"/>.</returns>
    public static async Task<Result<T>> TapAsync<T>(this Result<T> result, Func<T, Task> onSuccess)
    {
        if (result.IsSuccess) await onSuccess(result.Value).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Awaits the specified <paramref name="result"/>, then performs <paramref name="onSuccess"/> action if successful.
    /// </summary>
    /// <typeparam name="T">Type of the result's value.</typeparam>
    /// <param name="result">The task returning a <see cref="Result{T}"/> to inspect.</param>
    /// <param name="onSuccess">Action to perform on the successful value.</param>
    /// <returns>A task that completes with the original <see cref="Result{T}"/>.</returns>
    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> result, Action<T> onSuccess) =>
        (await result.ConfigureAwait(false)).Tap(onSuccess);

    /// <summary>
    /// Awaits the specified <paramref name="result"/>, then asynchronously performs <paramref name="onSuccess"/> if successful.
    /// </summary>
    /// <typeparam name="T">Type of the result's value.</typeparam>
    /// <param name="onSuccess">Async function to perform on the successful value.</param>
    /// <param name="result">The task returning a <see cref="Result{T}"/> to inspect.</param>
    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> result, Func<T, Task> onSuccess) =>
        await (await result.ConfigureAwait(false)).TapAsync(onSuccess).ConfigureAwait(false);


    // ── Result Tap overloads ───────────────────────────────────

    /// <summary>
    /// Performs the specified <paramref name="onSuccess"/> action if the <paramref name="result"/> is successful,
    /// then returns the original result unchanged.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> to inspect.</param>
    /// <param name="onSuccess">Action to perform on success.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static Result Tap(this Result result, Action onSuccess)
    {
        if (result.IsSuccess) onSuccess();
        return result;
    }

    /// <summary>
    /// Asynchronously performs the specified <paramref name="onSuccess"/> function if the <paramref name="result"/> is successful,
    /// then returns the original result unchanged.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> to inspect.</param>
    /// <param name="onSuccess">Async function to perform on success.</param>
    /// <returns>A task that completes with the original <paramref name="result"/>.</returns>
    public static async Task<Result> TapAsync(this Result result, Func<Task> onSuccess)
    {
        if (result.IsSuccess) await onSuccess().ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Awaits the specified <paramref name="result"/>, then performs <paramref name="onSuccess"/> action if successful.
    /// </summary>
    /// <param name="result">The task returning a <see cref="Result"/> to inspect.</param>
    /// <param name="onSuccess">Action to perform on success.</param>
    /// <returns>A task that completes with the original <see cref="Result"/>.</returns>
    public static async Task<Result> TapAsync(this Task<Result> result, Action onSuccess) =>
        (await result.ConfigureAwait(false)).Tap(onSuccess);

    /// <summary>
    /// Awaits the specified <paramref name="result"/>, then asynchronously performs <paramref name="onSuccess"/> if successful.
    /// </summary>
    /// <param name="result">The task returning a <see cref="Result"/> to inspect.</param>
    /// <param name="onSuccess">Async function to perform on success.</param>
    /// <returns>A task that completes with the original <see cref="Result"/>.</returns>
    public static async Task<Result> TapAsync(this Task<Result> result, Func<Task> onSuccess) =>
        await (await result.ConfigureAwait(false)).TapAsync(onSuccess).ConfigureAwait(false);
}