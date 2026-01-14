namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Attempts to recover from a failure by applying a recovery function if the error matches the specified type.
    /// If the current result is successful or the error type doesn't match, the result passes through unchanged.
    /// </summary>
    /// <param name="errorType">The error type to match for recovery.</param>
    /// <param name="recovery">The function to apply to the error to produce a recovery result.</param>
    /// <returns>The recovery result if the error type matches, or the original result unchanged.</returns>
    public Result<T> Recover(ErrorType errorType, Func<Error, Result<T>> recovery)
    {
        if (IsSuccess) return this;
        if (Error.Type != errorType) return this;
        return recovery(Error);
    }

    /// <summary>
    /// Attempts to recover from a failure by applying a recovery function if the error satisfies the predicate.
    /// If the current result is successful or the predicate returns false, the result passes through unchanged.
    /// </summary>
    /// <param name="predicate">The predicate to evaluate against the error.</param>
    /// <param name="recovery">The function to apply to the error to produce a recovery result.</param>
    /// <returns>The recovery result if the predicate is satisfied, or the original result unchanged.</returns>
    public Result<T> Recover(Func<Error, bool> predicate, Func<Error, Result<T>> recovery)
    {
        if (IsSuccess) return this;
        if (!predicate(Error)) return this;
        return recovery(Error);
    }

    /// <summary>
    /// Attempts to recover from a failure by providing a fallback value if the error matches the specified type.
    /// If the current result is successful or the error type doesn't match, the result passes through unchanged.
    /// </summary>
    /// <param name="errorType">The error type to match for recovery.</param>
    /// <param name="fallbackValue">The fallback value to use for recovery.</param>
    /// <returns>A successful result with the fallback value if the error type matches, or the original result unchanged.</returns>
    public Result<T> Recover(ErrorType errorType, T fallbackValue)
    {
        if (IsSuccess) return this;
        if (Error.Type != errorType) return this;
        return fallbackValue;
    }

    /// <summary>
    /// Asynchronously attempts to recover from a failure by applying a recovery function if the error matches the specified type.
    /// If the current result is successful or the error type doesn't match, the result passes through unchanged.
    /// </summary>
    /// <param name="errorType">The error type to match for recovery.</param>
    /// <param name="recoveryAsync">The asynchronous function to apply to the error to produce a recovery result.</param>
    /// <returns>A task containing the recovery result if the error type matches, or the original result unchanged.</returns>
    public async Task<Result<T>> RecoverAsync(ErrorType errorType, Func<Error, Task<Result<T>>> recoveryAsync)
    {
        if (IsSuccess) return this;
        if (Error.Type != errorType) return this;
        return await recoveryAsync(Error).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously attempts to recover from a failure by applying a recovery function if the error satisfies the predicate.
    /// If the current result is successful or the predicate returns false, the result passes through unchanged.
    /// </summary>
    /// <param name="predicate">The predicate to evaluate against the error.</param>
    /// <param name="recoveryAsync">The asynchronous function to apply to the error to produce a recovery result.</param>
    /// <returns>A task containing the recovery result if the predicate is satisfied, or the original result unchanged.</returns>
    public async Task<Result<T>> RecoverAsync(Func<Error, bool> predicate, Func<Error, Task<Result<T>>> recoveryAsync)
    {
        if (IsSuccess) return this;
        if (!predicate(Error)) return this;
        return await recoveryAsync(Error).ConfigureAwait(false);
    }
}

/// <summary>
/// Provides extension methods for recovering from errors on Task-wrapped Result instances.
/// </summary>
public static class RecoverExtensions
{
    /// <summary>
    /// Asynchronously recovers from a failure by applying a recovery function if the error matches the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="resultTask">The task containing the result to recover from.</param>
    /// <param name="errorType">The error type to match for recovery.</param>
    /// <param name="recovery">The function to apply to the error to produce a recovery result.</param>
    /// <returns>A task containing the recovery result if the error type matches, or the original result unchanged.</returns>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        ErrorType errorType,
        Func<Error, Result<T>> recovery)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Recover(errorType, recovery);
    }

    /// <summary>
    /// Asynchronously recovers from a failure by applying a recovery function if the error satisfies the predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="resultTask">The task containing the result to recover from.</param>
    /// <param name="predicate">The predicate to evaluate against the error.</param>
    /// <param name="recovery">The function to apply to the error to produce a recovery result.</param>
    /// <returns>A task containing the recovery result if the predicate is satisfied, or the original result unchanged.</returns>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        Func<Error, bool> predicate,
        Func<Error, Result<T>> recovery)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Recover(predicate, recovery);
    }

    /// <summary>
    /// Asynchronously recovers from a failure by providing a fallback value if the error matches the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="resultTask">The task containing the result to recover from.</param>
    /// <param name="errorType">The error type to match for recovery.</param>
    /// <param name="fallbackValue">The fallback value to use for recovery.</param>
    /// <returns>A task containing a successful result with the fallback value if the error type matches, or the original result unchanged.</returns>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        ErrorType errorType,
        T fallbackValue)
    {
        var result = await resultTask.ConfigureAwait(false);
        return result.Recover(errorType, fallbackValue);
    }

    /// <summary>
    /// Asynchronously recovers from a failure by applying an asynchronous recovery function if the error matches the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="resultTask">The task containing the result to recover from.</param>
    /// <param name="errorType">The error type to match for recovery.</param>
    /// <param name="recoveryAsync">The asynchronous function to apply to the error to produce a recovery result.</param>
    /// <returns>A task containing the recovery result if the error type matches, or the original result unchanged.</returns>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        ErrorType errorType,
        Func<Error, Task<Result<T>>> recoveryAsync)
    {
        var result = await resultTask.ConfigureAwait(false);
        return await result.RecoverAsync(errorType, recoveryAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously recovers from a failure by applying an asynchronous recovery function if the error satisfies the predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="resultTask">The task containing the result to recover from.</param>
    /// <param name="predicate">The predicate to evaluate against the error.</param>
    /// <param name="recoveryAsync">The asynchronous function to apply to the error to produce a recovery result.</param>
    /// <returns>A task containing the recovery result if the predicate is satisfied, or the original result unchanged.</returns>
    public static async Task<Result<T>> RecoverAsync<T>(
        this Task<Result<T>> resultTask,
        Func<Error, bool> predicate,
        Func<Error, Task<Result<T>>> recoveryAsync)
    {
        var result = await resultTask.ConfigureAwait(false);
        return await result.RecoverAsync(predicate, recoveryAsync).ConfigureAwait(false);
    }
}
