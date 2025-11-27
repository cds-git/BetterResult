namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Validates the value in a successful result using a predicate.
    /// If the predicate returns false, the result becomes a failure with the specified error.
    /// If the current result is a failure, the error is propagated without executing the predicate.
    /// </summary>
    /// <param name="predicate">The validation predicate to apply to the value.</param>
    /// <param name="error">The error to return if the predicate fails.</param>
    /// <returns>The original result if the predicate succeeds, or a failure result if the predicate fails or the current result is already a failure.</returns>
    public Result<T> Ensure(Func<T, bool> predicate, Error error)
    {
        if (IsFailure) return Error;

        return predicate(Value) ? this : error;
    }

    /// <summary>
    /// Validates the value in a successful result using a predicate.
    /// If the predicate returns false, the result becomes a failure with an error created by the error factory.
    /// If the current result is a failure, the error is propagated without executing the predicate.
    /// </summary>
    /// <param name="predicate">The validation predicate to apply to the value.</param>
    /// <param name="errorFactory">A function that creates an error from the value when the predicate fails.</param>
    /// <returns>The original result if the predicate succeeds, or a failure result if the predicate fails or the current result is already a failure.</returns>
    public Result<T> Ensure(Func<T, bool> predicate, Func<T, Error> errorFactory)
    {
        if (IsFailure) return Error;

        return predicate(Value) ? this : errorFactory(Value);
    }

    /// <summary>
    /// Asynchronously validates the value in a successful result using a predicate.
    /// If the predicate returns false, the result becomes a failure with the specified error.
    /// If the current result is a failure, the error is propagated without executing the predicate.
    /// </summary>
    /// <param name="predicate">The asynchronous validation predicate to apply to the value.</param>
    /// <param name="error">The error to return if the predicate fails.</param>
    /// <returns>A task containing the original result if the predicate succeeds, or a failure result if the predicate fails or the current result is already a failure.</returns>
    public async Task<Result<T>> EnsureAsync(Func<T, Task<bool>> predicate, Error error)
    {
        if (IsFailure) return Error;

        return await predicate(Value).ConfigureAwait(false) ? this : error;
    }

    /// <summary>
    /// Asynchronously validates the value in a successful result using a predicate.
    /// If the predicate returns false, the result becomes a failure with an error created by the error factory.
    /// If the current result is a failure, the error is propagated without executing the predicate.
    /// </summary>
    /// <param name="predicate">The asynchronous validation predicate to apply to the value.</param>
    /// <param name="errorFactory">A function that creates an error from the value when the predicate fails.</param>
    /// <returns>A task containing the original result if the predicate succeeds, or a failure result if the predicate fails or the current result is already a failure.</returns>
    public async Task<Result<T>> EnsureAsync(Func<T, Task<bool>> predicate, Func<T, Error> errorFactory)
    {
        if (IsFailure) return Error;

        return await predicate(Value).ConfigureAwait(false) ? this : errorFactory(Value);
    }
}

/// <summary>
/// Provides extension methods for async validation operations on Task-wrapped Results.
/// </summary>
public static class EnsureExtensions
{
    /// <summary>
    /// Asynchronously validates a Task-wrapped Result with a synchronous predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to validate.</param>
    /// <param name="predicate">The validation predicate to apply to the value.</param>
    /// <param name="error">The error to return if the predicate fails.</param>
    /// <returns>A task containing the original result if the predicate succeeds, or a failure result if the predicate fails.</returns>
    public static async Task<Result<T>> EnsureAsync<T>(
        this Task<Result<T>> result, Func<T, bool> predicate, Error error) =>
            (await result.ConfigureAwait(false)).Ensure(predicate, error);

    /// <summary>
    /// Asynchronously validates a Task-wrapped Result with a synchronous predicate and error factory.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to validate.</param>
    /// <param name="predicate">The validation predicate to apply to the value.</param>
    /// <param name="errorFactory">A function that creates an error from the value when the predicate fails.</param>
    /// <returns>A task containing the original result if the predicate succeeds, or a failure result if the predicate fails.</returns>
    public static async Task<Result<T>> EnsureAsync<T>(
        this Task<Result<T>> result, Func<T, bool> predicate, Func<T, Error> errorFactory) =>
            (await result.ConfigureAwait(false)).Ensure(predicate, errorFactory);

    /// <summary>
    /// Asynchronously validates a Task-wrapped Result with an asynchronous predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to validate.</param>
    /// <param name="predicate">The asynchronous validation predicate to apply to the value.</param>
    /// <param name="error">The error to return if the predicate fails.</param>
    /// <returns>A task containing the original result if the predicate succeeds, or a failure result if the predicate fails.</returns>
    public static async Task<Result<T>> EnsureAsync<T>(
        this Task<Result<T>> result, Func<T, Task<bool>> predicate, Error error) =>
            await (await result.ConfigureAwait(false)).EnsureAsync(predicate, error).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously validates a Task-wrapped Result with an asynchronous predicate and error factory.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The task containing the result to validate.</param>
    /// <param name="predicate">The asynchronous validation predicate to apply to the value.</param>
    /// <param name="errorFactory">A function that creates an error from the value when the predicate fails.</param>
    /// <returns>A task containing the original result if the predicate succeeds, or a failure result if the predicate fails.</returns>
    public static async Task<Result<T>> EnsureAsync<T>(
        this Task<Result<T>> result, Func<T, Task<bool>> predicate, Func<T, Error> errorFactory) =>
            await (await result.ConfigureAwait(false)).EnsureAsync(predicate, errorFactory).ConfigureAwait(false);
}
