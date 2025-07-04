namespace BetterResult;

public static partial class ResultExtensions
{
    // ── Result<T> TapError overloads ───────────────────────────────────

    /// <summary>
    /// Performs the specified <paramref name="onError"/> action if the <paramref name="result"/> is a failure,
    /// then returns the original result unchanged.
    /// </summary>
    /// <typeparam name="T">Type of the result's value.</typeparam>
    /// <param name="result">The <see cref="Result{T}"/> to inspect.</param>
    /// <param name="onError">Action to perform on the error.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static Result<T> TapError<T>(this Result<T> result, Action<Error> onError)
    {
        if (result.IsFailure) onError(result.Error);
        return result;
    }

    /// <summary>
    /// Asynchronously performs the specified <paramref name="onError"/> function if the <paramref name="result"/> is a failure,
    /// then returns the original result unchanged.
    /// </summary>
    /// <typeparam name="T">Type of the result's value.</typeparam>
    /// <param name="result">The <see cref="Result{T}"/> to inspect.</param>
    /// <param name="onError">Async function to perform on the error.</param>
    /// <returns>A task that completes with the original <paramref name="result"/>.</returns>
    public static async Task<Result<T>> TapErrorAsync<T>(this Result<T> result, Func<Error, Task> onError)
    {
        if (result.IsFailure) await onError(result.Error).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Awaits the specified <paramref name="result"/>, then performs the <paramref name="onError"/> action if it's a failure.
    /// </summary>
    /// <typeparam name="T">Type of the result's value.</typeparam>
    /// <param name="result">A task returning a <see cref="Result{T}"/> to inspect.</param>
    /// <param name="onError">Action to perform on the error.</param>
    /// <returns>A task that completes with the original <see cref="Result{T}"/>.</returns>
    public static async Task<Result<T>> TapErrorAsync<T>(this Task<Result<T>> result, Action<Error> onError) =>
        (await result.ConfigureAwait(false)).TapError(onError);

    /// <summary>
    /// Awaits the specified <paramref name="result"/>, then asynchronously performs the <paramref name="onError"/> function if it's a failure.
    /// </summary>
    /// <typeparam name="T">Type of the result's value.</typeparam>
    /// <param name="result">A task returning a <see cref="Result{T}"/> to inspect.</param>
    /// <param name="onError">Async function to perform on the error.</param>
    /// <returns>A task that completes with the original <see cref="Result{T}"/>.</returns>
    public static async Task<Result<T>> TapErrorAsync<T>(this Task<Result<T>> result, Func<Error, Task> onError) =>
        await (await result.ConfigureAwait(false)).TapErrorAsync(onError).ConfigureAwait(false);


    // ── Result TapError overloads ───────────────────────────────────

    /// <summary>
    /// Performs the specified <paramref name="onError"/> action if the <paramref name="result"/> is a failure,
    /// then returns the original result unchanged.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> to inspect.</param>
    /// <param name="onError">The action to perform on the <see cref="Error"/>.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static Result TapError(this Result result, Action<Error> onError)
    {
        if (result.IsFailure) onError(result.Error);
        return result;
    }

    /// <summary>
    /// Asynchronously performs the specified <paramref name="onError"/> function if the <paramref name="result"/> is a failure,
    /// then returns the original result unchanged.
    /// </summary>
    /// <param name="result">The <see cref="Result"/> to inspect.</param>
    /// <param name="onError">The asynchronous function to perform on the <see cref="Error"/>.</param>
    /// <returns>
    /// A <see cref="Task{Result}"/> that completes with the original <paramref name="result"/>.
    /// </returns>
    public static async Task<Result> TapErrorAsync(this Result result, Func<Error, Task> onError)
    {
        if (result.IsFailure) await onError(result.Error).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Awaits the specified <paramref name="result"/>, then performs the <paramref name="onError"/> action if it is a failure,
    /// and returns the completed <see cref="Result"/>.
    /// </summary>
    /// <param name="result">A <see cref="Task{Result}"/> to await and inspect.</param>
    /// <param name="onError">The action to perform on the <see cref="Error"/>.</param>
    /// <returns>
    /// A <see cref="Task{Result}"/> that completes with the original <see cref="Result"/>.
    /// </returns>
    public static async Task<Result> TapErrorAsync(this Task<Result> result, Action<Error> onError) =>
        (await result.ConfigureAwait(false)).TapError(onError);

    /// <summary>
    /// Awaits the specified <paramref name="result"/>, then asynchronously performs the <paramref name="onError"/> function if it is a failure,
    /// and returns the completed <see cref="Result"/>.
    /// </summary>
    /// <param name="result">A <see cref="Task{Result}"/> to await and inspect.</param>
    /// <param name="onError">The asynchronous function to perform on the <see cref="Error"/>.</param>
    /// <returns>
    /// A <see cref="Task{Result}"/> that completes with the original <see cref="Result"/>.
    /// </returns>
    public static async Task<Result> TapErrorAsync(this Task<Result> result, Func<Error, Task> onError) =>
        await (await result.ConfigureAwait(false)).TapErrorAsync(onError).ConfigureAwait(false);
}