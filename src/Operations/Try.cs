namespace BetterResult;

public partial record Result<T>
{
    /// <summary>
    /// Transforms the value in a successful result by applying an operation that may throw an exception.
    /// If the current result is a failure, the error is propagated without executing the operation.
    /// If an exception is thrown during the operation, returns a failure result with an Unexpected error.
    /// </summary>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="operation">The operation to apply to the current value if the result is successful.</param>
    /// <returns>A result containing the new value if successful, or an error if the operation throws or the current result is failed.</returns>
    public Result<U> Try<U>(Func<T, U> operation)
    {
        if (IsFailure) return Error;

        try
        {
            return operation(Value);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return Error.Unexpected("EXCEPTION", ex.Message)
                .WithMetadata("ExceptionType", ex.GetType().Name);
        }
    }

    /// <summary>
    /// Transforms the value in a successful result by applying an operation that may throw an exception, using a custom error mapper.
    /// If the current result is a failure, the error is propagated without executing the operation.
    /// If an exception is thrown during the operation, the error mapper is called to create the error.
    /// </summary>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="operation">The operation to apply to the current value if the result is successful.</param>
    /// <param name="errorMapper">A function to convert the exception to an Error.</param>
    /// <returns>A result containing the new value if successful, or an error if the operation throws or the current result is failed.</returns>
    public Result<U> Try<U>(Func<T, U> operation, Func<Exception, Error> errorMapper)
    {
        if (IsFailure) return Error;

        try
        {
            return operation(Value);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return errorMapper(ex);
        }
    }

    /// <summary>
    /// Asynchronously transforms the value in a successful result by applying an operation that may throw an exception.
    /// If the current result is a failure, the error is propagated without executing the operation.
    /// If an exception is thrown during the operation, returns a failure result with an Unexpected error.
    /// </summary>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="operation">The asynchronous operation to apply to the current value if the result is successful.</param>
    /// <returns>A task containing a result with the new value if successful, or an error if the operation throws or the current result is failed.</returns>
    public async Task<Result<U>> TryAsync<U>(Func<T, Task<U>> operation)
    {
        if (IsFailure) return Error;

        try
        {
            return await operation(Value).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return Error.Unexpected("EXCEPTION", ex.Message)
                .WithMetadata("ExceptionType", ex.GetType().Name);
        }
    }

    /// <summary>
    /// Asynchronously transforms the value in a successful result by applying an operation that may throw an exception, using a custom error mapper.
    /// If the current result is a failure, the error is propagated without executing the operation.
    /// If an exception is thrown during the operation, the error mapper is called to create the error.
    /// </summary>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="operation">The asynchronous operation to apply to the current value if the result is successful.</param>
    /// <param name="errorMapper">A function to convert the exception to an Error.</param>
    /// <returns>A task containing a result with the new value if successful, or an error if the operation throws or the current result is failed.</returns>
    public async Task<Result<U>> TryAsync<U>(Func<T, Task<U>> operation, Func<Exception, Error> errorMapper)
    {
        if (IsFailure) return Error;

        try
        {
            return await operation(Value).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return errorMapper(ex);
        }
    }
}

/// <summary>
/// Provides extension methods for async operations on Task-wrapped Results with exception handling.
/// </summary>
public static class TryExtensions
{
    /// <summary>
    /// Asynchronously applies an exception-throwing operation on a Task-wrapped Result with a synchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the input result.</typeparam>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="result">The task containing the result to operate on.</param>
    /// <param name="operation">The operation to apply to the current value if the result is successful.</param>
    /// <returns>A task containing a result with the new value if successful, or an error if the operation throws or the current result is failed.</returns>
    public static async Task<Result<U>> TryAsync<T, U>(
        this Task<Result<T>> result, Func<T, U> operation) =>
            (await result.ConfigureAwait(false)).Try(operation);

    /// <summary>
    /// Asynchronously applies an exception-throwing operation on a Task-wrapped Result with a synchronous function and custom error mapper.
    /// </summary>
    /// <typeparam name="T">The type of the value in the input result.</typeparam>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="result">The task containing the result to operate on.</param>
    /// <param name="operation">The operation to apply to the current value if the result is successful.</param>
    /// <param name="errorMapper">A function to convert the exception to an Error.</param>
    /// <returns>A task containing a result with the new value if successful, or an error if the operation throws or the current result is failed.</returns>
    public static async Task<Result<U>> TryAsync<T, U>(
        this Task<Result<T>> result, Func<T, U> operation, Func<Exception, Error> errorMapper) =>
            (await result.ConfigureAwait(false)).Try(operation, errorMapper);

    /// <summary>
    /// Asynchronously applies an exception-throwing operation on a Task-wrapped Result with an asynchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the input result.</typeparam>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="result">The task containing the result to operate on.</param>
    /// <param name="operation">The asynchronous operation to apply to the current value if the result is successful.</param>
    /// <returns>A task containing a result with the new value if successful, or an error if the operation throws or the current result is failed.</returns>
    public static async Task<Result<U>> TryAsync<T, U>(
        this Task<Result<T>> result, Func<T, Task<U>> operation) =>
            await (await result.ConfigureAwait(false)).TryAsync(operation).ConfigureAwait(false);

    /// <summary>
    /// Asynchronously applies an exception-throwing operation on a Task-wrapped Result with an asynchronous function and custom error mapper.
    /// </summary>
    /// <typeparam name="T">The type of the value in the input result.</typeparam>
    /// <typeparam name="U">The type of the value in the returned result.</typeparam>
    /// <param name="result">The task containing the result to operate on.</param>
    /// <param name="operation">The asynchronous operation to apply to the current value if the result is successful.</param>
    /// <param name="errorMapper">A function to convert the exception to an Error.</param>
    /// <returns>A task containing a result with the new value if successful, or an error if the operation throws or the current result is failed.</returns>
    public static async Task<Result<U>> TryAsync<T, U>(
        this Task<Result<T>> result, Func<T, Task<U>> operation, Func<Exception, Error> errorMapper) =>
            await (await result.ConfigureAwait(false)).TryAsync(operation, errorMapper).ConfigureAwait(false);
}
