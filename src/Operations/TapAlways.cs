namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Executes a side effect action with the result on both the success and failure branches, then
    /// returns the original result unchanged. This method is useful for work that has to happen either
    /// way, such as stopping a timer or emitting a completion metric tagged with the outcome, without
    /// duplicating the closure across <see cref="Tap"/> and <see cref="TapError"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The action receives the result un-narrowed, so check <see cref="IsSuccess"/> or
    /// <see cref="IsFailure"/> before reading <see cref="Value"/> or <see cref="Error"/>: each throws
    /// on the branch it does not belong to.
    /// </para>
    /// <para>
    /// Exceptions thrown by <paramref name="onBoth"/> propagate to the caller, as with
    /// <see cref="Tap"/> and <see cref="TapError"/>. On the failure branch that replaces a handled
    /// <see cref="Error"/> with a thrown exception, so keep the action non-throwing.
    /// </para>
    /// </remarks>
    /// <param name="onBoth">The action to execute with the result, on both branches.</param>
    /// <returns>The original result, unchanged.</returns>
    public Result<T> TapAlways(Action<Result<T>> onBoth)
    {
        onBoth(this);
        return this;
    }

    /// <summary>
    /// Asynchronously executes a side effect action with the result on both the success and failure
    /// branches, then returns the original result unchanged.
    /// </summary>
    /// <remarks>See <see cref="TapAlways"/> for how the un-narrowed result and thrown exceptions behave.</remarks>
    /// <param name="onBoth">The asynchronous action to execute with the result, on both branches.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public async Task<Result<T>> TapAlwaysAsync(Func<Result<T>, Task> onBoth)
    {
        await onBoth(this).ConfigureAwait(false);
        return this;
    }
}

/// <summary>
/// Provides extension methods for tapping both branches (performing side effects on success and failure) of Task-wrapped Result instances.
/// </summary>
public static class TapAlwaysExtensions
{
    /// <summary>
    /// Asynchronously executes a synchronous side effect action on a Task-wrapped Result, on both branches.
    /// </summary>
    /// <remarks>
    /// The action runs only once the awaited task has produced a <see cref="Result{T}"/>. If the task
    /// faults or is cancelled, that exception propagates and the action does not run.
    /// </remarks>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to tap both branches of.</param>
    /// <param name="onBoth">The action to execute with the result, on both branches.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public static async Task<Result<T>> TapAlwaysAsync<T>(this Task<Result<T>> result, Action<Result<T>> onBoth) =>
        (await result.ConfigureAwait(false)).TapAlways(onBoth);

    /// <summary>
    /// Asynchronously executes an asynchronous side effect action on a Task-wrapped Result, on both branches.
    /// </summary>
    /// <remarks>
    /// The action runs only once the awaited task has produced a <see cref="Result{T}"/>. If the task
    /// faults or is cancelled, that exception propagates and the action does not run.
    /// </remarks>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to tap both branches of.</param>
    /// <param name="onBoth">The asynchronous action to execute with the result, on both branches.</param>
    /// <returns>A task containing the original result, unchanged.</returns>
    public static async Task<Result<T>> TapAlwaysAsync<T>(this Task<Result<T>> result, Func<Result<T>, Task> onBoth) =>
        await (await result.ConfigureAwait(false)).TapAlwaysAsync(onBoth).ConfigureAwait(false);
}
