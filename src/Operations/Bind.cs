namespace BetterResult;

public partial record Result<T>
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
}
