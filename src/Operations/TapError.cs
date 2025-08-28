namespace BetterResult;

public partial record Result
{
    /// <summary>
    /// Executes a side effect action with the error if the result is a failure, then returns the original result unchanged.
    /// This method is useful for performing operations like error logging or notifications without affecting the result.
    /// </summary>
    /// <param name="onFailure">The action to execute with the error if the result is a failure.</param>
    /// <returns>The original result, unchanged.</returns>
    public Result TapError(Action<Error> onFailure)
    {
        if (IsFailure) onFailure(Error);
        return this;
    }

    /// <summary>
    /// Asynchronously executes a side effect action with the error if the result is a failure, then returns the original result unchanged.
    /// This method is useful for performing asynchronous operations like error logging or notifications without affecting the result.
    /// </summary>
    /// <param name="onFailure">The asynchronous action to execute with the error if the result is a failure.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public async Task<Result> TapErrorAsync(Func<Error, Task> onFailure)
    {
        if (IsFailure) await onFailure(Error).ConfigureAwait(false);
        return this;
    }
}

public partial record Result<T>
{
    /// <summary>
    /// Executes a side effect action with the error if the result is a failure, then returns the original result unchanged.
    /// This method is useful for performing operations like error logging or notifications without affecting the result.
    /// </summary>
    /// <param name="onFailure">The action to execute with the error if the result is a failure.</param>
    /// <returns>The original result, unchanged.</returns>
    public Result<T> TapError(Action<Error> onFailure)
    {
        if (IsFailure) onFailure(Error);
        return this;
    }

    /// <summary>
    /// Asynchronously executes a side effect action with the error if the result is a failure, then returns the original result unchanged.
    /// This method is useful for performing asynchronous operations like error logging or notifications without affecting the result.
    /// </summary>
    /// <param name="onFailure">The asynchronous action to execute with the error if the result is a failure.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public async Task<Result<T>> TapErrorAsync(Func<Error, Task> onFailure)
    {
        if (IsFailure) await onFailure(Error).ConfigureAwait(false);
        return this;
    }
}

/// <summary>
/// Provides extension methods for asynchronously tapping error operations (performing side effects on failures) on Task-wrapped Result instances.
/// </summary>
public static class TapErrorExtensions
{
    /// <summary>
    /// Asynchronously executes a synchronous side effect action on a Task-wrapped void Result if it failed.
    /// </summary>
    /// <param name="result">The task containing the result to tap errors on.</param>
    /// <param name="onFailure">The action to execute with the error if the result is a failure.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public static async Task<Result> TapErrorAsync(this Task<Result> result, Action<Error> onFailure) =>
        (await result.ConfigureAwait(false)).TapError(onFailure);

    /// <summary>
    /// Asynchronously executes an asynchronous side effect action on a Task-wrapped void Result if it failed.
    /// </summary>
    /// <param name="result">The task containing the result to tap errors on.</param>
    /// <param name="onFailure">The asynchronous action to execute with the error if the result is a failure.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public static async Task<Result> TapErrorAsync(this Task<Result> result, Func<Error, Task> onFailure) =>
        await (await result.ConfigureAwait(false)).TapErrorAsync(onFailure).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously executes a synchronous side effect action on a Task-wrapped Result if it failed.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to tap errors on.</param>
    /// <param name="onFailure">The action to execute with the error if the result is a failure.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public static async Task<Result<T>> TapErrorAsync<T>(this Task<Result<T>> result, Action<Error> onFailure) =>
        (await result.ConfigureAwait(false)).TapError(onFailure);

    /// <summary>
    /// Asynchronously executes an asynchronous side effect action on a Task-wrapped Result if it failed.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to tap errors on.</param>
    /// <param name="onFailure">The asynchronous action to execute with the error if the result is a failure.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public static async Task<Result<T>> TapErrorAsync<T>(this Task<Result<T>> result, Func<Error, Task> onFailure) =>
        await (await result.ConfigureAwait(false)).TapErrorAsync(onFailure).ConfigureAwait(false);
}