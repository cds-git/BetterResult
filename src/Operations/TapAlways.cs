namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Runs <paramref name="onBoth"/> on either branch, then returns the result unchanged. Use it for
    /// work that has to happen either way, such as stopping a timer or emitting a completion metric,
    /// instead of duplicating the closure across <see cref="Tap"/> and <see cref="TapError"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The action gets the whole result, so check <see cref="IsSuccess"/> or <see cref="IsFailure"/>
    /// before reading <see cref="Value"/> or <see cref="Error"/>. Each throws on the wrong branch.
    /// </para>
    /// <para>
    /// The action only runs once a result exists. If an earlier step throws, the exception unwinds past
    /// <c>TapAlways</c> and the action is skipped, so use <c>using</c> or <c>try</c>/<c>finally</c> for
    /// resource cleanup.
    /// </para>
    /// <para>
    /// Exceptions from <paramref name="onBoth"/> propagate, same as <see cref="Tap"/> and
    /// <see cref="TapError"/>. On the failure branch that swaps a handled <see cref="Error"/> for a
    /// thrown exception, so keep the action non-throwing.
    /// </para>
    /// </remarks>
    public Result<T> TapAlways(Action<Result<T>> onBoth)
    {
        onBoth(this);
        return this;
    }

    /// <inheritdoc cref="TapAlways(Action{Result{T}})"/>
    public async Task<Result<T>> TapAlwaysAsync(Func<Result<T>, Task> onBoth)
    {
        await onBoth(this).ConfigureAwait(false);
        return this;
    }
}

/// <summary>
/// <see cref="Result{T}.TapAlways"/> for results that are still inside a task.
/// </summary>
public static class TapAlwaysExtensions
{
    /// <inheritdoc cref="Result{T}.TapAlways(Action{Result{T}})"/>
    public static async Task<Result<T>> TapAlwaysAsync<T>(this Task<Result<T>> result, Action<Result<T>> onBoth) =>
        (await result.ConfigureAwait(false)).TapAlways(onBoth);

    /// <inheritdoc cref="Result{T}.TapAlways(Action{Result{T}})"/>
    public static async Task<Result<T>> TapAlwaysAsync<T>(this Task<Result<T>> result, Func<Result<T>, Task> onBoth) =>
        await (await result.ConfigureAwait(false)).TapAlwaysAsync(onBoth).ConfigureAwait(false);
}
