namespace BetterResult;

public partial record Result
{
    /// <summary>
    /// Executes a side effect action if the result is successful, then returns the original result unchanged.
    /// This method is useful for performing operations like logging or notifications without affecting the result.
    /// </summary>
    /// <param name="onSuccess">The action to execute if the result is successful.</param>
    /// <returns>The original result, unchanged.</returns>
    public Result Tap(Action onSuccess)
    {
        if (IsSuccess) onSuccess();
        return this;
    }

    /// <summary>
    /// Asynchronously executes a side effect action if the result is successful, then returns the original result unchanged.
    /// This method is useful for performing asynchronous operations like logging or notifications without affecting the result.
    /// </summary>
    /// <param name="onSuccess">The asynchronous action to execute if the result is successful.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public async Task<Result> TapAsync(Func<Task> onSuccess)
    {
        if (IsSuccess) await onSuccess().ConfigureAwait(false);
        return this;
    }
}

public partial record Result<T>
{
    /// <summary>
    /// Executes a side effect action with the result value if the result is successful, then returns the original result unchanged.
    /// This method is useful for performing operations like logging or notifications without affecting the result.
    /// </summary>
    /// <param name="onSuccess">The action to execute with the result value if the result is successful.</param>
    /// <returns>The original result, unchanged.</returns>
    public Result<T> Tap(Action<T> onSuccess)
    {
        if (IsSuccess) onSuccess(Value);
        return this;
    }

    /// <summary>
    /// Asynchronously executes a side effect action with the result value if the result is successful, then returns the original result unchanged.
    /// This method is useful for performing asynchronous operations like logging or notifications without affecting the result.
    /// </summary>
    /// <param name="onSuccess">The asynchronous action to execute with the result value if the result is successful.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public async Task<Result<T>> TapAsync(Func<T, Task> onSuccess)
    {
        if (IsSuccess) await onSuccess(Value).ConfigureAwait(false);
        return this;
    }
}

/// <summary>
/// Provides extension methods for asynchronously tapping (performing side effects) on Task-wrapped Result instances.
/// </summary>
public static class TapExtensions
{
    /// <summary>
    /// Asynchronously executes a synchronous side effect action on a Task-wrapped void Result if successful.
    /// </summary>
    /// <param name="result">The task containing the result to tap on.</param>
    /// <param name="onSuccess">The action to execute if the result is successful.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public static async Task<Result> TapAsync(this Task<Result> result, Action onSuccess) =>
        (await result.ConfigureAwait(false)).Tap(onSuccess);

    /// <summary>
    /// Asynchronously executes an asynchronous side effect action on a Task-wrapped void Result if successful.
    /// </summary>
    /// <param name="result">The task containing the result to tap on.</param>
    /// <param name="onSuccess">The asynchronous action to execute if the result is successful.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public static async Task<Result> TapAsync(this Task<Result> result, Func<Task> onSuccess) =>
        await (await result.ConfigureAwait(false)).TapAsync(onSuccess).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously executes a synchronous side effect action on a Task-wrapped Result if successful.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to tap on.</param>
    /// <param name="onSuccess">The action to execute with the result value if the result is successful.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> result, Action<T> onSuccess) =>
        (await result.ConfigureAwait(false)).Tap(onSuccess);

    /// <summary>
    /// Asynchronously executes an asynchronous side effect action on a Task-wrapped Result if successful.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to tap on.</param>
    /// <param name="onSuccess">The asynchronous action to execute with the result value if the result is successful.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> result, Func<T, Task> onSuccess) =>
        await (await result.ConfigureAwait(false)).TapAsync(onSuccess).ConfigureAwait(false);
}