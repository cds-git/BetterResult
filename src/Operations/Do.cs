namespace BetterResult;

public static partial class ResultExtensions
{
    /// <summary>
    /// Executes <paramref name="onSuccess"/> if <paramref name="result"/> is successful, then returns <paramref name="result"/>.
    /// </summary>
    /// <typeparam name="T">Type wrapped by the <see cref="Result{T}"/>.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">Action to invoke on success.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static Result<T> Do<T>(this Result<T> result, Action<T> onSuccess)
    {
        if (result.IsSuccess) onSuccess(result.Value);
        return result;
    }

    /// <summary>
    /// Executes <paramref name="onError"/> if <paramref name="result"/> is a failure, then returns <paramref name="result"/>.
    /// </summary>
    /// <typeparam name="T">Type wrapped by the <see cref="Result{T}"/>.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onError">Action to invoke on error.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static Result<T> Do<T>(this Result<T> result, Action<Error> onError)
    {
        if (result.IsFailure) onError(result.Error);
        return result;
    }

    /// <summary>
    /// Executes either <paramref name="onSuccess"/> or <paramref name="onError"/>, then returns <paramref name="result"/>.
    /// </summary>
    /// <typeparam name="T">Type wrapped by the <see cref="Result{T}"/>.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">Action to invoke on success.</param>
    /// <param name="onError">Action to invoke on error.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static Result<T> Do<T>(this Result<T> result, Action<T> onSuccess, Action<Error> onError)
    {
        if (result.IsSuccess) onSuccess(result.Value);
        if (result.IsFailure) onError(result.Error);
        return result;
    }

    /// <summary>
    /// Asynchronously executes <paramref name="onSuccess"/> if <paramref name="result"/> is successful, then returns <paramref name="result"/>.
    /// </summary>
    /// <typeparam name="T">Type wrapped by the <see cref="Result{T}"/>.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">Async action to invoke on success.</param>
    /// <returns>A task yielding the original <paramref name="result"/>.</returns>
    public static async Task<Result<T>> DoAsync<T>(this Result<T> result, Func<T, Task> onSuccess)
    {
        if (result.IsSuccess)
        {
            await onSuccess(result.Value).ConfigureAwait(false);
        }

        return result;
    }

    /// <summary>
    /// Asynchronously executes <paramref name="onError"/> if <paramref name="result"/> is a failure, then returns <paramref name="result"/>.
    /// </summary>
    /// <typeparam name="T">Type wrapped by the <see cref="Result{T}"/>.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onError">Async action to invoke on error.</param>
    /// <returns>A task yielding the original <paramref name="result"/>.</returns>
    public static async Task<Result<T>> DoAsync<T>(
        this Result<T> result, Func<Error, Task> onError)
    {
        if (result.IsFailure)
        {
            await onError(result.Error).ConfigureAwait(false);
        }

        return result;
    }

    /// <summary>
    /// Asynchronously executes either <paramref name="onSuccess"/> or <paramref name="onError"/>, then returns <paramref name="result"/>.
    /// </summary>
    /// <typeparam name="T">Type wrapped by the <see cref="Result{T}"/>.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">Async action to invoke on success.</param>
    /// <param name="onError">Async action to invoke on error.</param>
    /// <returns>A task yielding the original <paramref name="result"/>.</returns>
    public static async Task<Result<T>> DoAsync<T>(
        this Result<T> result, Func<T, Task> onSuccess, Func<Error, Task> onError)
    {
        if (result.IsSuccess)
        {
            await onSuccess(result.Value).ConfigureAwait(false);
        }
        else
        {
            await onError(result.Error).ConfigureAwait(false);
        }

        return result;
    }

    // ── Non-generic Result overloads ───────────────────────────────────

    /// <summary>
    /// Executes <paramref name="onSuccess"/> if <paramref name="result"/> is successful, then returns <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">Action to invoke on success.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static Result Do(this Result result, Action onSuccess)
    {
        if (result.IsSuccess) onSuccess();
        return result;
    }

    /// <summary>
    /// Executes <paramref name="onError"/> if <paramref name="result"/> is a failure, then returns <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="onError">Action to invoke on error.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static Result Do(this Result result, Action<Error> onError)
    {
        if (result.IsFailure) onError(result.Error);
        return result;
    }

    /// <summary>
    /// Executes either <paramref name="onSuccess"/> or <paramref name="onError"/>, then returns <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">Action to invoke on success.</param>
    /// <param name="onError">Action to invoke on error.</param>
    /// <returns>The original <paramref name="result"/>.</returns>
    public static Result Do(
        this Result result, Action onSuccess, Action<Error> onError)
    {
        if (result.IsSuccess) onSuccess();
        else onError(result.Error);
        return result;
    }

    /// <summary>
    /// Asynchronously executes <paramref name="onSuccess"/> if <paramref name="result"/> is successful, then returns <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">Async action to invoke on success.</param>
    /// <returns>A task yielding the original <paramref name="result"/>.</returns>
    public static async Task<Result> DoAsync(this Result result, Func<Task> onSuccess)
    {
        if (result.IsSuccess) await onSuccess().ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Asynchronously executes <paramref name="onError"/> if <paramref name="result"/> is a failure, then returns <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="onError">Async action to invoke on error.</param>
    /// <returns>A task yielding the original <paramref name="result"/>.</returns>
    public static async Task<Result> DoAsync(this Result result, Func<Error, Task> onError)
    {
        if (result.IsFailure) await onError(result.Error).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Asynchronously executes either <paramref name="onSuccess"/> or <paramref name="onError"/>, then returns <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">Async action to invoke on success.</param>
    /// <param name="onError">Async action to invoke on error.</param>
    /// <returns>A task yielding the original <paramref name="result"/>.</returns>
    public static async Task<Result> DoAsync(
        this Result result, Func<Task> onSuccess, Func<Error, Task> onError)
    {
        if (result.IsSuccess) await onSuccess().ConfigureAwait(false);
        else await onError(result.Error).ConfigureAwait(false);
        return result;
    }
}