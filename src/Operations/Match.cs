namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Unwraps the result by running <paramref name="onSuccess"/> or <paramref name="onFailure"/>.
    /// Both must return the same type, and exactly one of them runs.
    /// </summary>
    public U Match<U>(Func<T, U> onSuccess, Func<Error, U> onFailure) =>
        IsSuccess ? onSuccess(Value) : onFailure(Error);

    /// <inheritdoc cref="Match{U}(Func{T, U}, Func{Error, U})"/>
    public async Task<U> MatchAsync<U>(Func<T, Task<U>> onSuccessAsync, Func<Error, U> onFailure) =>
        IsSuccess ? await onSuccessAsync(Value).ConfigureAwait(false) : onFailure(Error);

    /// <inheritdoc cref="Match{U}(Func{T, U}, Func{Error, U})"/>
    public async Task<U> MatchAsync<U>(Func<T, U> onSuccess, Func<Error, Task<U>> onFailureAsync) =>
        IsSuccess ? onSuccess(Value) : await onFailureAsync(Error).ConfigureAwait(false);

    /// <inheritdoc cref="Match{U}(Func{T, U}, Func{Error, U})"/>
    public async Task<U> MatchAsync<U>(Func<T, Task<U>> onSuccessAsync, Func<Error, Task<U>> onFailureAsync) =>
        IsSuccess ? await onSuccessAsync(Value).ConfigureAwait(false) : await onFailureAsync(Error).ConfigureAwait(false);
}

/// <summary>
/// <see cref="Result{T}.Match{U}"/> for results that are still inside a task.
/// </summary>
public static class MatchExtensions
{
    /// <inheritdoc cref="Result{T}.Match{U}(Func{T, U}, Func{Error, U})"/>
    public static async Task<U> MatchAsync<T, U>(
       this Task<Result<T>> result, Func<T, U> onSuccess, Func<Error, U> onFailure) =>
           (await result.ConfigureAwait(false)).Match(onSuccess, onFailure);

    /// <inheritdoc cref="Result{T}.Match{U}(Func{T, U}, Func{Error, U})"/>
    public static async Task<U> MatchAsync<T, U>(
       this Task<Result<T>> result, Func<T, Task<U>> onSuccessAsync, Func<Error, U> onFailure) =>
           await (await result.ConfigureAwait(false)).MatchAsync(onSuccessAsync, onFailure).ConfigureAwait(false);

    /// <inheritdoc cref="Result{T}.Match{U}(Func{T, U}, Func{Error, U})"/>
    public static async Task<U> MatchAsync<T, U>(
       this Task<Result<T>> result, Func<T, U> onSuccess, Func<Error, Task<U>> onFailureAsync) =>
           await (await result.ConfigureAwait(false)).MatchAsync(onSuccess, onFailureAsync).ConfigureAwait(false);

    /// <inheritdoc cref="Result{T}.Match{U}(Func{T, U}, Func{Error, U})"/>
    public static async Task<U> MatchAsync<T, U>(
       this Task<Result<T>> result, Func<T, Task<U>> onSuccessAsync, Func<Error, Task<U>> onFailureAsync) =>
           await (await result.ConfigureAwait(false)).MatchAsync(onSuccessAsync, onFailureAsync).ConfigureAwait(false);
}
