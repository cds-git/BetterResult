namespace BetterResult;

public readonly partial record struct Result<T>
{
    /// <summary>
    /// Runs <paramref name="bind"/> on the value and returns its result. A failure short-circuits and
    /// its error is passed on.
    /// </summary>
    public Result<U> Bind<U>(Func<T, Result<U>> bind) =>
        IsSuccess ? bind(Value) : Error;

    /// <inheritdoc cref="Bind{U}(Func{T, Result{U}})"/>
    public async Task<Result<U>> BindAsync<U>(Func<T, Task<Result<U>>> bindAsync) =>
        IsSuccess ? await bindAsync(Value).ConfigureAwait(false) : Error;

    /// <summary>
    /// Runs <paramref name="bind"/> on the value, then feeds both the value and the bound value to
    /// <paramref name="selector"/>, so a later step can use an earlier value without nesting. A failure
    /// on either side short circuits and its error is passed on.
    /// </summary>
    public Result<TResult> Bind<U, TResult>(Func<T, Result<U>> bind, Func<T, U, TResult> selector)
    {
        if (IsFailure) return Error;
        var value = Value;
        var bound = bind(value);
        if (bound.IsFailure) return bound.Error;
        return selector(value, bound.Value);
    }

    /// <inheritdoc cref="Bind{U, TResult}(Func{T, Result{U}}, Func{T, U, TResult})"/>
    public async Task<Result<TResult>> BindAsync<U, TResult>(Func<T, Task<Result<U>>> bindAsync, Func<T, U, TResult> selector)
    {
        if (IsFailure) return Error;
        var value = Value;
        var bound = await bindAsync(value).ConfigureAwait(false);
        if (bound.IsFailure) return bound.Error;
        return selector(value, bound.Value);
    }
}

/// <summary>
/// <see cref="Result{T}.Bind{U}"/> for results that are still inside a task.
/// </summary>
public static class BindExtensions
{
    /// <inheritdoc cref="Result{T}.Bind{U}(Func{T, Result{U}})"/>
    public static async Task<Result<U>> BindAsync<T, U>(
        this Task<Result<T>> result, Func<T, Result<U>> bind) =>
            (await result.ConfigureAwait(false)).Bind(bind);

    /// <inheritdoc cref="Result{T}.Bind{U}(Func{T, Result{U}})"/>
    public static async Task<Result<U>> BindAsync<T, U>(
        this Task<Result<T>> result, Func<T, Task<Result<U>>> bindAsync) =>
            await (await result.ConfigureAwait(false)).BindAsync(bindAsync).ConfigureAwait(false);

    /// <inheritdoc cref="Result{T}.Bind{U, TResult}(Func{T, Result{U}}, Func{T, U, TResult})"/>
    public static async Task<Result<TResult>> BindAsync<T, U, TResult>(
        this Task<Result<T>> result, Func<T, Result<U>> bind, Func<T, U, TResult> selector) =>
            (await result.ConfigureAwait(false)).Bind(bind, selector);

    /// <inheritdoc cref="Result{T}.Bind{U, TResult}(Func{T, Result{U}}, Func{T, U, TResult})"/>
    public static async Task<Result<TResult>> BindAsync<T, U, TResult>(
        this Task<Result<T>> result, Func<T, Task<Result<U>>> bindAsync, Func<T, U, TResult> selector) =>
            await (await result.ConfigureAwait(false)).BindAsync(bindAsync, selector).ConfigureAwait(false);
}
